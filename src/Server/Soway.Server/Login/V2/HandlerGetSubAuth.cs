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
    class HandlerGetSubAuth : Handler
    {
        private ResultGetSubAuth Data;
        private GetSubAuthOption Option;
        public HandlerGetSubAuth(GetSubAuthOption option)
        {
            Option = option;
            PostData = option;
            Data = new ResultGetSubAuth();
            IsNeedAuthenticate = true;
            Result = Data;
        
        }
        protected override void ImplementBusinessLogic()
        {

            var info = Info;
            Data.Token = this.PostData.Token;
            Data.Items = new List<AuthItem>();
            Data.Token = this.PostData.Token;


                var appSyscxt = new Soway.Model.SqlServer.DynamicContext(info.AppSqlCon.ToString(), this);
            dynamic authUser = appSyscxt.GetById(
                typeof(SOWAY.ORM.AUTH.AuthorizedUser), info.User.UserId);
                List<SOWAY.ORM.AUTH.MenuItem> auths = new List<SOWAY.ORM.AUTH.MenuItem>();
                if (String.IsNullOrEmpty(this.Option.ParentAuthCode))
                    auths = new SOWAY.ORM.AUTH.MenuItemFactory(info.CurrentSqlCon,this).GetTopMenus(authUser);
                else
                    auths = new SOWAY.ORM.AUTH.MenuItemFactory(info.CurrentSqlCon,this).GetMenus(authUser, System.Convert.ToInt64(this.Option.ParentAuthCode));
                foreach (var menu in auths.OrderBy(p=>p.Index))
                {
                    var item = new Login.V2.AuthItem()
                    {
                        Text = menu.Text,
                        AuthType = AuthType.ListView,
                        ImageUrl = menu.Image,
                        Note = menu.Text,
                        ViewId = menu.ViewID,
                        ViewType = Soway.Service.Login.V1.ViewType.List,
                        Index = menu.Index,

                        AuthNo = menu.ID.ToString()
                    };
                    Data.Items.Add(item);
                }              
        
        }

    }




}