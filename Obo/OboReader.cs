using System;
using System.Collections.Generic;
using System.IO;

namespace Obo
{
    public class OboReader : IDisposable
    {
        private readonly StreamReader _streamReader;

        public OboReader(string dbPath)
        {
            _streamReader = new StreamReader(new FileStream(dbPath, FileMode.Open, FileAccess.Read, FileShare.Read));

            while (true)
            {
                string line = _streamReader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;
            }
        }

        public Term GetNextTerm()
        {
            string line = _streamReader.ReadLine();
            if (line == null) return null;

            if (line == "[Typedef]") return null;

            if (line != "[Term]") throw new ApplicationException(line);

            string name       = null;
            string id         = null;
            var    parentIds  = new List<string>();
            var    isObsolete = false;

            while (true)
            {
                line = _streamReader.ReadLine();
                if (string.IsNullOrEmpty(line)) break;

                string[] cols  = line.Split(' ');
                string   key   = cols[0];
                string   value = cols[1];

                switch (key)
                {
                    case "id:":
                        id = value;
                        break;
                    case "name:":
                        name = value;
                        break;
                    case "is_a:":
                        parentIds.Add(value);
                        break;
                    case "is_obsolete:":
                        if (value == "true") isObsolete = true;
                        break;
                }
            }

            if (name == null || id == null) throw new ApplicationException(line);

            return new Term(name, id, parentIds, isObsolete);
        }

        public void Dispose() => _streamReader?.Dispose();
    }
}