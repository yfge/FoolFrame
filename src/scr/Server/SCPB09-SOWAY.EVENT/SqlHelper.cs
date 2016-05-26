using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Event
{
    internal static class SqlHelper
    {
        public static System.Data.SqlClient.SqlCommand GetQueryCommand(dynamic model, dynamic def)

        {

            var tableName = model.DataTableName;
            var command = new System.Data.SqlClient.SqlCommand()
            {
                CommandText = String.Format("SELECT * FROM {0} WHERE {1}", model.DataTableName,
                def.Filter)
            };
            return command;
        }
        
            
         
        
    }
}
