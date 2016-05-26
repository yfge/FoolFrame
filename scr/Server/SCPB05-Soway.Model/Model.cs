using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Soway.Model
{
    [DataContract(IsReference=true)]
    [Soway.Data.Discription.ORM.Table(Name = "SW_SYS_MODEL")]
    public class Model :IEquatable<Model>
    {
     
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_NAME",IsKey = true ,KeyGroupName = "NAME")]
        [DataMember]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 类名称
        /// </summary>
        /// 
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_CLASS", IsKey = true, KeyGroupName = "CLASSNAME")]
        [DataMember]
        public string ClassName
        {
            get;
            set;
        }

        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_CONTYPE")]
        public ConnectionType ConnectionType { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        /// 
        [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_DATABASETABLE")]
        public string DataTableName
        {
            get;
            set;
        }
 
        /// <summary>
        /// 基类型
        /// </summary>
        /// 
           [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_PARENT")]
        public Model BaseModel
        {
            get;
            set;
        }
           /// <summary>
           /// 模块
           /// </summary>
           [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_MODULE")]
        public Module Module
        {
            get;
            set;
        }

        /// <summary>
        /// 是否生成自增长ID
        /// </summary>
        /// 
           [DataMember]
         [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_AUTOID")]
        public bool AutoSysId
        {
            get;
            set;
        }
         /// <summary>
         /// ID的属性名称
         /// </summary>
         /// 
           [DataMember]
         [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_IDPROPERTY")]
         public Property IdProperty
         {
             get;
             set;
         }

           /// <summary>
           /// 默认的格式（未启用）
           /// </summary>
           [DataMember]
         [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_DEFAULTFORMAT")]
         public string DefaultFormat { get; set; }

        private List<Property> items = new List<Property>();
        /// <summary>
        /// 属性
        /// </summary>
           [DataMember]
        public List<Property> Properties
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        public override string ToString()
        {
            return String.Format("Name:{0},Class{1},Modules{2}", this.Name, this.ClassName, this.Module.Name);//.GetHashCode();

        }
        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
         [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_TYPE")]
        public ModelType ModelType
        {
            get;
            set;
        }

        private List<EnumValues> values = new List<EnumValues>();

        /// <summary>
        /// 枚举值
        /// </summary>
           [DataMember]
       
        public List<EnumValues> EnumValues
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
            }
        }
           /// <summary>
           /// 在数据库中是否是一个视图
           /// </summary>
           [DataMember]
        /// <summary>
        /// 是否为视图(抽象类)
        /// </summary>
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_ISVIEW")]
        public bool IsView
        {
            get;
            set;
        }


        private List<Relation> relation = new List<Relation>();

        /// <summary>
        /// 关系
        /// </summary>
           [DataMember]
        public List<Relation> Relations
        {
            get
            {
                return relation;
            }
            set
            {
                relation = value;
            }
        }

           /// <summary>
           /// 数据库连接
           /// </summary>
           [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_CON")]
        public SqlCon SqlCon
        {
            get;
            set;
        }

           /// <summary>
           /// ID
           /// </summary>
           [DataMember]

        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_ID", 
            IsKey = true, IsAutoGenerate = Soway.Data.Discription.ORM.GenerationType.OnInSert, 
            IsIdentify = true)]
        public long ID { get; set; }

        /// <summary>
        /// 主要显示的属性
        /// </summary>
        /// 
           [DataMember]
        public Property ShowProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// 

        List<Operation> operation = new List<Operation>();
        /// <summary>
        /// 操作
        /// </summary>
      [DataMember]
        public List<Operation> 
          Operations
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


        private List<Triggers.ModelTrigger> triggers = new List<Triggers.ModelTrigger>();
        /// <summary>
        /// 触发器
        /// </summary>
           [DataMember]
        public List<Triggers.ModelTrigger> Triggers
        {
            get
            {
                return triggers;
            }
            set
            {
                triggers = value;
            }
        }

           /// <summary>
           /// 默认列表视图
           /// </summary>
           [DataMember]
                [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_DEFAULTLISTVIEW")]
        public View.View DefaultListView { get; set; }
           /// <summary>
           /// 默认详细视图
           /// </summary>
           [DataMember]
                [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_DEFAULTITEMVIEW")]

        public View.View DefualtItemView { get; set; }


                public bool Equals(Model other)
                {

                    return this == other;
                }

        [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODEL_DEFAULTOWNER")]
        public Model Owner { get; set; }
    }
}
