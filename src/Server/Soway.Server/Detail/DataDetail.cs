using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Detail
{
    public class DataDetail
        
    {
        
        public string ObjId { get; set; }
        public string Name { get; set; }
        public List<ObjValuePair> SimpleData { get; set; }
        public List<PropertyDataItems> Items { get; set; }
        public string Model { get;  set; }

        public string ParentId { get; set; }

    } 









}
