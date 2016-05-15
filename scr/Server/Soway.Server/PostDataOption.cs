using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service
{
    [DataContract]
    public   class PostDataOption
    {
        [DataMember]
        public string Token { get; set; }

    }
}
