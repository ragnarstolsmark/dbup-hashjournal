using DbUp.Builder;
using DbUp.HashJournal.Implementations.SqlServer;

namespace DbUp.HashJournal
{
    public static class UpgradeEngineBuilderExtensions
    {
        public static UpgradeEngineBuilder UseSqlServerHashJournal(this UpgradeEngineBuilder builder, string schema)
        {
            builder.Configure(c => c.Journal = new SqlServerHashJournal(() => c.ConnectionManager, () => c.Log, schema, "HashVersions"));
            builder.Configure(c => c.ScriptFilter = new HashFilter());
            return builder;
        }
    }
}
