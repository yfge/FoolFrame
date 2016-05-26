using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service
{
    [DataContract]
    public abstract class ResultSession:Result
    {
        
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public int SesstionTimeOut { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public bool IsLogin { get; set; }
        [DataMember]
        public string UserShowName { get; set; }
        [DataMember]
        public string UserAvtar { get; set; }
    }
} 