using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Soway.DB
{
     internal  class SqlTransAutoMic
    {
         public IDbCommand  command { get; set; }
         public Object ob { get; set; }
         public SqlOperation Operation { get; set; }
         

    }
}
