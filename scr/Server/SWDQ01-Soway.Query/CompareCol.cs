using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Query
{
    public class CompareCol
    {
        public Query.Entity.IQueryColumn Col
        {
            get;
            set;
        }

        public string SelectedTableName
        {
            get;
            set;
        }
    }
}
