using System;
using System.Collections.Generic;
using System.IO;

namespace Obo
{
    public static class OboWriter
    {
        public static void Write(IDictionary<string, DotNode> termNameToNode, string path, string rootTerm)
        {
            Console.Write("- writing dot file... ");

            var             visitedNames       = new HashSet<string>();
            HashSet<string> supportedTermNames = GetSupportedConsequences();

            CheckSupportedConsequences(termNameToNode, supportedTermNames);

            using (var writer = new DotWriter(path))
            {
                DumpNodeList(writer, supportedTermNames, termNameToNode);
                DumpRoot(rootTerm, writer, visitedNames, termNameToNode);
            }

            Console.WriteLine("finished.");
        }

        private static void CheckSupportedConsequences(IDictionary<string, DotNode> termNameToNode,
                                                       IEnumerable<string> supportedTermNames)
        {
            var foundError = false;

            foreach (string termName in supportedTermNames)
            {
                if (termNameToNode.ContainsKey(termName)) continue;
                Console.WriteLine($"ERROR: Found unknown term name ({termName})");
                foundError = true;
            }

            if (foundError)
            {
                throw new InvalidDataException("Found unknown supported consequences.");
            }
        }

        private static HashSet<string> GetSupportedConsequences() =>
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

        private static void DumpNodeList(DotWriter writer, IEnumerable<string> supportedTermNames,
                                         IDictionary<string, DotNode> termNameToNode)
        {
            foreach (string termName in supportedTermNames)
            {
                if (termNameToNode.ContainsKey(termName)) writer.WriteFoundNode(termName);
                else writer.WriteMissingNode(termName);
            }
        }

        private static void DumpRoot(string rootName, DotWriter writer, ISet<string> visitedNames,
                                     IDictionary<string, DotNode> termNameToNode)
        {
            if (visitedNames.Contains(rootName)) return;

            if (!termNameToNode.TryGetValue(rootName, out DotNode node))
            {
                throw new InvalidDataException($"Unable to find {rootName}");
            }

            visitedNames.Add(rootName);

            if (node.ToNames.Count != 0) writer.Write(node);

            foreach (string childName in node.ToNames) DumpRoot(childName, writer, visitedNames, termNameToNode);
        }
    }
}