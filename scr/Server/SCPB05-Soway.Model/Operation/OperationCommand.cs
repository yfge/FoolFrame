using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;

namespace Soway.Model
{

    
    [Table(Name = "SW_SYS_COMMANDS")]
    [DataContract]
    public class OperationCommand  : Soway.Model.ICommand
    {
        /// <summary>
        /// 类型
        /// </summary>
        /// 

              [DataMember]

        [Column(ColumnName ="SW_SYS_COMMAND_TYPE")]
        public CommandsType CommandsType
        {
            get;
            set;
        }
              [DataMember]

        /// <summary>
        /// 属性
        /// </summary>
        /// 
             [Column(ColumnName = "SW_SYS_COMMAND_PROPERTY")]
        public Property Property
        {
            get;
            set;
        }

        /// <summary>
        /// 表达式
        /// </summary>
        /// 

              [DataMember]

             [Column(ColumnName = "SW_SYS_COMMAND_EXP")]
        public string Exp
        {
            get;
            set;
        }

        /// <summary>
        /// 目标模型
        /// </summary>
        /// 

              [DataMember]

             [Column(ColumnName = "SW_SYS_COMMAND_ARGMODEL")]
        public Model ArgModel
        {

            get;
            set;
        }
             [Column(ColumnName = "SW_SYS_COMMAND_ARGEXP")]
        /// <summary>
        /// 目标属性返回值
        /// </summary>
        /// 

             [DataMember]

        public string ArgPropertyExp
        {
            get;
            set;
        }

        /// <summary>
        /// 目标模型的ID表达式
        /// </summary>
        /// 

              [DataMember]

             [Column(ColumnName = "SW_SYS_COMMAND_ARGID")]
        public string ArgSourceIdExp
        {
            get;
            set;
        }


              [Column(ColumnName = "SW_SYS_COMMAND_INDEX")]
              public int Index
              {
                  get;
                  set;
              }

        [Column(ColumnName = "SW_SYS_COMMAND_PROPERTY_EXP")]
        public string PropertyExp
        {
            get; set;
        }
        [Column(ColumnName = "SW_SYS_COMMAND_TEMPVALUE")]
        public string TempResultValue
        {
            get; set;
        }

    }
}
