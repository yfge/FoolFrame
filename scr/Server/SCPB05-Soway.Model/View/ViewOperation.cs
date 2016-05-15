using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;
using Soway.Model.View;

namespace Soway.Model
{

    [DataContract]
    [Table(Name = "SW_SYS_VIEW_OPERATION")]
    public class ViewOperation
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// 
        [DataMember]
        [Column(ColumnName="SW_VIEW_OPERATION_NAME")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 操作
        /// </summary>
        /// 
            [DataMember]
        [Column(ColumnName = "SW_VIEW_OPERATION_MODELOPERATION")]
        public OperationView Operation
        {
            get;
            set;
        }

        /// <summary>
        /// 结果视图
        /// </summary>
        /// 
            [DataMember]
        [Column(ColumnName = "SW_VIEW_OPERATION_RESULTVIEW")]
        public View.View ResultView
        {
            get;
            set;
        }

        /// <summary>
        /// 显示操作过程
        /// </summary>
        /// 
        [Column(ColumnName = "SW_VIEW_OPERATION_SHOWPROCESS")]
        public bool ShowProcess
        {
            get; set;
        }


        [Column(ColumnName = "SW_VIEW_OPERATION_INDEX")]
        public int Location
        {
            get; set;
        }

        [Column(ColumnName = "SW_VIEW_OPERATION_REQUIRESELECTB")]
        public bool RequireSelect
        {
            get; set;
        }
        [Column(ColumnName = "SW_VIEW_OPERATION_IMAGE")]

        public string Image { get; set; }
    }
}
