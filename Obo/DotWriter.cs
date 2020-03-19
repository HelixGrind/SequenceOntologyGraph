using System;
using System.IO;

namespace Obo
{
    public class DotWriter : IDisposable
    {
        private readonly StreamWriter _writer;

        public DotWriter(string dotPath)
        {
            _writer = new StreamWriter(dotPath) { NewLine = "\n" };
            _writer.WriteLine("digraph {");
        }
        
        private void Close()
        {
            _writer.WriteLine("}");
            _writer.Close();
        }

        public void WriteColoredNode(string nodeName, string color) =>
            _writer.WriteLine($"\t\"{nodeName}\" [style=filled, fillcolor=\"{color}\", fontname=\"Ebrima\"];");

        public void Write(DotNode dotNode)
        {
            var nodeString = dotNode.ToString();
            if (nodeString == null) return;
            _writer.WriteLine(nodeString);
        }

        public void Dispose()
        {
            Close();
            _writer.Dispose();
        }
    }
}
