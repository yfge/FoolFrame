using System;
using Soway.Model;
using Soway.Model.Context;
using System.Collections.Generic;
using System.Linq;

namespace Soway.Event
{
    internal class AllMessageFac
    {
        private SqlCon appcon;
        private SqlCon con;
        private ICurrentContextFactory conFac;

        public AllMessageFac(ICurrentContextFactory conFac, SqlCon con,SqlCon appcon)
        {
            this.conFac = conFac;
            this.con = con;

            this.appcon = appcon;
        }

        internal void CreateMessage(dynamic obj, dynamic defs)
        {
            var userModel = ModelFac.Models.First(p => p.ClassName == "SOWAY.ORM.AUTH.AuthorizedUser");
            var items = new Soway.Model.SqlServer.dbContext(this.appcon, this.conFac).GetBySqlCommand(userModel, "Select * from [SW_APP_AUTH_USER]");
           
            new MessageFactory(con).CreateMessage(MsgNotifyType.All, items, obj, defs);
        }
    }
}