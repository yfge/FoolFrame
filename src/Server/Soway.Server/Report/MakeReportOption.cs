using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    public class MakeReportOption:PostDataOption
    {
        public int ViewId { get; set; }
        public List<ReportCol> ReportCols { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public BoolExp FilterExp { get; set; }

        

    
    }
}
