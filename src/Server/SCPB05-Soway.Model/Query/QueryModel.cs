using Soway.Query.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Query
{
    public class QueryModel
    {
        public IQueryTable Table { get; set; }
        public List<ModelQueryCol> Columns { get; set; }

    }
}
