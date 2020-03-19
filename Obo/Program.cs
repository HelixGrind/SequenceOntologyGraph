using System;

namespace Obo
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine($"{Utilities.GetProgramName()} <input obo path> <output dot path> <root term> <pruned Y/N>");
                Environment.Exit(1);
            }

            string inputPath  = args[0];
            string outputPath = args[1];
            string rootTerm   = args[2];
            bool   pruned     = args[3].ToLower().StartsWith("y");

            DotNode rootNode = OboParser.Load(inputPath, rootTerm);
            if (pruned) Pruner.PruneLevels(rootNode);
            Statistics.CalculateCoverage(rootNode);
            OboWriter.Write(rootNode, outputPath);
        }
    }
}