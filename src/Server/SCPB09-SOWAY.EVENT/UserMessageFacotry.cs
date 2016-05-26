using System;
using Soway.Model;
using Soway.Model.Context;
using SOWAY.ORM.AUTH;
using System.Collections.Generic;

namespace Soway.Event
{
    internal class UserMessageFacotry
    {
        private SqlCon con;
        private ICurrentContextFactory conFac;

        public UserMessageFacotry()
        {
        }

        public UserMessageFacotry(ICurrentContextFactory conFac, SqlCon con)
        {
            this.conFac = conFac;
            this.con = con;
        }

        internal void CreateMessage(List<dynamic> users, dynamic obj, dynamic defs)
        {
            new MessageFactory(con).CreateMessage(MsgNotifyType.User, users, obj, defs);
        }
    }
}