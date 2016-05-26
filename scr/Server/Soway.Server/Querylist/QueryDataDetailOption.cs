using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service
{

    [DataContract]
    public class QueryDataDetailOption :PostDataOption
    {

        [DataMember]
        public  long viewId { get; set; }
        [DataMember]
        public object objId
        {
            get; set;
        }

        [DataMember]
        public string IdExp
        {
            get; set;
        }

    }
}