// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using System.Collections.Generic;
using System.Linq;

namespace RenRe.Doris.Data.Ssis
{
    /// <summary>
    /// A scripter generates code given an <c>ImportSchema</c> instance
    /// using <c>ScriptItem</c> instances.
    /// It knows where and how to store the generated code.
    /// </summary>
    public abstract class Scripter
    {
        private readonly IList<ScriptItem> _items = new List<ScriptItem>();

        protected Scripter(ImportSchema importSchema)
        {
            ImportSchema = importSchema;
        }

        public bool Generated { get; protected set; }

        public ImportSchema ImportSchema { get; protected set; }

        public string Name { get; protected set; }

        public bool HasElement(string elementName)
        {
            return _items.FirstOrDefault(s => s.Element.Name == elementName) != null;
        }

        protected abstract ScriptItem CreateScriptItem(ImportElement element);

        public virtual void Generate()
        {
            Init();
            foreach (var element in ImportSchema.Elements)
            {
                var s = CreateScriptItem(element);
                _items.Add(s);
                s.Generate();
            }
            Finish();
            Generated = true;
        }

        protected abstract void Init();

        protected abstract void Finish();
    }
}