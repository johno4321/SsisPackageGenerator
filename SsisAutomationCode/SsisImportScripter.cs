// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;
using RenRe.Doris.Data.Ssis.Tasks;
using RenRe.Doris.Data.Ssis.Tasks.ExecuteSqlCommand;

namespace RenRe.Doris.Data.Ssis
{
    public abstract class SsisImportScripter : Scripter
    {
        private const string ImportRunIdVarName = "ImportRunId";
        private const string SourceIdsVarName = "SourceIds";

        private const string StagingConnectionName = "StagingConnection";
        private const string SourceConnectionName = "SourceConnection";
        private const string DestinationConnectionName = "DestinationConnection";
        
        private readonly string _path;
        private readonly string _sourceConnectionString;
        private readonly string _destinationConnectionString;
        private readonly string _stagingDatabaseConnection;
        private readonly long _defaultImportRunId;
        private readonly string _defaultSourceIds;
        private readonly string _logFileDir;

        public Dictionary<TaskComponent, TaskComponent> TaskConnections;

        public string ImportSchemaName { get; private set; }

        protected SsisImportScripter(string name, string importSchemaName, string path, string sourceConnectionString,
                                  string destinationConnectionString, string stagingDatabaseConnection, long defaultImportRunId, string defaultSourceIds, string logFileDir,
                                  ImportSchema schema) : base(schema)
        {
            Name = name;
            ImportSchemaName = importSchemaName;
            _path = path;
            _sourceConnectionString = sourceConnectionString;
            _destinationConnectionString = destinationConnectionString;
            _stagingDatabaseConnection = stagingDatabaseConnection;
            _defaultImportRunId = defaultImportRunId;
            _defaultSourceIds = defaultSourceIds;
            _logFileDir = logFileDir;
            TaskConnections = new Dictionary<TaskComponent, TaskComponent>();
        }

        public Application Application { get; private set; }
        public Package Package { get; private set; }
        
        public Variable ImportRunIdVar { get; private set; }
        public Variable SourceIdsVar { get; protected set; }

        //the previous Component this Component links to in the overall package
        public TaskComponent PreviousTaskComponent { get; set; }

        public ConnectionManager SourceConnection
        {
            get { return Package.Connections[SourceConnectionName]; }
        }

        public ConnectionManager DestinationConnection
        {
            get { return Package.Connections[DestinationConnectionName]; }
        }

        public ConnectionManager StagingConnection
        {
            get { return Package.Connections[StagingConnectionName]; }
        }

        protected override ScriptItem CreateScriptItem(ImportElement element)
        {
            return new SsisImportScriptItem(this, element);
        }

        protected override void Init()
        {
            Application = new Application();
            Package = new Package {Name = Name, PackageType = DTSPackageType.DTSDesigner100, DelayValidation = true};

            Package.AddTextLog(Name, Path.Combine(_logFileDir, string.Format("{0}Ssis.log", Name)));
            
            Package.SetStandardErrorLoggingOptions();

            Package.AddOledbConnection(SourceConnectionName, _sourceConnectionString);
            Package.AddOledbConnection(DestinationConnectionName, _destinationConnectionString);
            Package.AddAboNetConnection(StagingConnectionName, _stagingDatabaseConnection);

            ImportRunIdVar = Package.AddVariable(ImportRunIdVarName, _defaultImportRunId);
            SourceIdsVar = Package.AddVariable(SourceIdsVarName, _defaultSourceIds);
        }

        protected override void Finish()
        {
            TaskComponent lastTaskComponent = null;
            foreach (var task in TaskConnections.Keys)
            {
                task.ConnectTaskComponentTo(TaskConnections[task]);
                lastTaskComponent = task;
            }
            CreateSqlCommandTask(lastTaskComponent);

            Application.SaveToXml(_path, Package, null);
        }

        private void CreateSqlCommandTask(TaskComponent connectToThis)
        {
            const string sqlExpression = "\"Import.usp_UpdateImportRun \" + (DT_WSTR, 10)@[User::ImportRunId]";
            var importRunUpdateSqlVar = Package.AddVariable("UpdateImportRunSql", sqlExpression, false, true);
            var sqlCommandTask = new NonQuerySqlCommandFromVarExpressionTask("UpdateImportRun", Package, Application, StagingConnection, importRunUpdateSqlVar, false);
                                                    
            sqlCommandTask.ConstructComponent();

            if (!sqlCommandTask.IsValid)
            {
                throw new SsisScripterException("Just tried to create a NonQuerySqlCommandFromVarExpressionTask for this import but it is not valid");
            }

            sqlCommandTask.ConnectTaskComponentTo(connectToThis);
        }
    }
}