using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;

namespace Soway.Model
{
    [DataContract]
    [Table(Name = "SW_SYS_RELATION")]
    public class Relation
    {
        /// <summary>
        /// 关系
        /// </summary>
        /// 

        [DataMember]
         [Column(ColumnName = "SW_SYS_RELATION_TYPE")]
        public RelationType RelationType
        {
            get;
            set;
        }

        /// <summary>
        /// 属性
        /// </summary>
        /// 


        [DataMember]
          [Column(ColumnName = "SW_SYS_RELATION_SOURCEPROPERTY")]
        public Property Property
        {
            get;
            set;
        }

        /// <summary>
        /// 目标属性
        /// </summary>
        /// 

        [DataMember]
          [Column(ColumnName = "SW_SYS_RELATION_TARGETPROPERTY")]
        public Property TargetProperty
        {
            get;
            set;
        }

        /// <summary>
        /// 表名
        /// </summary>
        /// 

        [DataMember]
         [Column(ColumnName = "SW_SYS_RELATION_TABLE")]
        public string RelationTable
        {
            get;
            set;
        }

        /// <summary>
        /// 属性列
        /// </summary>
        /// 

        [DataMember]
          [Column(ColumnName = "SW_SYS_RELATION_SOURCECOL")]
        public string PropertyColumn
        {
            get;
            set;
        }

        /// <summary>
        /// 目标列
        /// </summary>
        /// 

        [DataMember]
          [Column(ColumnName = "SW_SYS_RELATION_TARGETCOL")]
        public string TargetColumn
        {
            get;
            set;
        }

        /// <summary>
        /// 可为空
        /// </summary>
        /// 

        [DataMember]
          [Column(ColumnName = "SW_SYS_RELATION_CANBENULL")]
        public bool CanBeNull
        {
            get;
            set;
        }
    }
}
