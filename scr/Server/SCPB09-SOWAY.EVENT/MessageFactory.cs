using Soway.Model;
using SOWAY.ORM.AUTH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Event
{
    class MessageFactory
    {
        private Model.Model model;
        private SqlCon con;
        private ICurrentContextFactory conFac;

        public SqlCon Con { get; private set; }
     
        public MessageFactory(Model.SqlCon con)
        {

            this.con = con;
            this.model = ModelFac.Models.First(p => p.ClassName == "Soway.Event.Message");

        }
        public void CreateMessage(MsgNotifyType notifyType,List<dynamic> users,dynamic ob, dynamic evtDef)
        {
            List<dynamic> items = new List<dynamic>();
            foreach(var user in users)
            {
                items.Add(createMsg(notifyType, user, ob, evtDef));
            }
            saveMsgToDb(items);
        }

        private void saveMsgToDb(List<dynamic> items)
        {

            var dbContext = new Soway.Model.SqlServer.dbContext(this.con, this.conFac);
            foreach(var item in items)
            {
                dbContext.Create(item);
            }
       
        }

        private dynamic createMsg(MsgNotifyType notifyType, dynamic user, dynamic ob, dynamic evtDef)
        {


            dynamic item = new SqlDataProxy(this.model, this.conFac, LoadType.Null, this.con);
           
                item.NotifyType = notifyType;
                item.NotifyUser = user.User;
                item.ObjId = (ob.ID ?? "").ToString();
                item.View = evtDef.View;
                item.GenerateTime = DateTime.Now;
                item.EvtId = evtDef.EventId;
                item.MessageId = Guid.NewGuid();
                item.MessageFmt = evtDef.Defination.MsgFmt;
                item.State = MsgState.Generate;
                item.ReadOperation = evtDef.Defination.Operation;

            
            return item;
        }

        
    }
}
