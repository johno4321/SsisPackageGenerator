// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.DerivedColumn
{
    public abstract class DerivedColumnDataFlowComponent : DataFlowComponent
    {
        protected const string Expression = "Expression";
        protected const string Friendlyexpression = "FriendlyExpression";

        protected Variable ColumnVariable;

        protected DerivedColumnDataFlowComponent(string name, IDTSObjectHost container, Variable columnVariable)
            : base(name, container)
        {
            ColumnVariable = columnVariable;
            ComponentType = SsisConstants.SsisComponentDerivedColumnComponentType;
        }
    }
}