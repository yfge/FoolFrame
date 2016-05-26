using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;

namespace Soway.Model.Triggers
{
    [Table(Name="SW_SYS_MODEL_TRIGGER")]

    [DataContract(IsReference=true)]
    public class ModelTrigger : IOperation
    {
        //  [Column(ColumnName (N))]

        /// <summary>
        /// 目标模型
        /// </summary>
        /// 

           [DataMember]
         [Column(ColumnName = "SW_MODEL_TRIGGER_ARGMODEL")]

        public Model ArgModel
        {
            get;
            set;
        }

           [DataMember]
               [Column(ColumnName = "SW_MODEL_TRIGGER_TYPE")]
        public ModelTriggerType ModelTriggerType
        {
            get;
            set;
        }

        /// <summary>
        /// 发生的条件
        /// </summary>
        /// 

           [DataMember]
               [Column(ColumnName = "SW_MODEL_TRIGGER_FILTER")]
        public string SourcFilter
        {
            get;
            set;
        }

        /// <summary>
        /// 影响的目标模型的条件
        /// </summary>
        /// 

           [DataMember]
               [Column(ColumnName = "SW_MODEL_TRIGGER_ARGFILTER")]
        public string ArgFilter
        {
            get;
            set;
        }

               private System.Collections.Generic.List<Soway.Model.ModelTriggerCommand> commands = new List<ModelTriggerCommand>();
           [DataMember]
               public System.Collections.Generic.List<Soway.Model.ModelTriggerCommand> Commands
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
               [Column(ColumnName = "SW_MODEL_TRIGGER_OPERATIONTYPE")]

               [DataMember]
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
           //[Column(ColumnName = "SW_MODEL_TRIGGER_FILTER")]
          
           //public string Filter { get; set; }
    }
}
