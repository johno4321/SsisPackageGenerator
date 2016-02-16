// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     10-05-2010
// Desc     :     Class that holds data specific to an RMS import (if any)
// 
namespace RenRe.Doris.Data.Ssis
{
    public class RmsSsisImportScripter : SsisImportScripter
    {
        public RmsSsisImportScripter(string name, string importSchemaName, string path, string sourceConnectionString,
                                  string destinationConnectionString, string stagingDatabaseConnectionString, long defaultImportRunId, string defaultSourceIds, string logFileDir,
                                  ImportSchema schema)
            : base(name, importSchemaName, path, sourceConnectionString, destinationConnectionString, stagingDatabaseConnectionString, defaultImportRunId, defaultSourceIds, logFileDir, schema)
        {
        }
    }
}
