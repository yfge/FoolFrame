using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soway.Model;
using Soway.Event;

namespace Soway.Service.Message
{
    class HandlerGetMessage : Handler

    {
        private Model.Model model;
        private GetMessageResult msgResult;

        public HandlerGetMessage(PostDataOption option)
        {
            this.PostData = option;
            this.msgResult = new GetMessageResult();
            this.msgResult.Messages = new List<MessageInfo>();
            this.Result = this.msgResult;
            this.IsNeedAuthenticate = true;
            this.model = Global.Models.First(p => p.ClassName == "Soway.Event.Message");

        
        }
        protected override void ImplementBusinessLogic()
        {


            //
            var sqlDataCommand = new System.Data.SqlClient.SqlCommand();
            sqlDataCommand.CommandText = "SELECT TOP 1 * from [SW_SYS_MSG] where [MSG_STATE]=@msgState and [MSG_USERID] = @userid order by [MSG_GENTIME] desc";
            sqlDataCommand.Parameters.AddWithValue("@msgState", (int)Soway.Event.MsgState.Generate);
            sqlDataCommand.Parameters.AddWithValue("@userid", this.Info.User.UserId);
            var db = new Soway.Model.SqlServer.dbContext(this.Info.CurrentSqlCon, this);

            foreach(var msg in db.GetBySqlCommand(this.model, sqlDataCommand))
            {
                this.msgResult.Messages.Add(new MessageInfo()
                {
                    GernerationTime =msg.GenerateTime,
                    IsTimeOut = false,
                    IsRead = false,
                    MessageContent = msg.MessageFmt,
                    MessageID = msg.MessageId.ToString(),
                    ResultKey =msg.ObjId,
                    ResultView = (msg.View ==null? 0:msg.View.ID)

                });
                msg.State = MsgState.Push;
                msg.PushTime = DateTime.Now;
                db.Save(msg);
            }

           
  
        }
    }
}
