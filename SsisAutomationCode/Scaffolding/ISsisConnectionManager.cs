// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     interface for the SSIS sealed connection manager class
// 
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace RenRe.Doris.Data.Ssis.Scaffolding
{
    public interface ISsisConnectionManager
    {
        ConnectionManager ConnectionManager {get; }

        string ID { get; }

        IDTSConnectionManager100 GetExtendedConnectionManager { get; }
    }
}
