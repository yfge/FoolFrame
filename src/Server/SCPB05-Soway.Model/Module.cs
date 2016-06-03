using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Soway.Model
{

    [DataContract(IsReference=true)]
    [Soway.Data.Discription.ORM.Table(Name="SW_SYS_MODULE")]
    public class Module :IEquatable<Module>
    {
        /// <summary>
        /// 名称
        /// </summary>
        /// 
        /// 
        /// 
              [DataMember]

        [Soway.Data.Discription.ORM.Column(ColumnName = "MODULE_NAME",IsKey = true,KeyGroupName="Name")]
        public string Name
        {
            get;
            set;
        }

     
              [DataMember]

          [Soway.Data.Discription.ORM.Column(ColumnName = "MODULE_REMARK")]
        public string Remartk
        {
            get;
            set;
        }

      
              [DataMember]

        [Soway.Data.Discription.ORM.Column(ColumnName = "MODULE_ASSEMBLY")]
        public string Assembly
        {
            get;
            set;
        }
        [Soway.Data.Discription.ORM.Column(ColumnName ="MODULE_FILENAME")]
        public string FileName
        {
            get;set;
        }

        /// <summary>
        /// 版本
        /// </summary>
        /// 
              [DataMember]

          [Soway.Data.Discription.ORM.Column(ColumnName = "MODULE_VERSION")]
        public string Verstion
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已经生成DLL
        /// </summary>
        /// 
              [DataMember]

        [Soway.Data.Discription.ORM.Column(ColumnName = "MODULE_GENERATIONCODE")]
        public bool GerationDLL
        {
            get;
            set;
        }
              [DataMember]


     
        public List<Module> Depdency
        {
            get;
            set;
        }

        [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "MODULE_CON")]
        public SqlCon SqlCon
        {
            get;
            set;
        }


        [Soway.Data.Discription.ORM.Column(NoMap =true)]
        internal string AssemblyFile { get;   set; }

        public bool Equals(Module other)
        {
            return this == other;
        }
    }
}
