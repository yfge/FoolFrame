using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Login.V2
{

    [DataContract]
    public class ResultGetSubAuth : Result
    {
        [DataMember]
        public List<AuthItem> Items { get; set; }
 

        [DataMember]
        public string Token;

        public ResultGetSubAuth()
        {
            Items = new List<AuthItem>();
        }
    }
}
