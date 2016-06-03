using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    public class ReportCol
    {
        public String ColName { get; set; }
        public String ColId { get; set; }

        public String SelectedTypeId { get; set; }

        public int Index { get; set; }

        public Query.OrderType OrderType { get; set; }
    }
}


