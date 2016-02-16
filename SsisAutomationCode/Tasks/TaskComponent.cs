// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using Microsoft.SqlServer.Dts.Runtime;
using RenRe.Doris.Data.Ssis.Scaffolding;

namespace RenRe.Doris.Data.Ssis.Tasks
{
    public abstract class TaskComponent : SsisComponent
    {
        protected Package Container;
        private readonly Application _dtsApplication;
        public IDTSObjectHost Executable { get; protected set; }

        protected TaskComponent(string name, Package container, Application dtsApplication)
            : base(name)
        {
            Container = container;
            _dtsApplication = dtsApplication;
        }

        public virtual void ConnectTaskComponentTo(TaskComponent targetTaskComponent)
        {
            Container.AddPrecedenceConstraint(targetTaskComponent.Executable as Executable, Executable as Executable);
        }

        public virtual void ConnectTaskComponentFrom(TaskComponent targetTaskComponent)
        {
            Container.AddPrecedenceConstraint(Executable as Executable, targetTaskComponent.Executable as Executable);
        }
    }
}