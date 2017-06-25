using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace SourceBuilding
{
    public class SourceBuilder
    {
        public byte[] Build(IEnumerable<string> sourceFiles)
        {
            var trees = sourceFiles.Select(s => CSharpSyntaxTree.ParseText(s));
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var system = MetadataReference.CreateFromFile(typeof(ISet<>).Assembly.Location);
            var systemDataCommon = MetadataReference.CreateFromFile(typeof(DbConnection).Assembly.Location);
            var generics = MetadataReference.CreateFromFile(typeof(HashSet<>).Assembly.Location);
            var efCore = MetadataReference.CreateFromFile(typeof(DbContext).Assembly.Location);
            var efRelational = MetadataReference.CreateFromFile(typeof(RelationalIndexBuilderExtensions).Assembly.Location);
            var efSql = MetadataReference.CreateFromFile(typeof(SqlServerDbContextOptionsExtensions).Assembly.Location);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create("testassembly", trees, new[] { mscorlib, system, systemDataCommon,
                generics, efCore, efRelational, efSql }, options);

            using (var compilationStream = new MemoryStream())
            using (var debugStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(compilationStream, debugStream);
                var errorListText = string.Join(Environment.NewLine, emitResult.Diagnostics.Select(d => d.ToString()));
                if (!emitResult.Success) throw new Exception();
                var assemblyBytes = compilationStream.ToArray();
                return assemblyBytes;
            }
        }
    }
}