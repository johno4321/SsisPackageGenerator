// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Cosntants used during the creation of SSIS packages
// 
namespace RenRe.Doris.Data.Ssis.Scaffolding
{
    public static class SsisConstants
    {
        //this is used as a token in dynamic SQL we build to indicate where we put dynamically created code in semi-static statements
        public const string DynamicSqlToken = "?";

        #region SSIS task id's
        public const string SsisComponentForEachLoopTaskType = "STOCK:FOREACHLOOP";
        public const string SsisComponentPipelineTaskTaskType = "STOCK:PipelineTask";
        public const string SsisComponentSqlCommandTaskType = "STOCK:SQLTask";
        #endregion SSIS task id's

        #region SSIS data flow component id's
        public const string SsisComponentOleDbSourceComponentType = "DTSAdapter.OLEDBSource.2";
        public const string SsisComponentOleDbDestinationComponentType = "DTSAdapter.OLEDBDestination.2";
        public const string SsisComponentDerivedColumnComponentType = "DTSTransform.DerivedColumn.2";
        #endregion

        #region Constants for creation of For Each loops
        public const string ForeachEventEnumeratorName = "Foreach Event Enumerator";
        public const string ForeachDatabaseEnumeratorName = "Foreach Database Enumerator";
        public const string ForeachFileEnumeratorName = "Foreach File Enumerator";
        public const string ForeachItemEnumeratorName = "Foreach Item Enumerator";
        public const string ForeachAdoEnumeratorName = "Foreach ADO Enumerator";
        public const string ForeachAdonetSchemaRowsetEnumeratorName = "Foreach ADO.NET Schema Rowset Enumerator";
        public const string ForeachFromVariableEnumeratorName = "Foreach From Variable Enumerator";
        public const string ForeachNodeListEnumeratorName = "Foreach NodeList Enumerator";
        public const string ForeachSmoEnumeratorName = "Foreach SMO Enumerator";
        #endregion

        #region Connection type constants
        public const string SsisConnectionOleDbConnectionId = "OLEDB";
        public const string SsisConnectionAdoNetSql = "ADO.NET:SQL";
        public const string FileConnectionId = "FILE";
        #endregion Connection type constants

        #region Common package variable namespaces
        public const string PackageVarUserNamespace = "User";
        #endregion

        #region OleDb Soruce constants
        public const string OleDbSourcePropertySqlCommand = "OleDbSourcePropertySqlCommand";
        public const string OleDbSourcePropertyParameterMapping = "OleDbSourcePropertyParameterMapping";
        public const string OleDbSourcePropertySqlCommandVariable = "SqlCommandVariable";
        #endregion

        #region Logging Providers
        public const string TextFileLoggingProvider = "DTS.LogProviderTextFile.2";
        #endregion

        #region Logging Events
        public const string LoggingEventOnError = "OnError";
        public const string LoggingEventOnTaskFailed = "OnTaskFailed";
        public const string LoggingEventOnWarning = "OnWarning";
        public const string LoggingEventOnPreExecute = "OnPreExecute";
        public const string LoggingEventOnPostExecute = "OnPostExecute";
        public const string LoggingEventOnInformation = "OnInformation";
        #endregion
    }
}