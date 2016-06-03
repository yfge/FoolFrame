using Soway.Service.Login.V1;
using Soway.Service.Login.V2;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service.Session
{
    
    public class GetMainInfoHandler : Handler
    {
        private ResultGetSubAuth Data;

        protected override void ImplementBusinessLogic()
        {
            var info = new CacheStore().Get(this.PostData.Token);
            if (info != null && info.User != null)
            {


                var user = new Soway.Model.SqlServer.ObjectContext<SOWAY.ORM.AUTH.User>(new bean.ConHelper().GetSysCon(),this).GetDetail(info.User.UserId);
                if (user != null)
                {
                    var authuser = new SOWAY.ORM.AUTH.AuthoriezedFactory(new Model.App.Application() { SysCon = info.AppSqlCon },this).GetAuthrizedUser(user);
                    List<SOWAY.ORM.AUTH.MenuItem> auths = new List<SOWAY.ORM.AUTH.MenuItem>();
                  
                        auths = new SOWAY.ORM.AUTH.MenuItemFactory(info.CurrentSqlCon,this).GetTopMenus(authuser);
                    foreach (var menu in auths)
                    {
                        var item = new Login.V2.AuthItem()
                        {
                            Text = menu.Text,
                            AuthType = AuthType.ListView,
                            ImageUrl = menu.Image,
                            Note = menu.Text,
                            ViewId = menu.ViewID,
                            ViewType =Login.V1.ViewType.List,
                            Index = 0,

                            AuthNo =menu.ID.ToString()
                        };
                        Data.Items.Add(item);
                    }
                    //Data.UserID = user.ID;
                    //Data.UserShowName = user.Name;
                    //Data.UserAvtar = user.Str5;
                    //Data.IsLogin = true;
                    

                    info.User.UserId = user.UserID;
                    info.User.UserName = user.FirstName+user.LastName;
                    info.User.UserAvtarUrl = user.Avtar;
                   
                }
                else
                {
                    //Data.IsLogin = false;
                    Data.Error = new ErrorInfo(ErrorDescription.CODE_AUTHENTICATE_FAIL,
                        ErrorDescription.MESSAGE_AUTHENTICATE_FAIL);
                }
            }
        }


        public GetMainInfoHandler(String token)
        {
            this.Token = token;
            Data = new ResultGetSubAuth();
            Result = Data;
            IsNeedAuthenticate = true;
        }
    }
}