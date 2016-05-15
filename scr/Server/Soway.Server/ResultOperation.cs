using Soway.Service.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class ResultOperation :Result
    {
        public List<ObjValuePair> Value;
        public bool IsSuccess { get; set; }
        public string ReturnObjId { get; set; }
        public long ReturnViewId { get; set; }
        public string ReturnMsg { get; set; }
    }
}