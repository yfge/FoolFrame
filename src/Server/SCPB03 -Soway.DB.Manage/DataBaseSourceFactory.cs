using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.DB.Manage
{
    public class DataBaseSourceFactory 
    {


        private WorkingDataBase Db { get; set; }

        
 

      
        public string GetConStr(String Key)
        {
            var dbs = new Manage.WorkDataBaseFactory().ALLList();
            DBContext db = new DBContext(this.Db.ConnectionString);
            var con = db.GetDetail<DataBaseSource>((object)Key);
            if (con != null)
            {
                var wdb = dbs.FirstOrDefault(p => p.Code == con.DbNo);
                if (wdb != null)
                    return wdb.ConnectionString;
            }
            return null;
        }

        public DataBaseSourceFactory(String DBCode)
        {

            this.Db = new WorkDataBaseFactory().ALLList().FirstOrDefault(p => p.Code == DBCode);
        }
    }
}
