// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Wrapper class around the SSIS sealed connection manager class
// 
using Microsoft.SqlServer.Dts.Runtime;

namespace RenRe.Doris.Data.Ssis.Scaffolding
{
    public class SsisConnectionManager : ISsisConnectionManager
    {
        public SsisConnectionManager() : this(null)
        {
        }

        public SsisConnectionManager(ConnectionManager connectionManager)
        {
            ConnectionManager = connectionManager;
        }

        public ConnectionManager ConnectionManager
        {
            get; private set;
        }

        public string ID
        {
            get { return ConnectionManager.ID; }
        }

        public Microsoft.SqlServer.Dts.Runtime.Wrapper.IDTSConnectionManager100 GetExtendedConnectionManager
        {
            get { return DtsConvert.GetExtendedInterface(ConnectionManager); }
        }
    }
}