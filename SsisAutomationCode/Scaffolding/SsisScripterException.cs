// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Exception used to wrap errors and exceptiosn detcted during creation of SSIS packages
// 
using System;
using System.Runtime.Serialization;

namespace RenRe.Doris.Data.Ssis.Scaffolding
{
    [Serializable]
    public class SsisScripterException : Exception
    {
        public SsisScripterException()
        {
            
        }

        public SsisScripterException(string message) : base(message)
        {
            
        }

        protected SsisScripterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }

        public SsisScripterException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
