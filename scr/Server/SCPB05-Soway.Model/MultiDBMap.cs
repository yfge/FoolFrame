using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Soway.Model
{
    [DataContract]
         [Soway.Data.Discription.ORM.Table(Name = "SW_SYS_MULTIMAP")]
         
    public class MultiDBMap
    {
        [Soway.Data.Discription.ORM.Column(ColumnName = "MAP_NAME")]
        [DataMember]

        public string PropertyName
        {

            get;
            set;
        }

        [Soway.Data.Discription.ORM.Column(ColumnName = "MAP_COLNAME")]
        [DataMember]

        public string DBColName
        {
            get;
            set;
        }
    }
}
