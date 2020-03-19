using System.Collections.Generic;
using System.Linq;

namespace Obo
{
    public class DotNode
    {
        private readonly string          _fromName;
        public readonly  HashSet<string> ToNames;

        public DotNode(string fromName)
        {
            _fromName = fromName;
            ToNames   = new HashSet<string>();
        }

        public override string ToString()
        {
            List<string> toNames = ToNames.ToList();
            if (toNames.Count == 1) return $"\t\"{_fromName}\" -> \"{toNames[0]}\";";

            var newToNames = new List<string>();
            foreach (string toName in toNames) newToNames.Add($"\"{toName}\"");

            string joinedNames = string.Join(", ", newToNames);
            return $"\t\"{_fromName}\" -> {{ {joinedNames} }};";
        }
    }
}