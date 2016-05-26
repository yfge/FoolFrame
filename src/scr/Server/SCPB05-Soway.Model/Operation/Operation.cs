using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;

namespace Soway.Model
{
    [Table(Name = "SW_SYS_OPERATION")]
    [DataContract(IsReference=true)]
    public class Operation : Soway.Model.IOperation
    {
        [DataMember]
        [Column(ColumnName = "SW_MODEL_OPERATION_NAME")]
        public string Name
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName = "SW_MODEL_OPERATION_FILTER")]
        public string Filer
        {
            get;
            set;
        }
        private System.Collections.Generic.List<Soway.Model.OperationCommand> commands = new List<OperationCommand>();
        [DataMember]
        public System.Collections.Generic.List<Soway.Model.OperationCommand> Commands
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
        [Column(ColumnName = "SW_MODEL_OPERATION_BASETYPE")]
        [DataMember]
        public BaseOperationType BaseOperationType
        {
            get;
            set;
        }

        
        [DataMember]
        [Column(ColumnName = "SW_MODEL_OPERATION_ARGMODEL")]
        public Model ArgModel
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName = "SW_MODEL_OPERATION_ARGFILTER")]
        public string ArgFilter
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




        [Column(ColumnName = "SW_MODEL_OPERATION_INVOKEDLL")]
        [DataMember]

        public string InvokeDLL
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName = "SW_MODEL_OPERATION_INVOKECLASS")]
        public string InvokeClass
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName = "SW_MODEL_OPERATION_INVOKEMETHOD")]
        public string InvokeMethod
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName = "SW_MODEL_OPERATION_RETURNMODEL")]
        public Model ReturnModel
        {
            get;
            set;
        }

        public List<OperationParam> Params { get; set; }

        /// </summary>
        [DataMember]

        [Soway.Data.Discription.ORM.Column(ColumnName = "SysId",
         IsKey = true, IsAutoGenerate = Soway.Data.Discription.ORM.GenerationType.OnInSert,
         IsIdentify = true)]
        public long ID { get; set; }


    }

}
