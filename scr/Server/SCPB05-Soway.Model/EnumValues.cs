using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Soway.Model
{

    [DataContract]
    [Soway.Data.Discription.ORM.Table(Name = "SW_SYS_EMUNVALUE")]
    public class EnumValues
    {
        [Soway.Data.Discription.ORM.Column(ColumnName = "EMUN_STR")]
        [DataMember]
        public string String
        {
            get;
            set;
        }

        [DataMember]
        [Soway.Data.Discription.ORM.Column(ColumnName = "EMUN_VALUE")]
        public int Value
        {
            get;
            set;
        }
    }
}
