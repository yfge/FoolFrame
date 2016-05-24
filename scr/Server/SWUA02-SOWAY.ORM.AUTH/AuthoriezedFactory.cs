using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model;
using Soway.Model.App;
using Soway.Model.Context;

namespace SOWAY.ORM.AUTH
{
    /// <summary>
    /// 得到一个应用的授权用户
    /// </summary>
    public class AuthoriezedFactory
    {
        public AuthoriezedFactory(Soway.Model.App.Application app,Soway.Model.Context.ICurrentContextFactory conFac)
        {
            this.App = app;
            this.ConFac = conFac;
        }

        
        public AuthorizedUser GetAuthrizedUser(SOWAY.ORM.AUTH.User user)
        {
            return  new Soway.Model.SqlServer.ObjectContext<AuthorizedUser>(App.SysCon,this.ConFac).GetDetail(user.UserID);

        }

        public SqlCon Con { get; private set; }
        public Application App { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }
    }
}
