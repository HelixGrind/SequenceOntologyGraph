using System;
using System.Collections.Generic;
using System.IO;

namespace Obo
{
    public static class OboParser
    {
        public static DotNode Load(string path, string rootTerm)
        {
            (List<Term> terms, List<DotNode> nodes) = GetTerms(path);

            Dictionary<string, DotNode> termNameToNode = GetTermNameToNode(nodes);
            Dictionary<string, string>  termIdToName   = GetTermIdToName(terms);
            AddChildrenToNodes(terms, termNameToNode, termIdToName);

            IEnumerable<string> supportedTermNames = GetSupportedTermNames();
            CheckSupportedTermNames(supportedTermNames, termNameToNode);
            
            return GetRootNode(nodes, rootTerm);
        }

        private static void CheckSupportedTermNames(IEnumerable<string> supportedTermNames,
                                                    IReadOnlyDictionary<string, DotNode> termNameToNode)
        {
            var foundError = false;

            foreach (string termName in supportedTermNames)
            {
                if (termNameToNode.TryGetValue(termName, out DotNode node))
                {
                    node.Status = Status.Supported;
                    ColorCoveredParents(node);
                }
                else
                {
                    Console.WriteLine($"ERROR: Found unknown term name ({termName})");
                    foundError = true;
                }
            }

            if (foundError) throw new InvalidDataException("Found unknown supported consequences.");
        }

        private static void ColorCoveredParents(DotNode node)
        {
            if (node.Status == Status.Covered) return;
            if (node.Status == Status.None) node.Status = Status.Covered;
            foreach (DotNode parentNode in node.Parents) ColorCoveredParents(parentNode);
        }

        private static (List<Term> Terms, List<DotNode> nodes) GetTerms(string path)
        {
            Console.Write("- parsing obo file... ");

            var terms = new List<Term>();
            var nodes = new List<DotNode>();

            using (var reader = new OboReader(path))
            {
                while (true)
                {
                    Term term = reader.GetNextTerm();
                    if (term == null) break;
                    if (term.IsObsolete) continue;

                    terms.Add(term);
                    nodes.Add(new DotNode(term.Name));
                }
            }

            Console.WriteLine("{0} terms added.", terms.Count);
            return (terms, nodes);
        }

        private static Dictionary<string, string> GetTermIdToName(IEnumerable<Term> terms)
        {
            var termIdToName = new Dictionary<string, string>();

            foreach (Term term in terms)
            {
                if (termIdToName.ContainsKey(term.Id))
                    throw new InvalidDataException($"ERROR: The term list already has a node with the same id: ({term.Id})");

                termIdToName[term.Id] = term.Name;
            }

            return termIdToName;
        }

        private static Dictionary<string, DotNode> GetTermNameToNode(IEnumerable<DotNode> nodes)
        {
            var termNameToNode = new Dictionary<string, DotNode>();

            foreach (DotNode node in nodes)
            {
                if (termNameToNode.ContainsKey(node.Name))
                    throw new InvalidDataException($"ERROR: The node list already has a node with the same name: ({node.Name})");

                termNameToNode[node.Name] = node;
            }
            
            return termNameToNode;
        }

        private static DotNode GetRootNode(IEnumerable<DotNode> nodes, string rootTerm)
        {
            foreach (DotNode node in nodes)
            {
                if (node.Name == rootTerm) return node;
            }
            
            throw new InvalidDataException($"Could not find the root term ({rootTerm}) in the nodes!");
         }

        private static void AddChildrenToNodes(IEnumerable<Term> terms, IReadOnlyDictionary<string, DotNode> termNameToNode,
                                               IReadOnlyDictionary<string, string> termIdToName)
        {
            Console.Write("- adding children to nodes... ");
            var numChildrenAdded = 0;

            foreach (Term term in terms)
            {
                if (!termNameToNode.TryGetValue(term.Name, out DotNode childNode))
                    throw new InvalidDataException($"ERROR: Could not find the node for name ({term.Name})");

                foreach (string id in term.ParentIds)
                {
                    string parentName = termIdToName[id];
                    if (!termNameToNode.TryGetValue(parentName, out DotNode parentNode))
                        throw new InvalidDataException($"ERROR: Could not find the node for ID ({id}) and name ({parentName})");

                    parentNode.Children.Add(childNode);
                    childNode.Parents.Add(parentNode);
                    numChildrenAdded++;
                }
            }

            Console.WriteLine("{0} children added.", numChildrenAdded);
        }
        
        private static IEnumerable<string> GetSupportedTermNames() =>
            new HashSet<string>
            {
                "coding_sequence_variant",
                "copy_number_change",
                "copy_number_decrease",
                "copy_number_increase",
                "downstream_gene_variant",
                "feature_elongation",
                "5_prime_UTR_variant",
                "frameshift_variant",
                "incomplete_terminal_codon_variant",
                "inframe_deletion",
                "inframe_insertion",
                "intron_variant",
                "mature_miRNA_variant",
                "missense_variant",
                "NMD_transcript_variant",
                "non_coding_transcript_exon_variant",
                "non_coding_transcript_variant",
                "protein_altering_variant",
                "regulatory_region_ablation",
                "regulatory_region_amplification",
                "regulatory_region_variant",
                "short_tandem_repeat_change",
                "short_tandem_repeat_contraction",
                "short_tandem_repeat_expansion",
                "splice_acceptor_variant",
                "splice_donor_variant",
                "splice_region_variant",
                "start_lost",
                "start_retained_variant",
                "stop_gained",
                "stop_lost",
                "stop_retained_variant",
                "synonymous_variant",
                "3_prime_UTR_variant",
                "transcript_ablation",
                "transcript_amplification",
                "feature_truncation", // transcript_truncation
                "transcript_variant",
                "unidirectional_gene_fusion",
                "upstream_gene_variant"
            };
    }
}