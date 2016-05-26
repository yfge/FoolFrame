using Soway.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service.Login
{
    [DataContract]
    public class LoginOperation: PostDataOption
    {
      
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string PassWord { get; set; }
        [DataMember]
        public string DbId { get; set;}
        [DataMember]
        public string CheckCode { get; set; }
        [DataMember]
        public string AppId;
        [DataMember]
        public string AppKey;
        [DataMember]
        public string CheckCodeKey;
    }
}