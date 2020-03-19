using System;
using System.Collections.Generic;

namespace Obo
{
    public static class Statistics
    {
        public static void CalculateCoverage(DotNode rootNode)
        {
            var stats = new Stats();
            CalculateStatistics(rootNode, stats);

            double percentage = (stats.NumCoveredNodes + stats.NumSupportedNodes) / (double) stats.NumNodes * 100.0;
            Console.WriteLine($"- {percentage:0.0} % of SO nodes covered.");
        }

        private static void CalculateStatistics(DotNode node, Stats stats)
        {
            if (stats.ObservedNames.Contains(node.Name)) return;
            stats.ObservedNames.Add(node.Name);

            switch (node.Status)
            {
                case Status.Covered:
                    stats.NumCoveredNodes++;
                    break;
                case Status.Supported:
                    stats.NumSupportedNodes++;
                    break;
            }

            stats.NumNodes++;

            foreach (DotNode childNode in node.Children) CalculateStatistics(childNode, stats);
        }
    }

    public class Stats
    {
        public int NumCoveredNodes;
        public int NumSupportedNodes;
        public int NumNodes;
        public readonly HashSet<string> ObservedNames = new HashSet<string>();
    }
}