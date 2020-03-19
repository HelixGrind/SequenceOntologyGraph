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

        public void WriteMissingNode(string nodeName) => _writer.WriteLine("\t\"{0}\" [style=filled, fillcolor=salmon];", nodeName);

        public void WriteFoundNode(string nodeName) => _writer.WriteLine("\t\"{0}\" [style=filled, fillcolor=palegreen];", nodeName);

        public void Write(DotNode dotNode) => _writer.WriteLine(dotNode);

        public void Dispose()
        {
            Close();
            _writer.Dispose();
        }
    }
}
