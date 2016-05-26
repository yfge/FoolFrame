using Soway.Service.bean;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class ResultInitApp : Result
    {

        public ResultInitApp() : base() { }

        public String AppTitle { get; set; }
        public string AppName { get; set; }

        public string AppImg { get; set; }

        public string AppVersion { get; set; }

        public string AppPowerBy { get; set; }

        public string AppUrl { get; set; }

        public CheckCode CheckCode { get; set; }

        public List<Init.StoreBaseInfo> Dbs{get;set;}
    }
}
