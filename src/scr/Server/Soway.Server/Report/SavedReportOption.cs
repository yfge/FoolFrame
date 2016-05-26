using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    public class SavedReportOption :PostDataOption
    {
        public int ViewId { get; set; }
        public List<ReportCol> ReportCols { get; set; }
        public BoolExp FilterExp { get; set; }
        public String ReportName { get; set; }
    }
}
