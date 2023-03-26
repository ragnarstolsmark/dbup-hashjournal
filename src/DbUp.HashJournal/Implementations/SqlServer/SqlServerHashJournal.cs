using System;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;

namespace DbUp.HashJournal.Implementations.SqlServer
{
    /// <summary>
    /// An implementation of the <see cref="Engine.IJournal"/> interface which tracks changes to scripts numbers for a 
    /// SQL Server database using a table called dbo.HashVersions.
    /// </summary>
    public class SqlServerHashJournal : HashJournal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerHashJournal"/> class.
        /// </summary>
        /// <param name="connectionManager">The connection manager.</param>
        /// <param name="logger">The log.</param>
        /// <param name="schema">The schema that contains the table.</param>
        /// <param name="table">The table name.</param>
        public SqlServerHashJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> logger, string schema, string table)
            : base(connectionManager, logger, new SqlServerObjectParser(), schema, table)
        {
        }

        protected override string GetInsertJournalEntrySql(string @scriptName, string @applied, string @hash)
        {
            return $"insert into {FqSchemaTableName} (ScriptName, Applied, Hash) values ({@scriptName}, {@applied}, {@hash})";
        }
        protected override string GetJournalEntriesSql()
        {
            return $@"SELECT [ScriptName], [Hash]
    FROM {FqSchemaTableName}
    WHERE [Id] IN (
        SELECT MAX([Id])
        FROM {FqSchemaTableName}
        GROUP BY [ScriptName]
    )
    ORDER BY [ScriptName]";
        }

        protected override string CreateSchemaTableSql(string quotedPrimaryKeyName)
        {
            return
                $@"create table {FqSchemaTableName} (
    [Id] int identity(1,1) not null constraint {quotedPrimaryKeyName} primary key,
    [ScriptName] nvarchar(255) not null,
    [Hash] char(32) not null,
    [Applied] datetime not null
)";
        }
    }
}
