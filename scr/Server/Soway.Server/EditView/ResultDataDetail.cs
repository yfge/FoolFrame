using Soway.Model;
using Soway.Service.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class ResultDataDetail :TokenResult
    {

        public DataDetail Data { get; set; }

        public int AutoFreshTime { get; set; }

        public bool CanEdit { get; set; }

        public List<ViewOperation> Operations { get; set; }

    }
}