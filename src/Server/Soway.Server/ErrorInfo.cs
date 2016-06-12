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
        public ErrorInfo(int code, string message,bool requireLogin)
        {
            this.Code = code;
            this.Message = message;
            this.RequireLogin = requireLogin;
        }

        [DataMember]
        public int Code;
        [DataMember]
        public string Message;
        [DataMember]
        public bool RequireLogin { get; set; }
    }
}