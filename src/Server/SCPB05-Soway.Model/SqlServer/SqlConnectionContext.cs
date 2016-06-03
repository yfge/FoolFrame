using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.SqlServer
{
    class SqlConnectionContext :IDisposable
    {

        public SqlConnectionContext()
        {
            this.trans = new Dictionary<string, System.Data.SqlClient.SqlConnection>();
        }
        public void Dispose()
        {
             
        }

        private System.Data.SqlClient.SqlConnection GetCon(Model model)
        {
            string conStr = "";
            if (model.SqlCon == null)
                conStr = model.Module.SqlCon.ToString();
            else
                conStr = model.SqlCon.ToString();
            if (trans.ContainsKey(conStr) == false)
            {
                var con = new System.Data.SqlClient.SqlConnection(conStr);
                con.Open();
                trans.Add(conStr, con);
            }
            return trans[conStr];



        }
        Dictionary<String, System.Data.SqlClient.SqlConnection> trans;
        public void AttachCommands(Model Model ,System.Data.SqlClient.SqlCommand Comand){
           


        }

        public void Commit() { }
    }
}
