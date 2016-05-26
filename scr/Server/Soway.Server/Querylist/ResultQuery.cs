using Soway.Model.Context;
using Soway.Service.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class ResultQuery :TokenResult
    {
        public int TotalItem;
        public int TotalPage;
        public int PageIndex;
        public List<QueryKeyValueResult> Data;
        public DateTime FreshTime;
        public List<String> Cols;

        public int AutoFreshTime { get;  set; }
    }

    public class QueryKeyValueResult
    {
        public long RowIndex;
        public object Id;
        public List<ObjValuePair> Items;
        public String RowFmt;
    }
}


