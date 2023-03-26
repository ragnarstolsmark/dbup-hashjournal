using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using DbUp.Support;

namespace DbUp.HashJournal
{
    public class HashFilter : IScriptFilter
    {

        public IEnumerable<SqlScript> Filter(IEnumerable<SqlScript> sorted, HashSet<string> executedScriptNames,
            ScriptNameComparer comparer)
            => sorted.Where(s =>
            {
                if (s.SqlScriptOptions.ScriptType == ScriptType.RunAlways)
                {
                    return true;
                }

                var hash = MD5Hasher.GetHash(s.Contents);
                var scriptNameToCheck = s.Name + HashJournalConstants.NameHashSeparator + hash;
                return !executedScriptNames.Contains(scriptNameToCheck, comparer);
            });
    }
}
