using System.Data;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.HashJournal.Implementations.SqlServer;
using DbUp.HashJournal.Tests.Common;
using DbUp.Tests.Common.RecordingDb;

namespace DbUp.HashJournal.Tests.HashJournalTests.SqlServerTests;
[UsesVerify]
public class SqlServerHashJournalTests
{
    [Fact]
    public Task Should_create_table_with_hash_column()
    {
        var logger = new CaptureLogsLogger();
        var scalarResults = new Dictionary<string?, Func<object>>
        {
            {"select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'HashVersions' and TABLE_SCHEMA = 'dbo'", () => 0}
        };
        var recordingDbCommand = new RecordingDbCommand(logger, null, "dbo", scalarResults, new Dictionary<string?, Func<int>>());
        var journal = new SqlServerHashJournal(null, () => logger, "dbo", "HashVersions");
        journal.EnsureTableExistsAndIsLatestVersion(() => recordingDbCommand);
        return Verify(recordingDbCommand.CommandText);
    }
}
