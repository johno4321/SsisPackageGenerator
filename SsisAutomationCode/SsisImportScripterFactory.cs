// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     10-05-2010
// Desc     :     Gets the appropriate SsisImportScripter based on the importName parameter
//
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis
{
    public class SsisImportScripterFactory
    {
        private const string RmsImportName = "RmsImport";
        private const string LoranImportName = "LoranImport";

        public SsisImportScripter GetSsisImportScripter(string importName, string importSchemaName, string path, string sourceConnectionString,
                                  string destinationConnectionString, string stagingDatabaseConnectionString, long defaultImportRunId, string defaultSourceIds, string logFileDir,
                                  ImportSchema schema, params object[] additionalParameters)
        {
            switch (importName)
            {
                case LoranImportName:
                    return new LoranSsisImportScripter
                    (
                        importName,
                        importSchemaName, 
                        path, 
                        sourceConnectionString,
                        destinationConnectionString, 
                        stagingDatabaseConnectionString,
                        defaultImportRunId,
                        defaultSourceIds,
                        logFileDir,
                        schema
                    );

                case RmsImportName:
                    return new RmsSsisImportScripter
                    (
                        importName,
                        importSchemaName,
                        path,
                        sourceConnectionString,
                        destinationConnectionString,
                        stagingDatabaseConnectionString,
                        defaultImportRunId,
                        defaultSourceIds,
                        logFileDir,
                        schema
                    );
                    
            }

            throw new SsisScripterException("I dont create scripts for import schemas called " + importSchemaName);
        }
    }
}
