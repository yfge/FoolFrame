using Soway.Model;
using Soway.Model.View;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class HandlerQueryDataDetail : Handler
    {
        

        public ResultDataDetail Data;
        private QueryDataDetailOption Option { get; set; }


        public HandlerQueryDataDetail(QueryDataDetailOption option)
        {
            this.PostData = option;
            this.Option = option;
            init();// option.viewId,option.objId);
        }

        private void init()//long viewId,object objId)
        {
            IsNeedAuthenticate = true;
            
            Data = new ResultDataDetail();
            Data.Token = this.PostData.Token;
            Data.CanEdit = true;
                 
           
            Result = Data;
        }

        protected override void ImplementBusinessLogic()
        {
        
            //AutoViewFactory factory = new AutoViewFactory(this.Info.AppSqlCon,this);
            //View view = factory.GetView(this.Option.viewId);

            var cxt = new Soway.Model.SqlServer.DynamicContext(this.Info.AppSqlCon.ToString(), this);
            var view = cxt.GetById(typeof(View), this.Option.viewId);
            var sql = GetViewSql(view);
            Data.AutoFreshTime = view.AutoFreshInterval;

            var objid = this.Option.objId;
            if(string.IsNullOrEmpty((objid ??"").ToString().Trim()))

            {
                if(string.IsNullOrEmpty((this.Option.IdExp ?? "").Trim())==false)
                {
                    objid = new Soway.Model.Expressions.GetValueExpression(this).GetValue(null, null, this.Option.IdExp);
                }
                else
                {
                   
                    var getidcontext = new Soway.Model.Context.InputContext(sql).Query(view.Model, "", null, "", 10);
                    objid = getidcontext.First().id;

                }
            }
            global::Soway.Model.SqlServer.dbContext context = new global::Soway.Model.SqlServer.dbContext(sql,this);
            IObjectProxy iObjectProxy = context.GetDetail(view.Model, objid);
            Data.CanEdit = view.CanEdit;
            Data.Data =  DataFormator.IObjectProxyToDetail(iObjectProxy, view);
            Data.Operations = new List<ViewOperation>();

           
            foreach(var op in view.Operations)
            {
                Data.Operations.Add(new ViewOperation()
                {
                    Name = op.Name,
                    ViewID =(op.Operation==null?(op.ResultView==null?0: op.ResultView.ID):(op.Operation.ResultView==null ?0:(op.Operation.ResultView.ID))) ,
                    ID =(op.Operation==null ?0: (op.Operation.Operation == null ? 0 : op.Operation.Operation.ID))
                });
            }

        }
    }
}