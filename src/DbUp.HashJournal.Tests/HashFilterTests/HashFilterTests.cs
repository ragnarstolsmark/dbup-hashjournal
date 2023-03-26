using DbUp.Engine;
using DbUp.Support;

namespace DbUp.HashJournal.Tests;
[UsesVerify]
public class HashFilterTests
{
    [Fact]
    public Task Should_run_all_scripts_if_no_script_has_run_before()
    {
        var scriptsToRun = new List<SqlScript>
        {
            new SqlScript("Delta01 - first script", "Insert into table1"),
            new SqlScript("Delta02 - second script", "Insert into table2"),
        };
        var hashFilter = new HashFilter();
        var scriptNameComparer = new ScriptNameComparer(StringComparer.Ordinal);
        var scriptsSelectedToRun = hashFilter.Filter(scriptsToRun, new HashSet<string>(), scriptNameComparer);
        return Verify(scriptsSelectedToRun.Select(s => s.Name));
    }
    [Fact]
    public Task Should_not_run_script_if_script_has_not_changed()
    {
        var script1 = new SqlScript("Delta01 - first script", "Insert into table1");
        var scriptsToRun = new List<SqlScript>
        {
            script1,
            new SqlScript("Delta02 - second script", "Insert into table2"),
        };
        var hashFilter = new HashFilter();
        var scriptNameComparer = new ScriptNameComparer(StringComparer.Ordinal);
        var alreadyExecutedScripts = new HashSet<string>()
        {
            script1.Name + HashJournalDefaults.NameHashSeparator + MD5Hasher.GetHash(script1.Contents)
        };
        var scriptsSelectedToRun = hashFilter.Filter(scriptsToRun, alreadyExecutedScripts, scriptNameComparer);
        return Verify(scriptsSelectedToRun.Select(s => s.Name));
    }
    [Fact]
    public Task Should_run_script_if_script_has_changed()
    {
        var script1 = new SqlScript("Delta01 - first script", "Insert into table1 --changed");
        var scriptsToRun = new List<SqlScript>
        {
            script1,
            new SqlScript("Delta02 - second script", "Insert into table2"),
        };
        var hashFilter = new HashFilter();
        var scriptNameComparer = new ScriptNameComparer(StringComparer.Ordinal);
        var alreadyExecutedScripts = new HashSet<string>()
        {
            script1.Name + HashJournalDefaults.NameHashSeparator + MD5Hasher.GetHash("Insert into table1")
        };
        var scriptsSelectedToRun = hashFilter.Filter(scriptsToRun, alreadyExecutedScripts, scriptNameComparer);
        return Verify(scriptsSelectedToRun.Select(s => s.Name));
    }
    [Fact]
    public Task Should_rerun_script_if_script_has_run_always()
    {
        var script1 = new SqlScript("Delta01 - first script", "Insert into table1", new SqlScriptOptions{ ScriptType = ScriptType.RunAlways});
        var scriptsToRun = new List<SqlScript>
        {
            script1,
            new SqlScript("Delta02 - second script", "Insert into table2"),
        };
        var hashFilter = new HashFilter();
        var scriptNameComparer = new ScriptNameComparer(StringComparer.Ordinal);
        var alreadyExecutedScripts = new HashSet<string>()
        {
            script1.Name + HashJournalDefaults.NameHashSeparator + MD5Hasher.GetHash(script1.Contents)
        };
        var scriptsSelectedToRun = hashFilter.Filter(scriptsToRun, alreadyExecutedScripts, scriptNameComparer);
        return Verify(scriptsSelectedToRun.Select(s => s.Name));
    }
}
