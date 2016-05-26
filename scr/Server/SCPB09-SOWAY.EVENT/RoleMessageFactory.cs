using System;
using Soway.Model;
using Soway.Model.Context;
using SOWAY.ORM.AUTH;
using System.Collections.Generic;

namespace Soway.Event
{
    internal class RoleMessageFactory
    {
        private SqlCon con;
        private ICurrentContextFactory conFac;
 

        public RoleMessageFactory(ICurrentContextFactory conFac, SqlCon con)
        {
            this.conFac = conFac;
            this.con = con;
        }

        internal void CreateMessage(dynamic role, dynamic obj, dynamic defs)
        {
            List<dynamic> users = new List<dynamic>();
            foreach(dynamic user in role.AuthUsers)
            {
                users.Add(user);

            }
            new MessageFactory(con).CreateMessage(MsgNotifyType.Role, users, obj, defs);
        }
    }
}