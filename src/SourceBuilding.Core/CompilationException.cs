using System;

namespace SourceBuilding.Core
{
    public class CompilationException : Exception
    {
        public CompilationException(string message) : base(message)
        {

        }
    }
}