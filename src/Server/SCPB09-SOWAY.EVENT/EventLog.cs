using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using Soway.Model.View;

namespace Soway.Event
{


    
    [Table(Name ="SW_SYS_MSG",ColPreStr ="MSG_")]
    public class Message
    {


        /// <summary>
        /// 消息ID
        /// </summary>
        /// 
        [Column(ColumnName ="ID",IsAutoGenerate = GenerationType.OnInSert,IsKey =true)]
        public Guid MessageId { get; set; }
        /// <summary>
        /// 事件ID
        /// </summary>
        [Column(ColumnName ="EVT")]
        public Guid EvtId { get; set; }
        /// <summary>
        /// 视图ID
        /// </summary>
        /// 
        [Column(ColumnName ="VIEW")]
        public View View
        {
            get;set;

        }
        /// <summary>
        /// 对像ID 
        /// </summary>
        /// 
        [Column(ColumnName ="OBJ")]
        public String ObjId { get; set; }

        /// <summary>
        /// 消息格式 
        /// </summary>
        [Column(ColumnName ="MSG")]
        public String MessageFmt { get; set; }

       
        /// <summary>
        /// 生成时间
        /// </summary>
        /// 
        [Column(ColumnName ="CREATETIME",IsAutoGenerate =  GenerationType.OnInSert)]
        public DateTime GenerateTime { get; set; }
        /// <summary>
        /// 读取时间 
        /// </summary>
        /// 
        [Column(ColumnName ="READTIME")]
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        /// 
        [Column(ColumnName ="PUSHTIME")]
        public DateTime PushTime { get; set; }


        /// <summary>
        /// 读取超时时间
        /// </summary>
        /// 
        [Column(ColumnName ="ENDLINETIME")]
        public DateTime ReadTimeOutTime { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        /// 
        [Column(ColumnName ="STATE")]
        public MsgState State { get; set; }
        /// <summary>
        /// 已经读操作
        /// </summary>
        /// 
        [Column(ColumnName ="READOPERATION")]
        public Soway.Model.Operation ReadOperation { get; set; }

        /// <summary>
        /// 通知人
        /// </summary>
        [Column(ColumnName ="USERID")]
        public SOWAY.ORM.AUTH.User NotifyUser { get; set; }


        /// <summary>
        /// 通知类型
        /// </summary>
        /// 
        [Column(ColumnName ="MSGTYPE")]
        public MsgNotifyType  NotifyType { get; set; }
      
    }
}