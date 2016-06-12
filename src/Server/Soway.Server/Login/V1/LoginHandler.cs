using Soway.Service.ThriftClient;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service.Login.V1
{
    public class LoginHandler : Handler
    {
       
        private  ResultLogin Data;
        private Soway.Service.Login.LoginOperation Option;

    
        public LoginHandler(Soway.Service.Login.LoginOperation option)
        {
            Option = option;
            PostData = option;
            Data = new ResultLogin();
            IsNeedAuthenticate = false;
        }

        protected override void ImplementBusinessLogic()
        {
            global::Soway.Model.App.AppFac fac = new global::Soway.Model.App.AppFac(new bean.ConHelper().GetSysCon(),this);
            var app = fac.GetApp(Option.AppId, Option.AppKey);
            if (app != null)
            {

                var user = new SOWAY.ORM.AUTH.LoginFactory(new bean.ConHelper().GetSysCon(),this).Login(Option.UserId, Option.PassWord);
                if (user == null)
                {
                    Data.Error = new ErrorInfo(ErrorDescription.CODE_AUTHENTICATE_FAIL,
                     ErrorDescription.MESSAGE_AUTHENTICATE_FAIL,true);

                }
                else
                {
                    string mSystemName = app.Name;
                    //var user = new global::Soway.UM.UserLoginFactory("12").Loigin(Option.UserId, Option.PassWord, mSystemName);
                    //if (user != null)
                    //{
                    //    CacheInfo info = new CacheInfo();
                    //    info.App = new AppInfo()
                    //    {
                    //        AppLogoUrl = app.Avatar,
                    //        AppName = app.Name,
                    //        AppNote = app.Note,
                    //        AppPowerBy = app.Company,
                    //        AppPowerUrl = app.Url,
                    //        AppVer = app.Version,
                    //        AppId = app.APPID.ToString()
                    //    };
                    //    info.User = new User.UserInfo()
                    //    {
                    //        UserId = user.UID,
                    //        UserName = user.Name,
                    //        LoginName = user.ID,
                    //        UserAvtarUrl = user.Str5,
                    //        CompanyName = "",
                    //        DepartmentName = "",


                    //    };

                    //    try
                    //    {
                    //        TokenAoStub stub = new TokenAoStub();
                    //        Data.Token = stub.getToken(3000, new TokenKeyGenerator().GetTokenKey(info.User, info.App));
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        SowayLog.Log.Error("get token fail.", e);

                    //        Data.Error = new ErrorInfo(ErrorDescription.CODE_SYSTEM_ERROR, ErrorDescription.MESSAGE_SYSTEM_ERROR);
                    //    }



                    //    try
                    //    {
                    //        new CacheStore().Store(Token, info);
                    //    }
                    //    catch (Exception sessionex)
                    //    {
                    //        SowayLog.Log.Error("store session fail.", sessionex);
                    //        Data.Error = new ErrorInfo(ErrorDescription.SET_SESSION_FAIL,
                    //            ErrorDescription.SET_SESSION_FAIL_MSG);
                    //    }

                    Data.LoginSucess = false;
                    //}
                    //else
                    //{

                    //    Data.Error = new ErrorInfo(ErrorDescription.CODE_AUTHENTICATE_FAIL,
                    //        ErrorDescription.MESSAGE_AUTHENTICATE_FAIL);
                    //}
                }
            }
            Result = Data;
        }

        //private  AuthItem GetLoginAuth(
        //    global::Soway.Data.DS.Tree.TreeNode<global::Soway.UM.Auth> auth) {

        //    var item = new  AuthItem()
        //    {
        //        Text = auth.Data.Text,
        //        AuthType = ( AuthType)auth.Data.AuthType,
        //        ImageUrl = auth.Data.Str5,
        //        Note = auth.Data.Text,
        //        ViewId = auth.Data.I2 != null ? auth.Data.I2.id : 0,
        //        ViewType = (ViewType)auth.Data.I3,
        //        Index = auth.Data.DisplayIndex,
        //        SubItems = new List<AuthItem>()
        //    };
        //    if(auth.Children.Count > 0)
        //    {
        //        foreach(var subAuth in auth.Children)
        //        {
        //            item.SubItems.Add(GetLoginAuth(subAuth));
        //        }
        //    }
        //    return item;

        //}
    }
}