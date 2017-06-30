using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace SourceBuilding.Core
{
    public class SourceBuilder
    {
        public byte[] Build(IEnumerable<string> sourceFiles)
        {
            var efSqlAssembly = typeof(SqlServerDbContextOptionsExtensions).GetTypeInfo().Assembly;
            var assemblyLocations = GetAssemblyLocations(efSqlAssembly);
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
        }

        private IEnumerable<string> GetAssemblyLocations(Assembly assembly)
        {
            var assemblies = ImmutableHashSet.Create(assembly.Location);
            var subAssemblyNames = assembly.GetReferencedAssemblies();
            return subAssemblyNames.Aggregate(assemblies, (current, subAssemblyName) => current.Add(Assembly.Load(subAssemblyName).Location));
        }
    }
}