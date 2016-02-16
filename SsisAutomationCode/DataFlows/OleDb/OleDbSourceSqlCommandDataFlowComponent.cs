// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.OleDb
{
    public class OleDbSourceSqlCommandDataFlowComponent : OleDbSourceDataFlowComponent
    {
        private readonly string _joinClause;
        private readonly Variable _portfolioIdVar;

        //TODO - the sql statement should be built outside of this class
        public OleDbSourceSqlCommandDataFlowComponent(string name, IDTSObjectHost container, ISsisConnectionManager connectionManager, string joinClause, Variable portfolioIdVar)
            : base(name, container, connectionManager)
        {
            _joinClause = joinClause;
            _portfolioIdVar = portfolioIdVar;
        }

        public override void ConstructComponent()
        {
            base.ConstructComponent();

            Instance.SetComponentProperty(ACCESS_MODE, OleDbSourceAccessMode.SqlCommand); //we want to use an SQL statment to get our data
            Instance.SetComponentProperty(SsisConstants.OleDbSourcePropertySqlCommand, _joinClause);

            //TODO - need to think about this - I probably want to pass in a dictionary<string, string> containing the mappings
            Instance.SetComponentProperty(SsisConstants.OleDbSourcePropertyParameterMapping, _portfolioIdVar.GetSqlCommandParameterMapping("Parameter0"));

            Instance.RefreshMetaData();

            Output = Component.OutputCollection[0];
            CanBeConnectedFrom = true;

            Input = null;
            CanBeConnectedTo = false;

            IsValid = true;
        }
    }
}