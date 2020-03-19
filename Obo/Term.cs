using System.Collections.Generic;

namespace Obo
{
    public class Term
    {
        public readonly string       Name;
        public readonly string       Id;
        public readonly List<string> ParentIds;
        public readonly bool         IsObsolete;

        public Term(string name, string id, List<string> parentIds, bool isObsolete)
        {
            Name       = name;
            Id         = id;
            ParentIds  = parentIds;
            IsObsolete = isObsolete;
        }

        public override string ToString() => $"Name: {Name}, ID: {Id}, Parents: {string.Join(",", ParentIds)}" +
                                             (IsObsolete ? " (obsolete)" : "");
    }
}