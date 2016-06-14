using Soway.Service.bean;
using Soway.Service.Login.V1;
using Soway.Service.ThriftClient;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Login.V2
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2015.10.20
    /// 已经更新</remarks>
    class HandlerLogin : Handler
    {
        private ResultLogin Data;
        private Soway.Service.Login.LoginOperation Option;
        protected override void ImplementBusinessLogic()
        {
            HandlerCheckCode checkcodeHandler = new HandlerCheckCode();
            bool isCodeValid= checkcodeHandler.Check(new bean.CheckCode() { Key= Option.CheckCodeKey, Code = Option.CheckCode });
            if(!isCodeValid)
            {
                Data.Error = new ErrorInfo(ErrorDescription.CHECK_CODE_ERROR,
                         ErrorDescription.CHECK_CODE_ERROR_MSG,true);
                return;
            }
            var cxt = new Soway.Model.SqlServer.DynamicContext
         (new ConHelper().GetSysCon().ToString(), this);
            dynamic app = cxt.GetById(typeof(Soway.Model.App.Application), this.Option.AppId);
            if (app != null)
            {
                dynamic datacurrentCon = null;
                foreach (dynamic con in app.DataBase)
                {
                    if(con.StoreBaseId.ToString().Trim().ToLower() == Option.DbId.Trim()){
                        datacurrentCon = con;
                    }
                }
             
                if (datacurrentCon == null)
                {
                    Data.Error = new ErrorInfo(ErrorDescription.DB_SELECT_ERROR, ErrorDescription.DB_SELECT_ERROR_MSG,true);
                    return;
                }


                dynamic loginUser = cxt.GetById(typeof(SOWAY.ORM.AUTH.User), this.Option.UserId);

                var user = new global::SOWAY.ORM.AUTH.LoginFactory(
                    new bean.ConHelper().GetSysCon(),this).Login(Option.UserId, Option.PassWord);
                if (user != null)
                {
                    var authuser = new SOWAY.ORM.AUTH.AuthoriezedFactory(app,this).GetAuthrizedUser(user);
                    CacheInfo info = new CacheInfo();
                    info.AppSqlCon = app.SysCon;
                    info.CurrentDataBaseName = datacurrentCon.Name;
                    info.CurrentSqlCon = datacurrentCon.Conection;
                    info.App = new AppInfo()
                    {
                        AppLogoUrl = app.Avatar,
                        AppName = app.Name,
                        AppNote = app.Note,
                        AppPowerBy = app.Company,
                        AppPowerUrl = app.Url,
                        AppVer = app.Version,
                        AppId = app.APPID.ToString(),
                        DefaultViewId = app.DefaultView
                    };
                    info.User = new User.UserInfo()
                    {
                        UserId = user.UserID,
                        UserName = user.FirstName,
                        LoginName = user.LoginName,
                        UserAvtarUrl = user.Avtar,
                        CompanyName = "",
                        DepartmentName = ""


                    };
                    


                    try
                    {
                        ITokenAoStub stub = new LocaTokenAoStub();
                        
                        Data.Token = stub.getToken(3000, new TokenKeyGenerator().GetTokenKey(info.User,info.App));
                        Data.LoginSucess = true;
                        
               
                    }
                    catch (Exception e)
                    {
                        //SowayLog.Log.Error("get token fail.", e);
                        Data.Error = new ErrorInfo(ErrorDescription.CODE_SYSTEM_ERROR, ErrorDescription.MESSAGE_SYSTEM_ERROR,true);
                    }
                   
                    try
                    {
                        new CacheStore().Store(Data.Token, info);
                    }
                    catch (Exception sessionex)
                    {
                        //SowayLog.Log.Error("store session fail.", sessionex);
                        Data.Error = new ErrorInfo(ErrorDescription.SET_SESSION_FAIL,
                            ErrorDescription.SET_SESSION_FAIL_MSG,true);
                        return;
                    }
                }
                else
                {
                    Data.Error = new ErrorInfo(ErrorDescription.CODE_AUTHENTICATE_FAIL,
                        ErrorDescription.MESSAGE_AUTHENTICATE_FAIL,true);
                }
            }
     
        }
             public HandlerLogin(Soway.Service.Login.LoginOperation option)
        {



            Option = option;
            PostData = option;
            Data = new ResultLogin();
            Result = Data;
            IsNeedAuthenticate = false;

        }
 
    }
}
