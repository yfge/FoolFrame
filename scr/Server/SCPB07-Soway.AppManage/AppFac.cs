using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.App
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2016.4.16
    /// Added the method getApps to getAll Application </remarks>
    public class AppFac
    {



        private SqlCon SqlCon { get; set; }
        private Context.ICurrentContextFactory ConFac { get; set; }
        public AppFac(SqlCon sqlCon,Context.ICurrentContextFactory conFac)
        {
            this.SqlCon = sqlCon;
            this.ConFac = ConFac;

          
        }
        public Application GetApp(String Appid, String AppPwd)
        {

            var fac = new SqlServer.SqlContext<Soway.Model.App.Application>(this.SqlCon,this.ConFac);

            var app =  fac.GetDetail(Appid);

            if (app != null &&app.AppKey == AppPwd)
                return app;
            else
                return null;
        }

        public List<Application> GetApps()
        {


            var db = new Soway.DB.DBContext(this.SqlCon.ToString());
            return db.Get<Application>();
        }

        public Application GetApp(String Appid)
        {
                  var fac = new SqlServer.SqlContext<Soway.Model.App.Application>(this.SqlCon,this.ConFac);

            return fac.GetDetail(Appid);
        }


     

}
}
