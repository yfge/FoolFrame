using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;


namespace Soway.Model.View
{





    [DataContract(IsReference=true)]
    [Table(Name = "SW_SYS_VIEW")]
    public class View
    {

        /// <summary>
        /// 模型
        /// </summary>
        [Column(ColumnName = "VIEW_MODEL")]
        [DataMember]
        public Model Model
        {
            get;
            set;
        }

        /// <summary>
        /// 视图名称
        /// </summary>
        /// 
        [Column(ColumnName = "VIEW_NAME",IsKey=true,KeyGroupName = "VIEWNAME")]
        [Soway.Data.Discription.Display.ShowDescription(DisplayName="视图名称",Display = true)]
        [DataMember]
        public string Name
        {
            get;
            set;
        }


        /// <summary>
        /// 显示的列
        /// </summary>
           [DataMember]
        public List<ViewItem> Items
        {
            get;
            set;
        }

        /// <summary>
        /// 筛选条件
        /// </summary>
        /// 
        [Column(ColumnName="VIEW_FILTER")]
        [DataMember]
        public string Filter
        {
            get;
            set;
        }


        /// <summary>
        /// 默认的详细视图
        /// </summary>
        [Column(ColumnName = "VIEW_DEFAULT")]
        [DataMember]
        public View DefaultDetailView { get; set; }


          List<ViewOperation> operation = new List<ViewOperation>();
          /// <summary>
          /// 视图上的操作
          /// </summary>
           [DataMember]
          public System.Collections.Generic.List<Soway.Model.ViewOperation> Operations
        {
            get
            {
                return operation;
            }
            set
            {
                operation = value;
            }
        }
    
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 类型
        /// </summary>
        [Column(ColumnName = "VIEW_TYPE")]
        [DataMember]
        public Soway.Model.ViewType ViewType
        {
            get;
            set;
        }

        [Soway.Data.Discription.ORM.Column(ColumnName = "VIEW_CONTYPE")]
        public ConnectionType ConnectionType { get; set; }


        /// <summary>
        /// ID
        /// </summary>
        [Soway.Data.Discription.ORM.Column(ColumnName = "VIEW_ID", IsKey = true, IsAutoGenerate = Soway.Data.Discription.ORM.GenerationType.OnInSert, IsIdentify = true)]
        [Soway.Data.Discription.Display.ShowDescription(DisplayName = "视图ID", Display = true)]
        [DataMember]
        public long ID { get; set; }


        [Column(ColumnName ="VIEW_FILE")]
        public ViewTemplateFile ViewFile { get; set; }


        [Column(ColumnName ="VIEW_CHECKAUTH")]
        public bool ReqCheck { get; set; }

        [Column(ColumnName ="VIEW_AUTOFRESHINTERVAL")]
        public int AutoFreshInterval { get; set; }


        [Column(ColumnName = "VIEW_CANEDIT")]
        public bool CanEdit { get; set; }
   
    }
}
