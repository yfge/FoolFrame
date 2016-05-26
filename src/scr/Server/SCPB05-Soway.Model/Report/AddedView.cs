using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Report
{
    public class AddedView
    {
        public Soway.Model.View.View View { get; set; }
        
        public Soway.Model.Relation Relation { get; set; }
        public int AddIndex { get; set; }
    }
}
