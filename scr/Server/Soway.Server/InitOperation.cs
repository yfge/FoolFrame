using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service
{
    [DataContract]
    public class InitOption :PostDataOption
    {
        [DataMember]
        public string AppId { get; set; }
        [DataMember]
        public string AppKey { get; set; }
    }
}