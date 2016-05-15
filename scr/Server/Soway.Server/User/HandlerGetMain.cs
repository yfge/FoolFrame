using Soway.Service.Login.V1;
using Soway.Service.Login.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    class HandlerGetMain : Handler
    {
        MainResult result = new MainResult();
        public HandlerGetMain(PostDataOption postdata)
        {
            this.PostData = postdata;
            IsNeedAuthenticate = true;
            Result = result;
        }
        protected override void ImplementBusinessLogic()
        {
            var info = this.Info;

            result.User = info.User;
            result.Token = this.PostData.Token;
            result.App = info.App;
            result.TopMenu = new List<Login.V2.AuthItem>();


            var user = new Soway.Model.SqlServer.SqlContext<SOWAY.ORM.AUTH.User>(new bean.ConHelper().GetSysCon(),this).GetDetail(info.User.UserId);
            if (user != null)
            {

                var authuser = new SOWAY.ORM.AUTH.AuthoriezedFactory(new Model.App.Application() { SysCon = info.AppSqlCon },this).GetAuthrizedUser(user);
                List<SOWAY.ORM.AUTH.MenuItem> auths = new SOWAY.ORM.AUTH.MenuItemFactory(info.CurrentSqlCon,this).GetTopMenus(authuser);
                foreach (var menu in auths)
                {
                    var item = new Login.V2.AuthItem()
                    {
                        Text = menu.Text,
                        AuthType = AuthType.ListView,
                        ImageUrl = menu.Image,
                        Note = menu.Text,
                        ViewId = menu.ViewID,
                        ViewType = Soway.Service.Login.V1.ViewType.List,
                        Index = 1,
                        AuthNo = menu.ID.ToString()
                    };
                    result.TopMenu.Add(item);
                }


            }
        }
    }
}
