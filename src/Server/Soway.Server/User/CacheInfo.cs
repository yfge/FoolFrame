using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    [Serializable]
    [DataContract]
    public class CacheInfo
    {
        [DataMember]
        public AppInfo App{get;set;}
        [DataMember]
        public UserInfo User { get; set; }
        [DataMember]
        public string CurrentDataBaseName { get; set; }
        [DataMember]
        internal Model.SqlCon CurrentSqlCon { get; set;}
        
        [DataMember]
        internal Model.SqlCon AppSqlCon { get; set; }

         
    }
}
