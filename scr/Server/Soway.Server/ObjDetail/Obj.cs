using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ObjDetail
{
    public class Obj
    {
        public string Id { get; set; }
        public List<SaveKeypair> Propertyies { get; set; }

        public List<ItemProperty> Itemproperties { get; set; }
        public string ViewID { get;  set; }

        public string ParentId { get; set; }

        public string Model { get; set; }
    }
}
