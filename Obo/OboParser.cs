using System;
using System.Collections.Generic;

namespace Obo
{
    public static class OboParser
    {
        public static IDictionary<string, DotNode> Load(string path)
        {
            (List<Term> terms, Dictionary<string, DotNode> termNameToNode, Dictionary<string, string> termIdToName) = ParseOboFile(path);
            AddChildrenToNodes(terms, termNameToNode, termIdToName);
            return termNameToNode;
        }

        private static (List<Term> Terms, Dictionary<string, DotNode> TermNameToNode,
            Dictionary<string, string> TermIdToName) ParseOboFile(string path)
        {
            Console.Write("- parsing obo file... ");

            var terms          = new List<Term>();
            var termNameToNode = new Dictionary<string, DotNode>();
            var termIdToName   = new Dictionary<string, string>();

            using (var reader = new OboReader(path))
            {
                while (true)
                {
                    Term term = reader.GetNextTerm();
                    if (term == null) break;

                    if (!term.IsObsolete) termNameToNode[term.Name] = new DotNode(term.Name);

                    terms.Add(term);
                    termIdToName[term.Id] = term.Name;
                }
            }

            Console.WriteLine("{0} terms added.", terms.Count);

            return (terms, termNameToNode, termIdToName);
        }

        private static void AddChildrenToNodes(IEnumerable<Term> terms, IReadOnlyDictionary<string, DotNode> termNameToNode,
                                               IReadOnlyDictionary<string, string> termIdToName)
        {
            Console.Write("- adding children to all parent nodes... ");
            var numChildrenAdded = 0;

            foreach (Term term in terms)
            {
                if (term.IsObsolete || term.ParentIds.Count == 0) continue;

                foreach (string id in term.ParentIds)
                {
                    string parentName = termIdToName[id];
                    if (!termNameToNode.TryGetValue(parentName, out DotNode node)) continue;
                    
                    node.ToNames.Add(term.Name);
                    numChildrenAdded++;
                }
            }

            Console.WriteLine("{0} children added.", numChildrenAdded);
        }
    }
}