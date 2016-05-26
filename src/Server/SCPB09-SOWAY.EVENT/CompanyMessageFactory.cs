using System;
using Soway.Model;
using SOWAY.ORM.AUTH;
using System.Collections.Generic;
using Soway.Model.Context;

namespace Soway.Event
{
    internal class CompanyMessageFactory
    {
        private SqlCon con;
        private ICurrentContextFactory conFac;

        public CompanyMessageFactory(ICurrentContextFactory conFac, SqlCon con)
        {
            this.conFac = conFac;
            this.con = con;
        }


        internal void CreateMessage(dynamic company, dynamic obj, dynamic defs)
        {

            var fac = new DepMessageFactory(null, null);
            List<dynamic> users = new List<dynamic>();
            foreach(dynamic dep in company.Deps)
            {
                users.AddRange(fac.GetUsers(dep).ToArray());
            }
            new MessageFactory(con).CreateMessage(MsgNotifyType.Company, users, obj, defs);
        }

        
    }
}