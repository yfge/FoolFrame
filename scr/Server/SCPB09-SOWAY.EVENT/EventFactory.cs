using System;
using Soway.Model.Context;
using Soway.Model;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Soway.Event
{
    internal class EventFactory
    {
        private ICurrentContextFactory conFac;
        private Model.Model model;
        private SqlCon con;

        public EventFactory(ICurrentContextFactory conFac,Soway.Model.SqlCon con)
        {
            this.conFac = conFac;
            this.model = ModelFac.Models.First(p => p.ClassName == "Soway.Event.Event");
            this.con = con;

        }

        internal void CheckAndGenerate(dynamic defs,Soway.Model.SqlCon conStr, Soway.Model.SqlCon appsyscon)
        {


            List<dynamic> items = check(defs,conStr);
            if(items.Count > 0)
            {
                foreach(dynamic obj in items)
                {
                    dynamic evt = MakeEvent(defs, obj,conStr);
                    if (evt != null)
                    {
                        makeMessage(obj, evt, conStr,appsyscon);
                    }
                }
            }
        }

        private dynamic MakeEvent(dynamic defs, dynamic obj,Soway.Model.SqlCon conStr)
        {

            dynamic item = new SqlDataProxy(this.model, this.conFac, LoadType.Null, conStr);
           
            if(defs.Operation !=null)
            item.DealOpertionText = defs.Operation.Name.ToString();
            item.Defination = defs;
            item.EventGenerationTime = DateTime.Now;
            item.ObjId = (obj.ID ?? "").ToString();
            item.View = defs.View;
            item.EventMsg = defs.MsgFmt;
            var db = new Soway.Model.SqlServer.dbContext(conStr, this.conFac);
            if (db.IsExits(item))
                return null;
           db.Create(item);

            return item;
        }

        private void makeMessage(dynamic obj, dynamic defs,Soway.Model.SqlCon con, Soway.Model.SqlCon appcon)
        {


            bool IsAddMsg = false;
            if(defs.Defination.NotifyDeps!= null && defs.Defination.NotifyDeps.Count> 0)
            {
                IsAddMsg = true;
                var defmessageFac = new DepMessageFactory(this.conFac,con);
                
                foreach(var dep in defs.Defination.NotifyDeps)
                {
                    defmessageFac.CreateMessage(dep,obj,defs);
                }
            }  
            if(defs.Defination.NotifyRoles!= null && defs.Defination.NotifyRoles.Count > 0)
            {
                IsAddMsg = true;
                var rolemessageFac = new RoleMessageFactory(this.conFac,con);
                foreach(var role in defs.Defination.NotifyRoles)
                {
                    rolemessageFac.CreateMessage(role, obj, defs);
                }
            }

            if(defs.Defination.NotifyUsers!= null && defs.Defination.NotifyUsers.Count > 0)
            {
                IsAddMsg = true;
                var usermessageFac = new UserMessageFacotry(conFac, con);
                usermessageFac.CreateMessage(defs.Defination.NotifyUsers, obj, defs);
              

            } 
             
            if(defs.Defination.NotifyCompanies!= null  && defs.Defination.NotifyCompanies.Count > 0)
            {
                IsAddMsg = true;
                var companymessageFac = new CompanyMessageFactory(this.conFac,this.con);
                foreach(var company in defs.Defination.NotifyCompanies)
                {
                    companymessageFac.CreateMessage(company, obj, defs);
                }
            }

            if (IsAddMsg==false)
            {
                var allMessageFac = new AllMessageFac(conFac, con,appcon);
                allMessageFac.CreateMessage(obj, defs);
            }

        }

        private List<dynamic> check(dynamic defs,Soway.Model.SqlCon con)
        {
            List<dynamic> items = new List<dynamic>();
            var defcheck = new EventCheckFactory(con);
            return defcheck.GetObjs(defs);
          
        }
    }
}