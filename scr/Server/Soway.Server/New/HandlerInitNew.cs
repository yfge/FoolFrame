using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soway.Model;
using Soway.Model.View;

namespace Soway.Service.New
{
    class HandlerInitNew : Handler
    {
       

        public ResultDataDetail Data;
        private InitNewOption Option { get; set; }

        public HandlerInitNew(InitNewOption option)
        {
            this.PostData = option;
            this.Option = option;
            this.init();
        
        }

        private void init()
        {
            IsNeedAuthenticate = true;
          
            Data = new ResultDataDetail();
            Data.Token = this.PostData.Token;
            Data.CanEdit = true;

        
        }

        protected override void ImplementBusinessLogic()
        {

            AutoViewFactory factory = new AutoViewFactory(this.Info.AppSqlCon,this);
            View view = factory.GetView(this.Option.ViewId);
            var sql = GetViewSql(view);
            global::Soway.Model.SqlServer.dbContext context = new global::Soway.Model.SqlServer.dbContext(sql,this);
            IObjectProxy iObjectProxy = new Soway.Model.ObjectProxy(view.Model,this);
           
            Data.Data = DataFormator.IObjectProxyToDetail(iObjectProxy, view);
            if (String.IsNullOrEmpty((this.Option.ParentObjId ?? "").ToString()) == false)
            {
                Data.Data.ParentId = this.Option.ParentObjId;

            }

            this.Result = Data;
        }
    }
}
