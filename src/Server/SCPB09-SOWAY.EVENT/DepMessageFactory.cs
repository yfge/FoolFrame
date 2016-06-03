using System;
using Soway.Model;
using Soway.Model.Context;
using SOWAY.ORM.AUTH;
using System.Collections.Generic;

namespace Soway.Event
{
    internal class DepMessageFactory
    {
        private SqlCon con;
        private ICurrentContextFactory conFac;

        public DepMessageFactory(Soway.Model.Context.ICurrentContextFactory conFac)
        {
            this.conFac = conFac;
        }

        public DepMessageFactory(ICurrentContextFactory conFac, SqlCon con) : this(conFac)
        {
            this.con = con;
        }
        internal List<dynamic> GetUsers(dynamic dep)
        {
            List<dynamic> users = new List<dynamic>();

            foreach (dynamic user in dep.Users)
            {
                users.Add(user);

            }
            AddUsers(users, dep.SubDepartments);
            return users;
        }
        internal void CreateMessage(dynamic dep, dynamic obj, dynamic defs)
        {

            var users = GetUsers(dep);
            new MessageFactory(con).CreateMessage(MsgNotifyType.Dep, users, obj, defs);

        }
        private void AddUsers(List<dynamic> users, dynamic subDepartments)
        {

            if (subDepartments != null && subDepartments.Count > 0)
                foreach (dynamic dep in subDepartments)
                {

                    foreach (dynamic user in dep.Users)
                    {
                        users.Add(user);
                    }
                    AddUsers(users, dep.SubDepartments);
                }

           
        }
    }
}