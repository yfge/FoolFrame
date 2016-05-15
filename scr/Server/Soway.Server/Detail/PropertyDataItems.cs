using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Detail
{
    public class PropertyDataItems
    {
        public List<ReadItemViewItem> Properties { get; set; }
        public List<DataItem> Items { get; set; }

        public long ListViewId { get; set; }
        public long DetailViewId { get; set; }

        public String Name { get; set; }
        public string PrpId { get;  set; }

         
        public bool SelectFromExists { get; set; }
        public string ItemName { get;  set; }
        public long SelectedView { get;   set; }
    }
}
