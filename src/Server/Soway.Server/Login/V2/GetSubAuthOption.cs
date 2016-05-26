using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Login.V2
{
   public class GetSubAuthOption:Soway.Service.PostDataOption
    {
        public string ParentAuthCode { get; set; }
    }
}
