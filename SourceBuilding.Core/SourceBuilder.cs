using System;
using System.Collections.Generic;
using System.Data.Common;
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
            var trees = sourceFiles.Select(s => CSharpSyntaxTree.ParseText(s));
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location);
            var system = MetadataReference.CreateFromFile(typeof(ISet<>).GetTypeInfo().Assembly.Location);
            var systemDataCommon = MetadataReference.CreateFromFile(typeof(DbConnection).GetTypeInfo().Assembly.Location);
            var generics = MetadataReference.CreateFromFile(typeof(HashSet<>).GetTypeInfo().Assembly.Location);
            var efCore = MetadataReference.CreateFromFile(typeof(DbContext).GetTypeInfo().Assembly.Location);
            var efRelational = MetadataReference.CreateFromFile(typeof(RelationalIndexBuilderExtensions).GetTypeInfo().Assembly.Location);
            var efSql = MetadataReference.CreateFromFile(typeof(SqlServerDbContextOptionsExtensions).GetTypeInfo().Assembly.Location);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create("testassembly", trees, new[] { mscorlib, system, systemDataCommon,
                generics, efCore, efRelational, efSql }, options);

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
    }
}