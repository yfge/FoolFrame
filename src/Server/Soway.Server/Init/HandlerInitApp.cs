using Soway.Service.bean;
using Soway.Service.Handlers1._0;
using Soway.Service.Login.V1;
using Soway.Service.ThriftClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2015.10.21
    /// 已经换到了新的库上 这部分OK
    /// </remarks>
    public class HandlerInitApp : Handler
    {


        public ResultInitApp Data;
        private string AppId;
        private string AppKey;

        public HandlerInitApp(string appId, string appKey)
        {
            IsNeedAuthenticate = false;
            init(appId, appKey);

        }

        private void init(string appId, string appKey)
        {

            AppId = appId;
            AppKey = appKey;
            Data = new ResultInitApp();
            Result = Data;
        }
        protected override void ImplementBusinessLogic()
        {
            try {
                global::Soway.Model.App.AppFac fac = new global::Soway.Model.App.AppFac(new ConHelper().GetSysCon(),this);
                var cxt = new Soway.Model.SqlServer.DynamicContext
                    (new ConHelper().GetSysCon().ToString(), this);
                var app = cxt.GetById(typeof(Soway.Model.App.Application), this.AppId);
                 
                //var app = fac.GetApp(this.AppId, this.AppKey);

                if (app == null || app.AppKey != this.AppKey)
                {
                    this.Data.Error = new ErrorInfo(ErrorDescription.CODE_AUTHENTICATE_APPUNAUTH, ErrorDescription.MESSAGE_AUTHENTICATE_PUUUNAUTH,true);

                }
                else
                {
                    this.Data.AppImg = app.InitImage;
                    this.Data.AppName = app.Name;
                    this.Data.AppPowerBy = app.Company;
                    this.Data.AppTitle = app.Name;
                    this.Data.AppUrl = app.Url;
                    this.Data.AppVersion = app.Version;
                    Handler hander = new HandlerCheckCode();
                    hander.Invoke();
                    var chk = (CheckCode)hander.Result;

                    this.Data.CheckCode = chk;
                    this.Data.Dbs = new List<Init.StoreBaseInfo>();

                    foreach (dynamic appdb in app.DataBase)
                    {
                        this.Data.Dbs.Add(new Init.StoreBaseInfo()
                        {
                            DbId = appdb.StoreBaseId,
                            DbName = appdb.Name

                        });
                    }


                }
            }catch (Exception ex)
            {

            }
        }
    }
}