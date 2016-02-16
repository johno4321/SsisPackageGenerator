// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
// 
namespace RenRe.Doris.Data.Ssis
{
    public class ImportElement
    {
        private readonly string _name;
        private readonly string _whereClause;
        private readonly string _joinClause;

        public ImportElement(string name, string whereClause, string joinClause)
        {
            _name = name;
            _whereClause = whereClause;
            _joinClause = joinClause;
        }

        public string Name
        {
            get { return _name; }
        }

        public string WhereClause
        {
            get { return _whereClause; }
        }

        public string JoinClause
        {
            get { return _joinClause; }
        }
    }
}