// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
using System.Collections.Generic;

namespace RenRe.Doris.Data.Ssis
{
    /// <summary>
    /// Represents the definition of the source data to load into the staging area.
    /// It contains a collection of import elements.
    /// </summary>
    public class ImportSchema
    {
        private readonly ICollection<ImportElement> _elements = new List<ImportElement>();
        
        public ImportSchema(): this("")
        {
        }

        public ImportSchema(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        
        public ICollection<ImportElement> Elements { get { return _elements;  } }

        public void AddTable(string tableName, string whereClause, string joinClause)
        {
            _elements.Add(new ImportElement(tableName, whereClause, joinClause));
        }
    }
}