using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ItemView
{
    [DataContract]
    public class ReadItemView :TokenResult
    {
        [DataMember]
        public string ViewName { get; set; }
        [DataMember]
        public long ViewId { get; set; }
        [DataMember]
        public List<Detail.ReadItemViewItem> Items
        {
            get; set;
        }
        [DataMember]
        public List<ReadItemViewDetailProperty> DetailViews { get; internal set; }
    }
}
