using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    public class ReportResult : Result

    {
        public int ViewId { get; set; }
   
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public long TotalRecords { get; set; }
        public long TotalPages { get; set; }
        public List<ReportCell> Cells { get; set; }

    }
}
