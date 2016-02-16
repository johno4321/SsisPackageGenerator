// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     10-05-2010
// Desc     :     Models an SSIS SqlCommand task that executes a non result set returning query
// 
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask;

namespace RenRe.Doris.Data.Ssis.Tasks.ExecuteSqlCommand
{
    public class NonQuerySqlCommandFromVarExpressionTask : SqlCommandTask
    {
        private readonly Variable _expression;
        public NonQuerySqlCommandFromVarExpressionTask(string name, Package container, Application dtsApplication, ConnectionManager connectionManager, Variable expression, bool isStoredProcedure)
            : base(name, container, dtsApplication, connectionManager, string.Empty, isStoredProcedure)
        {
            _expression = expression;
        }
        
        public override void ConstructComponent()
        {
            base.ConstructComponent();

            var executeSqlTask = SqlCommandTaskHost.InnerObject as ExecuteSQLTask;
            
            executeSqlTask.SqlStatementSourceType = SqlStatementSourceType.Variable;
            executeSqlTask.SqlStatementSource = _expression.QualifiedName;
            executeSqlTask.ResultSetType = ResultSetType.ResultSetType_None;

            IsValid = true;
        }
    }
}
