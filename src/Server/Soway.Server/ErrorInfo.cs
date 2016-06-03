using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service
{
    [DataContract]
    public class ErrorInfo
    {
        public ErrorInfo()
        {

        }
        public ErrorInfo(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        [DataMember]
        public int Code;
        [DataMember]
        public string Message;
    }
}