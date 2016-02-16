// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Models an SSIS SqlCommand task
// 
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.Tasks.ExecuteSqlCommand
{
    public abstract class SqlCommandTask : TaskComponent
    {
        #region Names of properties for the SSIS execute SQL task
        protected const string BypassPreparePropName = "BypassPrepare";
        protected const string CodePagePropName = "CodePage";
        protected const string ConnectionPropName = "Connection";
        protected const string ExecutionValuePropName = "ExecutionValue";
        protected const string IsStoredProcedurePropName = "IsStoredProcedure";
        protected const string ParameterBindingsPropName = "ParameterBindings";
        protected const string ResultSetBindingsPropName = "ResultSetBindings";
        protected const string ResultSetTypePropName = "ResultSetType";
        protected const string SqlStatementSourcePropName = "SqlStatementSource";
        protected const string SqlStatementSourceTypePropName = "SqlStatementSourceType";
        protected const string TimeOutPropName = "TimeOut";
        #endregion Names of properties for the SSIS execute SQL task

        private readonly bool _isStoredProcedure;
        private readonly ConnectionManager _connectionManager;

        protected readonly string CommandText;

        protected SqlCommandTask(string name, Package container, Application dtsApplication, ConnectionManager connectionManager, string commandText, bool isStoredProcedure)
            : base(name, container, dtsApplication)
        {
            ComponentType = SsisConstants.SsisComponentSqlCommandTaskType;
            
            _isStoredProcedure = isStoredProcedure;
            _connectionManager = connectionManager;
            CommandText = commandText;
        }

        protected TaskHost SqlCommandTaskHost
        {
            get; private set;
        }

        public override void ConstructComponent()
        {
            var executableInstance = Container.Executables.Add(ComponentType);
            Executable = executableInstance as IDTSObjectHost;

            SqlCommandTaskHost = executableInstance as TaskHost;

            if (SqlCommandTaskHost == null)
            {
                throw new SsisScripterException(
                    string.Format("Cannot cast the components executable to: {1}\nReal type is {0}",
                    executableInstance == null ? "NULL" : executableInstance.GetType().AssemblyQualifiedName, typeof(TaskHost).AssemblyQualifiedName));
            }
            
            SqlCommandTaskHost.Name = Name;

            var executeSqlTask = SqlCommandTaskHost.InnerObject as ExecuteSQLTask;

            if (executeSqlTask == null)
            {
                throw new SsisScripterException(
                    string.Format("Cannot cast the components executable to: {1}\nReal type is {0}", 
                    SqlCommandTaskHost.InnerObject == null ? "NULL" : SqlCommandTaskHost.InnerObject.GetType().AssemblyQualifiedName,
                    typeof(ExecuteSQLTask).AssemblyQualifiedName));
            }
            executeSqlTask.Connection = _connectionManager.Name;
            executeSqlTask.IsStoredProcedure = _isStoredProcedure;
        }
    }
}
