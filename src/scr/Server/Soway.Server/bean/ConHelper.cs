using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.bean
{
    class ConHelper
    {
        public Soway.Model.SqlCon GetCon(String key)
        {


            System.Data.SqlClient.SqlConnectionStringBuilder builder =
              //
           new System.Data.SqlClient.SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[key].ConnectionString);
    

            return new global::Soway.Model.SqlCon()
            {
                DataSource = builder.DataSource,
                InitialCatalog = builder.InitialCatalog,
                UserID = builder.UserID,
                Password = builder.Password,
                IsLocal = false,
                IntegratedSecurity = false
            };
        }
     

        public Soway.Model.SqlCon GetSysCon()
        {
            return GetCon("dbSys");
        }
    }
}
