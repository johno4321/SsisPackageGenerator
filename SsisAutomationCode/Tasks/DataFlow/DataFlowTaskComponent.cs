// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.Tasks.DataFlow
{
    public class DataFlowTaskComponent : TaskComponent
    {
        public DataFlowTaskComponent(string name, Package container, Application dtsApplication)
            : base(name, container, dtsApplication)
        {
            ComponentType = SsisConstants.SsisComponentPipelineTaskTaskType;
        }

        public override void ConstructComponent()
        {
            var executableInstance = Container.Executables.Add(ComponentType);
            var dataFlowTaskHost = executableInstance as TaskHost;
            if (dataFlowTaskHost == null)
            {
                throw new SsisScripterException("Cannot cast the components executable to a TaskHost object");
            }

            dataFlowTaskHost.Name = Name;

            Executable = executableInstance as IDTSObjectHost;

            IsValid = true;
        }

        public TaskHost TaskHost
        {
            get
            {
                return Executable.InnerObject as TaskHost;
            }
        }
    }
} 