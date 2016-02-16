// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Interface for an Ssis component
// 
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace RenRe.Doris.Data.Ssis.Scaffolding
{
    public interface ISsisComponent
    {
        /// <summary>
        /// The name of the component
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Is this valid? Has it been constructed and can it be added to an SSIS package?
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// The input to this component
        /// </summary>
        IDTSInput100 Input { get; }

        /// <summary>
        /// The output from this component
        /// </summary>
        IDTSOutput100 Output { get;  }

        /// <summary>
        /// Can this component be connected to?
        /// </summary>
        bool CanBeConnectedTo { get; }

        /// <summary>
        /// Can this component be connected from?
        /// </summary>
        bool CanBeConnectedFrom { get; }

        /// <summary>
        /// Do everything required to make this component "live" and ready for insertion into an SSIS package
        /// </summary>
        void ConstructComponent();
    }
}
