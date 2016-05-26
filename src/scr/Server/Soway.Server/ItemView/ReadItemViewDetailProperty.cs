using Soway.Service.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ItemView
{
    [DataContract]
    public class ReadItemViewDetailProperty : ReadItemViewItem
    {
     
        [DataMember]
        public List<ReadItemViewItem> Items { get; set; }
    }
}
