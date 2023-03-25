using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using DbUp.Support;

namespace DbUp.HashJournal.Core
{
    public class HashFilter : IScriptFilter
    {
        public HashFilter(string nameHashSeparator = HashJournalDefaults.NameHashSeparator)
        {
            NameHashSeparator = nameHashSeparator;
        }
        /// <summary>
        /// Separator between name and hash value in executedScriptNames
        /// </summary>
        protected string NameHashSeparator { get; private set; }

        public IEnumerable<SqlScript> Filter(IEnumerable<SqlScript> sorted, HashSet<string> executedScriptNames,
            ScriptNameComparer comparer)
            => sorted.Where(s =>
            {
                if (s.SqlScriptOptions.ScriptType == ScriptType.RunAlways)
                {
                    return true;
                }

                var hash = MD5Hasher.GetHash(s.Contents);
                var scriptNameToCheck = s.Name + NameHashSeparator + hash;
                return !executedScriptNames.Contains(scriptNameToCheck, comparer);
            });
    }
}
