using Soway.Model;
using Soway.Model.View;
using Soway.Service.ObjDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.New
{
    class HandlerSaveNew :Handler
    {

        public NewObjOption Option { get; set; }



        public HandlerSaveNew(NewObjOption opt)
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
            IObjectProxy iObjectProxy =    new Soway.Model.ObjectProxy(view.Model,this);
            if (String.IsNullOrEmpty(this.Option.OwnerViewId) == false)
            {
                var ownerModel = factory.GetView(this.Option.OwnerViewId).Model;
                var owner = context.GetDetail(ownerModel, this.Option.OwnerId);
                Soway.Model.ModelBindingList array = owner[this.Option.Property] as Soway.Model.ModelBindingList;
                 iObjectProxy = array.AddNew();
                DataFormator.ObjUpdateToProxy(this.Option.SaveObj, iObjectProxy);
                context.Save(owner);
                //iObjectProxy.Owner = new ObjectProxy(ownerModel) { ID = this.Option.OwnerId ,SaveInDB=true,IsLoad= LoadType.Complete};
            }
            else
            {
                DataFormator.ObjUpdateToProxy(this.Option.SaveObj, iObjectProxy);
                context.Create(iObjectProxy);
            }



        }
    }
}
