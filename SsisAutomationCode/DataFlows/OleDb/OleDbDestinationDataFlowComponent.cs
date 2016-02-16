// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     09-04-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.OleDb
{
    public abstract class OleDbDestinationDataFlowComponent : OleDbDataFlowComponent
    {
        public enum OleDbDestinationAccessMode
        {
            OpenRowset = 0,
            OpenRowsetFromVariable = 1,
            SqlCommand = 2,
            OpenRowsetUsingFastload = 3,
            OpenRowsetUsingFastloadFromVariable = 4
        }

        protected const string AccessMode = "AccessMode";
        protected string ImportSchemaName;

        protected OleDbDestinationDataFlowComponent(string name, IDTSObjectHost container, ISsisConnectionManager connectionManager, string importSchemaName)
            : base(name, container, connectionManager)
        {
            ImportSchemaName = importSchemaName;
            ComponentType = SsisConstants.SsisComponentOleDbDestinationComponentType;
            Name = "OLEDBDestination";
        }
    }
}