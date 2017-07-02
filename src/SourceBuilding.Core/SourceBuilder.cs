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

namespace SourceBuilding.Core
{
    public class SourceBuilder
    {
        public byte[] Build(IEnumerable<string> sourceFiles)
        {
            throw new NotImplementedException();
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

        public string BuildScript(IEnumerable<string> sourceFiles)
        {
            var members = sourceFiles.Select(s => GetNamespaceMembers(s)).SelectMany(m => m);
            var compilation = CompilationUnit()
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
                .WithMembers(List(members))
                .NormalizeWhitespace();

            var text = compilation.NormalizeWhitespace().GetText().ToString();

            return text;

            IEnumerable<MemberDeclarationSyntax> GetNamespaceMembers(string sourceFile)
            {
                var tree = CSharpSyntaxTree.ParseText(sourceFile).GetCompilationUnitRoot();
                var @namespace = tree.Members.OfType<NamespaceDeclarationSyntax>().Single();
                return @namespace.Members;
            }
        }
    }
}