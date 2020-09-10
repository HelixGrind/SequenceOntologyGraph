using System;
using System.Collections.Generic;

namespace Obo
{
    public static class OboWriter
    {
        public static void Write(DotNode rootNode, string path, bool pruned)
        {
            Console.Write("- writing dot file... ");

            var visitedNames = new HashSet<string>();
            
            using (var writer = new DotWriter(path))
            {
                rootNode.DumpNodeColoring(writer, visitedNames, pruned);
                visitedNames.Clear();
                rootNode.Dump(writer, visitedNames);
            }

            Console.WriteLine("finished.");
        }

        private const string LightGreen = "#D6ECD2";
        private const string DarkGreen  = "#99D18F";
        private const string White      = "#FFFFFF";

        private static void DumpNodeColoring(this DotNode node, DotWriter writer, ISet<string> visitedNames,
                                             bool pruned)
        {
            if (visitedNames.Contains(node.Name)) return;
            visitedNames.Add(node.Name);

            switch (node.Status)
            {
                case Status.Covered:
                    writer.WriteColoredNode(node.Name, LightGreen);
                    break;
                case Status.Supported:
                    writer.WriteColoredNode(node.Name, DarkGreen);
                    break;
                default:
                    if (!pruned) writer.WriteColoredNode(node.Name, White);
                    break;
            }

            foreach (DotNode childNode in node.Children) childNode.DumpNodeColoring(writer, visitedNames, pruned);
        }

        private static void Dump(this DotNode node, DotWriter writer, ISet<string> visitedNames)
        {
            if (node.Status == Status.Pruned || visitedNames.Contains(node.Name)) return;
            visitedNames.Add(node.Name);
            
            writer.Write(node);
            foreach (DotNode childNode in node.Children)
                if (childNode.Status != Status.Pruned) childNode.Dump(writer, visitedNames);
        }
    }
}