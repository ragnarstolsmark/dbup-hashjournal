using System;
using System.Data;
using DbUp.HashJournal.Tests.Common;

namespace DbUp.Tests.Common.RecordingDb
{
    class RecordingDbTransaction : IDbTransaction
    {
        readonly CaptureLogsLogger logger;

        public RecordingDbTransaction(CaptureLogsLogger logger)
        {
            this.logger = logger;
        }

        public void Dispose()
        {
            logger.WriteDbOperation("Dispose transaction");
        }

        public void Commit()
        {
            logger.WriteDbOperation("Commit transaction");
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public IDbConnection? Connection { get; private set; }
        public IsolationLevel IsolationLevel { get; private set; }
    }
}
