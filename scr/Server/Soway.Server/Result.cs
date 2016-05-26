using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service
{
    [DataContract]
    public class Result
    {
        [DataMember]
        public ErrorInfo Error { get; set; }
    }
}