// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.OleDb
{
    public abstract class OleDbDataFlowComponent : DataFlowComponent
    {
        protected ISsisConnectionManager ConnectionManager;
        protected string TableName;

        protected OleDbDataFlowComponent(string name, IDTSObjectHost container, ISsisConnectionManager connectionManager)
            : base(name, container)
        {
            ConnectionManager = connectionManager;
            TableName = name;
        }

        public override void ConstructComponent()
        {
            base.ConstructComponent();

            //Need to instantiate the Component here as the runtime connection collection will be empty if it's not
            Instance = Component.InstantiateComponent();

            Component.AttachConnection(ConnectionManager);
        }
    }
}