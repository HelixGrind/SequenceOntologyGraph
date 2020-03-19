using System.Collections.Generic;
using System.Linq;

namespace Obo
{
    public class DotNode
    {
        public readonly string        Name;
        public readonly List<DotNode> Children = new List<DotNode>();
        public readonly List<DotNode> Parents  = new List<DotNode>();

        public Status Status;

        public DotNode(string name) => Name = name;

        public override string ToString()
        {
            List<string> childNames = (from childNode in Children where childNode.Status != Status.Pruned select $"\"{childNode.Name}\"").ToList();
            if (childNames.Count == 0) return null;

            string       joinedNames = string.Join(", ", childNames);
            return $"\t\"{Name}\" -> {{ {joinedNames} }};";
        }
    }
}