using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Report
{
    public class ReportCol
    {
        public string ColName { get; set; }

        public ViewItem Item { get; set; }

        public Soway.Query.SelectType SelectType { get; set; }
        public int Index { get; set; }

        public int Width { get; set; }
        public int ViewIndex { get; set; }
    }
}
