using Soway.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Soway.Service.Detail
{
    [DataContract]
    public class ReadItemViewItem
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Data.PropertyType PrpType { get; set; }
        [DataMember]
        public int Index
        {
            get; set;
        }
        [DataMember]
        public string PrpId { get;  set; }
        [DataMember]
        public long PrpModelId { get;  set; }
        [DataMember]
        public string ID { get;  set; }
        [DataMember]
        public string PrpShowName { get;  set; }
        [DataMember]
        public bool ReadOnly { get; set; }
        [DataMember]
        public ItemEditType EditType { get; set; }
    }
}
