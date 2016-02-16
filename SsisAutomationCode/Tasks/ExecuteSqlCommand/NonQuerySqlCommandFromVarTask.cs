// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     10-05-2010
// Desc     :     Models an SSIS SqlCommand task that executes a non result set returning query
// 
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Tasks.ExecuteSQLTask;

namespace RenRe.Doris.Data.Ssis.Tasks.ExecuteSqlCommand
{
    public class NonQuerySqlCommandTask : SqlCommandTask
    {
        public NonQuerySqlCommandTask(string name, Package container, Application dtsApplication, ConnectionManager connectionManager, string commandText, bool isStoredProcedure)
            : base(name, container, dtsApplication, connectionManager, commandText, isStoredProcedure)
        {
        }
        
        public override void ConstructComponent()
        {
            base.ConstructComponent();
            
            var executeSqlTask = SqlCommandTaskHost.InnerObject as ExecuteSQLTask;

            IsValid = true;
        }
    }
}
