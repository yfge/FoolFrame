using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Soway.DB.MSSQL
{
    class SqlCommandPaterns
    {
        public string SqlScript { get; set; }
        public List<SqlParameter> Params { get; set; }
        public SqlCommandPaterns()
        {
            Params = new List<SqlParameter>();
        }
    }
}
