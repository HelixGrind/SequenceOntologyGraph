using System;
using System.Collections.Generic;

namespace Obo
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine($"{Utilities.GetProgramName()} <input obo path> <output dot path> <root term>");
                Environment.Exit(1);
            }

            string inputPath  = args[0];
            string outputPath = args[1];
            string rootTerm   = args[2];

            IDictionary<string, DotNode> termNameToNode = OboParser.Load(inputPath);
            OboWriter.Write(termNameToNode, outputPath, rootTerm);
        }
    }
}