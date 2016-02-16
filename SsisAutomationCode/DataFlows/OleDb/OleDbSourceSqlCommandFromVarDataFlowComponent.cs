// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Encapsulates an SSIS OleDbSource "box" that uses a variable to hold SQL that grabs data and passed it on to the next component
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.OleDb
{
    public class OleDbSourceSqlCommandFromVarDataFlowComponent : OleDbSourceDataFlowComponent
    {
        private readonly Variable _sqlStatement;

        //TODO - the sql statement should be built outside of this class
        public OleDbSourceSqlCommandFromVarDataFlowComponent(string name, IDTSObjectHost container, ISsisConnectionManager connectionManager, Variable sqlStatement)
            : base(name, container, connectionManager)
        {
            _sqlStatement = sqlStatement;
        }

        public override void ConstructComponent()
        {
            base.ConstructComponent();
            
            Instance.SetComponentProperty(ACCESS_MODE, OleDbSourceAccessMode.SqlCommandVariable); //we want to use a variable to hold an SQL statment to get our data
            Instance.SetComponentProperty(SsisConstants.OleDbSourcePropertySqlCommandVariable, _sqlStatement.QualifiedName);

            Instance.RefreshMetaData();

            Output = Component.OutputCollection[0];
            CanBeConnectedFrom = true;

            Input = null;
            CanBeConnectedTo = false;

            IsValid = true;
        }
    }
}