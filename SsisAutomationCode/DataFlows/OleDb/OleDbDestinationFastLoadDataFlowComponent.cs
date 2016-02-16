// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows.OleDb
{
    public class OleDbDestinationFastLoadDataFlowComponent : OleDbDestinationDataFlowComponent
    {
        private const string OpenRowset = "OpenRowset";
        private const string CommandTimeout = "CommandTimeout";
        private const string FastLoadKeepNulls = "FastLoadKeepNulls";
        private const string FastLoadKeepIdentity = "FastLoadKeepIdentity";
        private const string FastLoadOptions = "FastLoadOptions";
        
        //fast load option values
        private const string Constraints = "CHECK_CONSTRAINTS";
        private const string Tablock = "TABLOCK";
        
        #region Properties this type of destination
        private readonly bool _checkConstraints;
        private readonly bool _tableLock;
        private readonly bool _keepNulls;
        private readonly bool _keepIdentity;
        #endregion

        public OleDbDestinationFastLoadDataFlowComponent(string name, IDTSObjectHost container, ISsisConnectionManager connectionManager, string importSchemaName, bool checkConstraints, bool tableLock, bool keepNulls, bool keepIdentity)
            : base(name, container, connectionManager, importSchemaName)
        {
            _checkConstraints = checkConstraints;
            _tableLock = tableLock;
            _keepNulls = keepNulls;
            _keepIdentity = keepIdentity;
        }

        public override void ConstructComponent()
        {
            base.ConstructComponent();

            Instance.SetComponentProperty(AccessMode, OleDbDestinationAccessMode.OpenRowsetUsingFastload); //we want to specify a table name for where our staging data gets dumped
            Instance.SetComponentProperty(OpenRowset, ImportSchemaName + "." + TableName);
            Instance.SetComponentProperty(CommandTimeout, 0); //TODO think about this. let it take as long as it wants to get it's data 

            Instance.SetComponentProperty(FastLoadKeepNulls, _keepNulls);
            Instance.SetComponentProperty(FastLoadKeepIdentity, _keepIdentity);
            Instance.SetComponentProperty(FastLoadOptions, GetFastLoadOptions());

            Instance.RefreshMetaData();

            Output = null;
            CanBeConnectedFrom = false;
            
            Input = Component.InputCollection[0];
            CanBeConnectedTo = true;

            IsValid = true;
        }

        private string GetFastLoadOptions()
        {
            var fastLoadOptions = string.Empty;

            if (_checkConstraints)
            {
                fastLoadOptions = Constraints;
            }

            if (_tableLock)
            {
                fastLoadOptions += ";" + Tablock;
            }

            return fastLoadOptions;
        }

        /// <summary>
        /// This is a bit messy.
        /// For some reason the input columns are not been generated for the OLE DB destination
        /// (I suspect it's the way I'm constructing & attaching the data flow components)
        /// So what I'm doing here is passing all the components in the data flow to this method and
        /// 1) iterating over the external meta data columns in this destination (the columns in the destination database)
        /// 2) iterating over the components passed to this method
        /// 3) iterating over the outputs from each component
        /// 4) trying to match a destination meta data column to a components output column
        /// 5) if they match generate an input column for the destination using the output column's LineageID and the meta data columns id
        /// 
        /// Obviously this sucks but until I work out what I'm doing wrong (if I'm doing anything wrong at all) this will work
        /// as our input schema column names match our output schema columns
        /// 
        /// To work out whats wrong with the ConstructComponent method I am using I will need to "script" out the type of data flows I am
        /// creating for staging imports and based on that work out what to replace this with.
        /// </summary>
        /// <param name="components"></param>
        public void MapComponentTo(params DataFlowComponent[] components)
        {
            foreach (IDTSExternalMetadataColumn100 metaDataCol in Input.ExternalMetadataColumnCollection)
            {
                foreach(var component in components)
                {
                    foreach (IDTSOutputColumn100 outputColumn in component.Output.OutputColumnCollection)
                    {
                        if (outputColumn.Name != metaDataCol.Name) continue;
                        
                        var inputCol = Input.InputColumnCollection.New();
                        inputCol.ExternalMetadataColumnID = metaDataCol.ID;
                        inputCol.LineageID = outputColumn.LineageID;
                        inputCol.UsageType = DTSUsageType.UT_READONLY;
                    }
                }
            }
        }
    }
}