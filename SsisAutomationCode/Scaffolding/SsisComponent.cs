// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Basic abstract implementation of the ISsisComponent interface
// 
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace RenRe.Doris.Data.Ssis.Scaffolding
{
    public abstract class SsisComponent : ISsisComponent
    {
        protected string ComponentType;

        public string Name { get; protected set; }

        public bool IsValid { get; protected set; }

        public IDTSInput100 Input { get; protected set; }

        public IDTSOutput100 Output { get; protected set; }

        public bool CanBeConnectedTo { get; protected set; }

        public bool CanBeConnectedFrom { get; protected set; }

        protected SsisComponent(string name)
        {
            Name = name;
        }

        public abstract void ConstructComponent();

        //public abstract void ConnectComponentTo(SsisComponent component);
    }
}