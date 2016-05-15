using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;

namespace Soway.Query
{
    public class QueryParameter
    {
        public CompareCol Column { get; set; }
        public System.Data.SqlClient.SqlParameter SqlParam { get; set; }

        public string Name { get; set; }
        
    }
}
