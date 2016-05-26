using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service
{
    public class ViewOption :PostDataOption
    {
        [DataMember]
        public int ViewId;
    }
}