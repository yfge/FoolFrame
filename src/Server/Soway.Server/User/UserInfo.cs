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
    public class UserInfo
    {
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string DepartmentName { get; set; }
        [DataMember]
        public string UserAvtarUrl { get; set; }
    
    }
}
