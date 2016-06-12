using Soway.Model;
using Soway.Model.View;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soway.Service
{
    public class HandlerRunOperation : Handler
    {
        private long mViewId;
        private long mOperationId;
        private string mObjectId;
        public ResultOperation Data;
        public HandlerRunOperation(OperationOption option)
        {
            init(option);
            IsNeedAuthenticate = true;
        }

        private void init(OperationOption option)
        {
            mObjectId = option.ObjectId;
            IsNeedAuthenticate = true;
            mViewId = option.ViewId;
            mOperationId = option.OperationId;
            Data = new ResultOperation();
            Result = Data;
            this.PostData = option;
        }
        protected override void ImplementBusinessLogic()
        {
           
            AutoViewFactory factory = new AutoViewFactory(Info.AppSqlCon,this);
            View view = factory.GetView(mViewId);

            var sql = this.GetViewSql(view);
            global::Soway.Model.SqlServer.dbContext context = new global::Soway.Model.SqlServer.dbContext(sql, this);
            IObjectProxy iObjectProxy = context.GetDetail(view.Model, this.mObjectId);
            var method = view.Operations.FirstOrDefault(p => p.Operation.Operation.ID == this.mOperationId);
            var methodContext = new Soway.Model.ModelMethodContext(sql,this);
        
            if (method != null)
            {
                try
                {
                    methodContext.ExcuteOperation(iObjectProxy, method.Operation.Operation);
                    Data.IsSuccess = true;
                    Data.ReturnMsg = method.Operation.SuccessMsg;
                
                
                }
                catch (Exception e)
                {

                    Data.Error = new ErrorInfo(ErrorDescription.CODE_RUN_OPERATION_ERROR, ErrorDescription.MESSAGE_RUN_OPERATION_ERROR,false);
                    Data.IsSuccess = false;
                    Data.ReturnMsg = method.Operation.ErrorMsg+e.ToString();

                }



            }
        }
    }
}