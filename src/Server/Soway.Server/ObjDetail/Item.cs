using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ObjDetail
{
    public class Item
    {
        public bool IsExist { get;   set; }
        public string ItemId { get; set; }
        public List<SaveKeypair> Propertyies { get; set; }


    }
}
