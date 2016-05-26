using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;


namespace Soway.Event
{
    [Table(Name ="SW_EVT_EVENT",ColPreStr ="EVT_")]
    public class Event
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        /// 
        [Column(ColumnName ="ID",IsKey =true)]
        public Guid EventId { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        /// 
        [Column(ColumnName ="CREATETIME",IsAutoGenerate = GenerationType.OnInSert)]
        public DateTime EventGenerationTime { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        /// 
        [Column(ColumnName ="MSG")]
        public String EventMsg { get; set; }

        /// <summary>
        /// 操作提示的信息
        /// </summary>
        /// 
        [Column(ColumnName ="DEALMSG")]
        public String DealOpertionText { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        /// 
        [Column(ColumnName ="DEALTIME")]
        public DateTime LastDealTime { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        /// 
        [Column(ColumnName ="DEALUSER")]
        public String LastDealUser { get; set; }

        [Column(ColumnName ="VIEW")]
        public Soway.Model.View.View View
        {
            get;set;
        }
        [Column(ColumnName ="DEF",IsKey =true,KeyGroupName ="MKONE")]
        public string ObjId
        {
            get;set;
        }
        [Column(ColumnName = "Defination",IsKey =true, KeyGroupName = "MKONE")]
        public EventDefination Defination { get; set; }


        public List<Message> Msgs { get; set; }
    }
}
