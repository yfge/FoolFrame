using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Soway.Model
{

    [DataContract(IsReference =true)]
     [Soway.Data.Discription.ORM.Table(Name = "SW_SYS_PROPERTY")]
    public class Property
    {



     
         //[Soway.Data.Discription.ORM.Column(ColumnName ="SysId",IsKey =true,IsIdentify = true,IsAutoGenerate=Soway.Data.Discription.ORM.GenerationType.OnInSert)]
         //public long Id { get; set; }
         public Property() { AllowDBNull = true; }
        /// <summary>
        /// 类型
        /// </summary>
        /// 

           [DataMember]
         [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_TYPE")]
        public PropertyType PropertyType
        {
            get;
            set;
        }
        [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_CONTYPE")]
        public ConnectionType ConnectionType { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// 

        [DataMember]
           [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_NAME")]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 模型类型
        /// </summary>
        /// 
                 [DataMember]
         [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_MODEL")]
        public Model Model
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是数组（一对多）
        /// </summary>
        /// 

           [DataMember]
              [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_ISARRAY")]
        public bool IsArray
        {
            get;
            set;
        }
           /// <summary>
           /// 数据库中的列名
           /// </summary>
           [DataMember]
              /// <summary>
              /// 数据库列名称,如果是单独的属性列, 则为本表中的列,如果是集合类型,则为集合表中的外键
              /// </summary>
              [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_COLNAME")]
        public string DBName
        {
            get;
            set;
        }


        /// <summary>
        /// 属性名称
        /// </summary>
        /// 

           [DataMember]
              [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_PROPERTYNAME")]
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是多重映射
        /// </summary>
        /// 
        /// 
        /// 

           [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_MULTIMAP")]
        public bool IsMultiMap
        {
            get;
            set;
        }

        private List<MultiDBMap> maps = new List<MultiDBMap>();
        /// <summary>
        /// 多重映射
        /// </summary>
        /// 
           [DataMember]
        public List<MultiDBMap> DBMaps
        {
            get
            {
                return maps;
            }
        }

        /// <summary>
        /// 外键组名
        /// </summary>
        /// 

           [DataMember]
           [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_IXGRPOUP")]
        public string IXGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 是否有Check约束
        /// </summary>
        /// 

           [DataMember]
           [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_ISCHECK")]
        public bool IsCheck
        {
            get;
            set;
        }


        [Soway.Data.Discription.ORM.Column(NoMap =true)]
        public bool KeysCanBeDefault
        {
            get;set;

        }
        /// <summary>
        /// 生成方式
        /// </summary>
        /// 

           [DataMember]
           [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_GENERATIONTYPE")]
        public Soway.Data.Discription.ORM.GenerationType AutoGenerationType
        {
            get;
            set;
        }

        public override string ToString()
        {

            return this.Name + " " + this.DBName;
        }

        /// <summary>
        /// 允许空
        /// </summary>
        /// 

           [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_ALLOWDBNULL")]
        public bool AllowDBNull
        {
            get;
            set;
        }

        /// <summary>
        /// 可写
        /// </summary>
        /// 
           [DataMember]
             [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_CANGET")]
        public bool CanGet
        {
            get;
            set;
        }

        /// <summary>
        /// 可读
        /// </summary>
        /// 

           [DataMember]
             [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_CANSET")]
        public bool CanSet
        {
            get;
            set;
        }

             /// <summary>
             /// 筛选条件
             /// </summary> 
             /// 
             /// 

           [DataMember]
             [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_FILTER")]
             public string Filter
             {
                 get;
                 set;
             }
           /// <summary>
           /// 来源
           /// </summary>
           [DataMember]
             /// <summary>
             /// 源
             /// </summary>
             [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_SOURCE")]
             public string Source
             {
                 get;
                 set;
             }


           /// <summary>
           /// 格式
           /// </summary>
           [DataMember]
             [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_FORMAT")]
             public string Format { get; set; }

       

        [Soway.Data.Discription.ORM.Column(ColumnName = "PROPERTY_SQLCON")]
        public SqlCon PropertySqlCon { get; set; }
             private List<Triggers.PropertyTrigger> triggers = new List<Triggers.PropertyTrigger>();
             /// <summary>
             /// 触发器
             /// </summary>
             [DataMember]
             public List<Triggers.PropertyTrigger> Triggers
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
             //     [Soway.Data.Discription.ORM.Column( 
             //         ColumnName = "SW_SYS_MODEL_PropertiesSysId",IsKey = true,KeyGroupName="Key")]
             //     [DataMember]
             //public Model ParentMode { get; set; }



    }
}
