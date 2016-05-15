using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Soway.Model;
using Soway.Model.View;

namespace Soway.Service
{
    public class ViewData
    {
        public long ID { get; set; }
        public string Name { get; set; }
        //public string Filter;
        public  ViewType Type { get; set; }

        //public long  Modelid;
        public List<ViewItem> Items { get; set; }
        public List<ViewOperation> Operations { get; set; }

        public long DetailViewId { get; set; }

        public string TempFile { get;  set; }
        public Model.ViewType ShowType { get;  set; }
        public int AutoFreshTime { get;   set; }
    }
}