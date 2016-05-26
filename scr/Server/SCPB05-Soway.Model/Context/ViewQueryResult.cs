using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Context
{
    public class QueryResult
    {
        public int CurrentPageIndex{ get; set; }
        public int TotalPagesCount { get; set; }

        public int TotalItemsCount { get; set; }

        public List<ViewResultItem> CurrentResult { get; set; }


   
    }
}
