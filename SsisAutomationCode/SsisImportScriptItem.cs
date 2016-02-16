// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
//

using RenRe.Doris.Data.Ssis.DataFlows.DerivedColumn;
using RenRe.Doris.Data.Ssis.DataFlows.OleDb;
using RenRe.Doris.Data.Ssis.Scaffolding;
using RenRe.Doris.Data.Ssis.Tasks.DataFlow;

namespace RenRe.Doris.Data.Ssis
{
    public class SsisImportScriptItem : ScriptItem
    {   
        private DataFlowTaskComponent _dataFlowTaskComponent;
        private OleDbSourceSqlCommandFromVarDataFlowComponent _oleDbSourceSqlCommandFromVarDataFlowComponent;
        private DerivedInsertedColumnDataFlowComponent _derivedImportRunIdColumn;
        private OleDbDestinationFastLoadDataFlowComponent _oleDbDestinationDataFlowComponent;
        public SsisImportScripter Parent { get; private set; }

        public SsisImportScriptItem(ImportElement importElement)
            : this(null, importElement)
        {
        }

        public SsisImportScriptItem(SsisImportScripter parent, ImportElement importElement)
            : base (importElement)
        {
            Parent = parent;
        }

        public override void Generate()
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Create a data flow task component - this corresponds to the import element object in this SsisImportScriptItem
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            CreateDataFlowTaskComponent();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Create a ole db source data flow component - defines where our staging data comes from
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //CreateOleDbSourceSqlCommandDataFlowComponent();
            CreateOleDbSourceSqlCommandFromVarDataFlowComponent();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Create a derived import run id column - this will be tagged onto our source staging data so we can identify it in the 
            /// destination database
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            CreateDerivedImportRunIdColumn();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Make the ole db source talk to the derived column
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _oleDbSourceSqlCommandFromVarDataFlowComponent.ConnectComponentTo(_derivedImportRunIdColumn);
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Create a ole db destination data flow component - defines where our staging data is going to live when we import it
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            CreateOleDbDestinationDataFlowComponent();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Make the derived import run id column talk to the destination data source
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _derivedImportRunIdColumn.ConnectComponentTo(_oleDbDestinationDataFlowComponent);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// VERY IMPORTANT BIT!
            /// This is where all the outputs exposed by the derivedImportRunIdColumn are mapped to the ole db destination
            /// If anything is going to fail it's going to be this - if the staging destination schema is wrong then this will blow up
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            _oleDbDestinationDataFlowComponent.MapComponentTo(_oleDbSourceSqlCommandFromVarDataFlowComponent, _derivedImportRunIdColumn);
        }

        private void CreateOleDbDestinationDataFlowComponent()
        {
            _oleDbDestinationDataFlowComponent = new OleDbDestinationFastLoadDataFlowComponent(Element.Name, _dataFlowTaskComponent.Executable, new SsisConnectionManager(Parent.DestinationConnection), Parent.ImportSchemaName, true, false, true, true);
            _oleDbDestinationDataFlowComponent.ConstructComponent();

            if (!_oleDbDestinationDataFlowComponent.IsValid)
            {
                throw new SsisScripterException("Just tried to create a OleDbDestinationFastLoadDataFlowComponent for this import but it is not valid");
            }
        }

        private void CreateDerivedImportRunIdColumn()
        {
            _derivedImportRunIdColumn = new DerivedInsertedColumnDataFlowComponent("ImportRunId", _dataFlowTaskComponent.Executable, Parent.ImportRunIdVar);
            _derivedImportRunIdColumn.ConstructComponent();
            
            if (!_derivedImportRunIdColumn.IsValid)
            {
                throw new SsisScripterException("Just tried to create a DerivedInsertedColumnDataFlowComponent for this import but it is not valid");
            }
        }

        private void CreateOleDbSourceSqlCommandFromVarDataFlowComponent()
        {
            //add a variable to the task host that holds the SQL select statement we are going to use to grab our data
            var sqlStatementExpression = Element.JoinClause.Replace(SsisConstants.DynamicSqlToken, "\" + " + Parent.SourceIdsVar.QualifiedName.GetSsisParameterForExpression() + " + \"" );
            sqlStatementExpression = "\"" + sqlStatementExpression + "\"";

            var sqlStatementVar = Parent.Package.AddVariable(Element.Name + "SqlStatement", sqlStatementExpression, false, true);

            _oleDbSourceSqlCommandFromVarDataFlowComponent = new OleDbSourceSqlCommandFromVarDataFlowComponent(Element.Name, _dataFlowTaskComponent.Executable, new SsisConnectionManager(Parent.SourceConnection), sqlStatementVar);
            _oleDbSourceSqlCommandFromVarDataFlowComponent.ConstructComponent();

            if (!_oleDbSourceSqlCommandFromVarDataFlowComponent.IsValid)
            {
                throw new SsisScripterException("Just tried to create a CreateOleDbSourceSqlCommandDataFlowComponent for this import but it is not valid");
            }
        }

        private void CreateOleDbSourceSqlCommandDataFlowComponent()
        {
            var oleDbSourceDataFlowComponent = new OleDbSourceSqlCommandDataFlowComponent(Element.Name, _dataFlowTaskComponent.Executable, new SsisConnectionManager(Parent.SourceConnection), Element.JoinClause, Parent.SourceIdsVar);
            oleDbSourceDataFlowComponent.ConstructComponent();

            if (!oleDbSourceDataFlowComponent.IsValid)
            {
                throw new SsisScripterException("Just tried to create a CreateOleDbSourceSqlCommandDataFlowComponent for this import but it is not valid");
            }
        }

        private void CreateDataFlowTaskComponent()
        {
            _dataFlowTaskComponent = new DataFlowTaskComponent(Element.Name, Parent.Package, Parent.Application);
            _dataFlowTaskComponent.ConstructComponent();
            
            if(!_dataFlowTaskComponent.IsValid)
            {
                throw new SsisScripterException("Just tried to create a DataFlowTaskComponent for this import but it is not valid");
            }

            if(Parent.PreviousTaskComponent != null)
            {
                Parent.TaskConnections.Add(_dataFlowTaskComponent, Parent.PreviousTaskComponent);
            }

            Parent.PreviousTaskComponent = _dataFlowTaskComponent;
        }
    }
}