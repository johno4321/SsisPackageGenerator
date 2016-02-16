// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 

namespace RenRe.Doris.Data.Ssis
{
    public abstract class ScriptItem
    {
        public ImportElement Element { get; private set; }

        protected ScriptItem(ImportElement importElement)
        {
            Element = importElement;
        }

        public abstract void Generate();
    }
}