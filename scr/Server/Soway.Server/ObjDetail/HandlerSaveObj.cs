using Soway.Model;
using Soway.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ObjDetail
{
    class HandlerSaveObj : Handler
    {

        public SaveObjOption Option { get; set; }



        public HandlerSaveObj(SaveObjOption opt)
        {

            this.Option = opt;
            this.PostData = this.Option;
            this.IsNeedAuthenticate = true;
            this.Result = new Result();

        }

        protected override void ImplementBusinessLogic()
        {

            AutoViewFactory factory = new AutoViewFactory(this.Info.AppSqlCon,this);
            View view = factory.GetView(this.Option.SaveObj.ViewID);
            var sql = GetViewSql(view);


            global::Soway.Model.SqlServer.dbContext context = new global::Soway.Model.SqlServer.dbContext(sql,this);
        
            IObjectProxy iObjectProxy = context.GetDetail(view.Model, this.Option.SaveObj.Id);
            DataFormator.ObjUpdateToProxy(this.Option.SaveObj, iObjectProxy);
            context.Save(iObjectProxy);
           

        }
    }
}
