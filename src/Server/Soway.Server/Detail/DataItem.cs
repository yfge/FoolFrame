using System.Collections.Generic;

namespace Soway.Service.Detail
{
    public  class DataItem
    {
        public string DataID { get; set; }
       
        public List<ObjValuePair> Values { get; set; }
    }
}