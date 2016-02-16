// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.OleDb
{
    public abstract class OleDbSourceDataFlowComponent : OleDbDataFlowComponent
    {
        protected const string ACCESS_MODE = "AccessMode"; //the type of Data acces mode this source is going to use

        //the different options for the access mode property
        public enum OleDbSourceAccessMode
        {
            TableOrView = 0,
            TableOrViewVariable = 1,
            SqlCommand = 2,
            SqlCommandVariable = 3,
        }

        protected OleDbSourceDataFlowComponent(string name, IDTSObjectHost container, ISsisConnectionManager connectionManager)
            : base(name, container, connectionManager)
        {
            ComponentType = SsisConstants.SsisComponentOleDbSourceComponentType;
            Name = "OLEDBSource";
        }
    }
}