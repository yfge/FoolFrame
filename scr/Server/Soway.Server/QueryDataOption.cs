using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service
{
    public class QueryDataOption : PostDataOption
    {
        [DataMember]
        public long viewId;
        [DataMember]
        public string filter;

    }
}