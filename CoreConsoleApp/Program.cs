using System;
using ScaffoldingCore;
using System.Linq;

namespace CoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new MyContext();
            var logsQueryable = context.Logs.Take(1);
            Console.WriteLine(logsQueryable);
        }
    }
}