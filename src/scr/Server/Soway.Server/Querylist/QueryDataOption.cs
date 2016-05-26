using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.QueryData
{
    public class QueryDataOption :PostDataOption
    {
        public long ViewId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int OrderByItem { get; set; }
        public int OrderByType { get; set; }

        public string QueryFilter { get; set; }
    }
}
