// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.DerivedColumn
{
    public class DerivedInsertedColumnDataFlowComponent : DerivedColumnDataFlowComponent
    {
        public DerivedInsertedColumnDataFlowComponent(string name, IDTSObjectHost container, Variable columnVariable)
            : base(name, container, columnVariable)
        {
        }

        public override void ConstructComponent()
        {
            base.ConstructComponent();

            Instance = Component.InstantiateComponent();

            var importIdOutput = Component.OutputCollection[0];
            var importIdOutputColumn = Instance.InsertOutputColumnAt(importIdOutput.ID, 0, Name, Name);
            importIdOutputColumn.SetDataTypeProperties(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I8, 0, 0, 0, 0);
            Instance.SetOutputColumnProperty(importIdOutput.ID, importIdOutputColumn.ID, Expression, string.Empty);
            Instance.SetOutputColumnProperty(importIdOutput.ID, importIdOutputColumn.ID, Friendlyexpression, ColumnVariable.QualifiedName.GetSsisParameterForExpression());

            Instance.RefreshMetaData();

            Output = Component.OutputCollection[0];
            CanBeConnectedFrom = true;

            Input = Component.InputCollection[0];
            CanBeConnectedTo = true;

            IsValid = true;
        }
    }
}