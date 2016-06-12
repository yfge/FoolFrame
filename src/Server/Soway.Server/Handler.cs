using Soway.Service.Login.V1;
using Soway.Service.ThriftClient;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Soway.Model.Context;

namespace Soway.Service
{
    public abstract class Handler : IHandler, Soway.Model.Context.ICurrentContextFactory
    {
        public bool IsNeedAuthenticate;
        public Result Result;
        public ErrorInfo Errors;
        public bool IsJsonResult;
        public string Token;

        public string TokenKey;
        public PostDataOption PostData { get; set; }

        protected CacheInfo Info { get; set; }
        public void Invoke()
        {
            if (IsNeedAuthenticate)
            {
                try
                {
                    Info  = new CacheStore().Get(this.PostData.Token);
                    if (Info == null)
                    {

                        Errors = new ErrorInfo(ErrorDescription.TOKEN_INVALIDAT, ErrorDescription.TOKEN_INVALIDAT_MSG,true)
                        ;
                        return;

                    }
                }
                catch (Exception e)
                {
                    //SowayLog.Log.Error("check session fail", e);

                    Errors = new ErrorInfo(ErrorDescription.TOKEN_INVALIDAT, ErrorDescription.TOKEN_INVALIDAT_MSG,true);
                }
            }


            if (Errors != null)
            {
                Result.Error = Errors;
                return;
            }

            ImplementBusinessLogic();
        }

        protected abstract void ImplementBusinessLogic();


      
        protected ChkCodeImg GetCheckCode()
        {
            ChkCodeImg chkImg = CheckCodeFac.GetCheckCode();

            try
            {
                ISessionDaoStub stub = new  SessionDaoStubFac().Get();
                stub.updateSession(3000, chkImg.Key, System.Text.UTF8Encoding.UTF8.GetBytes(chkImg.CheckCode), 60);
            }
            catch (Exception e)
            {

               

                Errors = new ErrorInfo(ErrorDescription.CODE_SYSTEM_ERROR, ErrorDescription.MESSAGE_SYSTEM_ERROR,true);
            }

            return chkImg;
        }

        protected Model.SqlCon GetViewSql(Soway.Model.View.View view)
        {
            switch (view.ConnectionType)
            {
                case Model.ConnectionType.AppSys:
                    if(this.Info == null)
                    {
                        return null;
                    }
                    return this.Info.AppSqlCon;
                case Model.ConnectionType.Current:
                    if(this.Info == null)
                    {
                        return null;
                    }
                    return this.Info.CurrentSqlCon;
                case Model.ConnectionType.Default:
                    if(this.Info == null)
                    {
                        return null;
                    }
                    return this.Info.CurrentSqlCon;
                case Model.ConnectionType.ModelSys:
                    if (view.Model.SqlCon != null)
                       return  view.Model.SqlCon;
                    else  if (view.Model.Module.SqlCon != null)
                       return  view.Model.Module.SqlCon;
                    return null;
                case Model.ConnectionType.System:
                    return new bean.ConHelper().GetSysCon();
                default:
                    return null;
                  

            }
        }

        public CurrentContext GetCurrentContext()
        {

            CurrentContext con = new CurrentContext();
            if (this.Info != null)
            {
                con.Address = "";
                con.AppCon = this.Info.AppSqlCon;
                con.CurrentCon = this.Info.CurrentSqlCon;
                con.ModelCon = null;
                con.SysCon = new bean.ConHelper().GetSysCon();
                con.UserId = this.Info.User.UserId.ToString();
                con.UserName = this.Info.User.UserName;
            }

            return con;
        }

        public object GetValue(string key)
        {

            return "";
        }

        public DateTime GetDateTime()
        {

            return DateTime.Now;
        }
    }
}