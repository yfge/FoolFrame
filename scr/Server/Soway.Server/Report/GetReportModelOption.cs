using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{

    public class GetReportModelOption :PostDataOption
    {
        public long ViewId { get; set; }

    }
}
