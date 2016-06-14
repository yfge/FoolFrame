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


            var appSyscxt = new Soway.Model.SqlServer.DynamicContext(info.AppSqlCon.ToString(), this);
            dynamic authUser = appSyscxt.GetById(
                typeof(SOWAY.ORM.AUTH.AuthorizedUser), info.User.UserId);
           
                List<SOWAY.ORM.AUTH.MenuItem> auths = new SOWAY.ORM.AUTH.MenuItemFactory(info.CurrentSqlCon,this).GetTopMenus(authUser);
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
