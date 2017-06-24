using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SourceBuilding
{
    public class SourceBuilder
    {
        public void Build(IEnumerable<string> sourceFiles)
        {
            System.ComponentModel.IListSource x;
            System.Linq.IQueryable<int> y;
            var trees = sourceFiles.Select(s => CSharpSyntaxTree.ParseText(s));
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var system = MetadataReference.CreateFromFile(typeof(ISet<>).Assembly.Location);
            var systemDataCommon = MetadataReference.CreateFromFile(typeof(DbConnection).Assembly.Location);
            var generics = MetadataReference.CreateFromFile(typeof(HashSet<>).Assembly.Location);
            var efCore = MetadataReference.CreateFromFile(typeof(DbContext).Assembly.Location);
            var efRelational = MetadataReference.CreateFromFile(typeof(RelationalIndexBuilderExtensions).Assembly.Location);
            var efSql = MetadataReference.CreateFromFile(typeof(SqlServerDbContextOptionsExtensions).Assembly.Location);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create("testassembly", trees, new[] { mscorlib, system, systemDataCommon, generics, efCore, efRelational, efSql }, options);

            using (var compilationStream = new MemoryStream())
            using (var debugStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(compilationStream, debugStream);
                var errorListText = string.Join(Environment.NewLine, emitResult.Diagnostics.Select(d => d.ToString()));
                if (!emitResult.Success) throw new Exception();
                var assemblyBytes = compilationStream.ToArray();
                //if (assemblyBytes.Length != x.Length) throw new Exception();
                //for (var i = 0; i < x.Length; i++)
                //{
                //    if (x[i] != assemblyBytes[i])
                //        continue;
                //}
                //var areEqual = x.SequenceEqual(assemblyBytes);
                //var newAssembly = Assembly.Load(fileAssemblyBytes);

                //byte[] dllAsArray;
                //using (var stream = new MemoryStream())
                //{
                //    var formatter = new BinaryFormatter();

                //    formatter.Serialize(stream, newAssembly);

                //    dllAsArray = stream.ToArray();
                //}
                //var types = newAssembly.GetTypes();
                File.WriteAllBytes("C:\\temp\\gen.dll", assemblyBytes);
                //var context = Activator.CreateInstance(types.Single(t => t.Name.Equals("generatedContext",
                //        StringComparison.InvariantCultureIgnoreCase)));
            }
        }
    }
}