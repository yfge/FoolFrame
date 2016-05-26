using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;

namespace Soway.Model.Triggers
{
    /// <summary>
    /// 属性触发器,当一个属性被更改时发生
    /// 
    /// </summary>
    /// 

    [DataContract(IsReference=true)]
    [Table(Name = "SW_SYS_PROPERTY_TRIGGER")]
    public class PropertyTrigger : IOperation
    {
            [DataMember]
        [Column(ColumnName = "SW_PROPERTY_TRIGGER_ARGFILTER")]
        public string ArgFilter { get; set; }

            [DataMember]
         [Column(ColumnName = "SW_PROPERTY_TRIGGER_ARGMODEL")]
        public Model ArgModel { get; set; }
        /// <summary>
        /// 发生的条件
        /// </summary>
        /// 

            [DataMember]
         [Column(ColumnName = "SW_PROPERTY_TRIGGER_FILTER")]
        public string FilterValue
        {
            get;
            set;
        }

        /// <summary>
        /// 事件的类型
        /// </summary>
        /// 

            [DataMember]
         [Column(ColumnName = "SW_PROPERTY_TRIGGER_TYPE")]

        public Soway.Model.PropertyTriggerType PropertyTriggerType
        {
            get;
            set;
        }

        /// <summary>
        /// 引发的操作
        /// </summary>
        /// 
         private System.Collections.Generic.List<Soway.Model.PropertyTriggerCommand> commands = new List<PropertyTriggerCommand>();
         [DataMember]
         
        public System.Collections.Generic.List<Soway.Model.PropertyTriggerCommand> Commands
        {
            get
            {
                return commands;
            }
            set
            {
                commands = value;
            }
        }

        /// <summary>
        /// 触发器的名称
        /// </summary>
        /// 
         [Column(ColumnName = "SW_PROPERTY_TRIGGER_NAME")]
         [DataMember]
        public string Name
        {
            get;
            set;
        }
            [DataMember]
        /// <summary>
        /// 发生的属性
        /// </summary>
        /// 
         [Column(ColumnName = "SW_PROPERTY_TRIGGER_PROPERTY")]

        public Property Property
        {
            get;
            set;
        }
            [DataMember]
        [Column(ColumnName = "SW_PROPERTY_TRIGGER_BASETYPE")]

        public BaseOperationType BaseOperationType
        {
            get;
            set;
        }
        public List<ICommand> GetCommands()
        {

            List<ICommand> result = new List<ICommand>();
            foreach (var i in this.Commands)
                result.Add(i);
            return result;
        }

            [DataMember]
        [Column(ColumnName = "SW_MODEL_TRIGGER_INVOKEDLL")]


        public string InvokeDLL
        {
            get;
            set;
        }
            [DataMember]
        [Column(ColumnName = "SW_MODEL_TRIGGER_INVOKECLASS")]
        public string InvokeClass
        {
            get;
            set;
        }
            [DataMember]
        [Column(ColumnName = "SW_MODEL_TRIGGER_INVOKEMETHOD")]
        public string InvokeMethod
        {
            get;
            set;
        }
            // [Column(ColumnName = "SW_MODEL_TRIGGER_FILTER")]
            //public string Filter { get; set; }
    }
}
