using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ObjDetail
{
    public class ItemProperty
    {
        public string Key { get; set; }

        public List<Item> Items { get; set; }

        public List<Item> DelteItems { get; set; }
        public List<Item> AddedItems { get; set; }
    }
}
