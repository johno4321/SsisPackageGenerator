// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.DataFlows
{
    public abstract class DataFlowComponent : SsisComponent
    {
        private readonly IDTSObjectHost _container;
        
        public IDTSComponentMetaData100 Component { get; protected set; }
        public IDTSDesigntimeComponent100 Instance { get; protected set; }

        protected MainPipe DataFlowMainPipe
        {
            get
            {
                return _container.InnerObject as MainPipe;
            }
        }

        protected DataFlowComponent(string name, IDTSObjectHost container)
            : base(name)
        {
            _container = container;
        }

        public override void ConstructComponent()
        {
            Component = DataFlowMainPipe.AddComponentToDataFlow(Name, ComponentType);
            
        }

        public virtual void ConnectComponentTo(SsisComponent component)
        {
            if (!CanBeConnectedFrom)
            {
                throw new SsisScripterException("The component " + Name + " cannot be used as an output for an SSIS connection");
            }

            if (!component.CanBeConnectedTo)
            {
                throw new SsisScripterException("The component " + Name + " cannot be used as an input for an SSIS connection");
            }

            var path = DataFlowMainPipe.PathCollection.New();
            path.AttachPathAndPropagateNotifications(Output, component.Input);

            Instance.RefreshMetaData();
        }
    }
}