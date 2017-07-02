using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Text;

namespace SourceBuilding.Core
{
    public class SourceBuilder
    {
        public byte[] Build(IEnumerable<string> sourceFiles, LibType libType)
        {
            return libType == LibType.Assembly ? BuildAssembly(sourceFiles) : BuildScript(sourceFiles);
        }

        public byte[] BuildAssembly(IEnumerable<string> sourceFiles)
        {
            var efSqlAssembly = typeof(SqlServerDbContextOptionsExtensions).GetTypeInfo().Assembly;
            var systemObject = typeof(object).GetTypeInfo().Assembly.Location;
            var coreDir = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);
            var mscorlib = Path.Combine(coreDir, "mscorlib.dll");
            var assemblyLocations = GetAssemblyLocations(efSqlAssembly).Add(systemObject).Add(mscorlib);
            var metadataReferences = assemblyLocations.Select(assemblyLocation => MetadataReference.CreateFromFile(assemblyLocation));
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var trees = sourceFiles.Select(s => CSharpSyntaxTree.ParseText(s));
            var compilation = CSharpCompilation.Create("generatedassembly", trees, metadataReferences, options);

            using (var compilationStream = new MemoryStream())
            using (var debugStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(compilationStream, debugStream);
                var errorListText = string.Join(Environment.NewLine, emitResult.Diagnostics.Select(d => d.ToString()));
                if (!emitResult.Success) throw new CompilationException("One or more errors occurred during compilation");
                var assemblyBytes = compilationStream.ToArray();
                return assemblyBytes;
            }

            ImmutableHashSet<string> GetAssemblyLocations(Assembly assembly)
            {
                var assemblies = ImmutableHashSet.Create(assembly.Location);
                var subAssemblyNames = assembly.GetReferencedAssemblies();
                return subAssemblyNames.Aggregate(assemblies, (current, subAssemblyName) => current.Add(Assembly.Load(subAssemblyName).Location));
            }
        }

        public byte[] BuildScript(IEnumerable<string> sourceFiles)
        {
            var compilations = sourceFiles.Select(s => CSharpSyntaxTree.ParseText(s).GetCompilationUnitRoot());
            var members = compilations.Select(c => GetNamespaceMembers(c)).SelectMany(m => m);
            var usings = compilations.Select(c => c.Usings).SelectMany(m => m);
            var newCompilation = CompilationUnit()
                .WithUsings(
                    List(new[] {
                        UsingDirective(IdentifierName("Microsoft.EntityFrameworkCore.Metadata"))
                            .WithUsingKeyword(
                                Token(
                                    TriviaList(
                                        Trivia(
                                            ReferenceDirectiveTrivia(
                                                Literal("nuget:NetStandard.Library,1.6.1"),
                                                true)),
                                        Trivia(
                                            ReferenceDirectiveTrivia(
                                                Literal("nuget:Microsoft.EntityFrameworkCore.SqlServer,1.1.2"),
                                                true))),
                                    SyntaxKind.UsingKeyword,
                                    TriviaList())),
                        UsingDirective(IdentifierName("Microsoft.EntityFrameworkCore"))}))
                        .AddUsings(usings.ToArray())
                .WithMembers(List(members))
                .NormalizeWhitespace();

            var text = newCompilation.NormalizeWhitespace().GetText().ToString();

            return Encoding.UTF8.GetBytes(text);

            IEnumerable<MemberDeclarationSyntax> GetNamespaceMembers(CompilationUnitSyntax compilation)
            {
                var @namespace = compilation.Members.OfType<NamespaceDeclarationSyntax>().Single();
                return @namespace.Members;
            }
        }
    }

    public enum LibType { Assembly, Script }
}