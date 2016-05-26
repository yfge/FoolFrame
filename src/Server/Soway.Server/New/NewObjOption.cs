using Soway.Service.ObjDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.New
{
    public class NewObjOption :PostDataOption
    {
        public Obj SaveObj { get; set; }
        public string OwnerViewId { get; set; }
        public string OwnerId { get; set; }
        public string Property { get; set; }

    }
}
