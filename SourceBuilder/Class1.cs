using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceBuilding
{
    public class SourceBuilder
    {
        public void Build(IEnumerable<string> sourceFiles)
        {
            var trees = sourceFiles.Select(s => CSharpSyntaxTree.ParseText(s));
            var assembly = typeof(object).Assembly;
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create("testassembly", trees, new[] { mscorlib }, options);

            using (var compilationStream = new MemoryStream())
            using (var debugStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(compilationStream, debugStream);
                if (!emitResult.Success) throw new Exception();
                var newAssembly = Assembly.Load(compilationStream.ToArray());
                var types = newAssembly.GetTypes();
            }
        }
    }
}