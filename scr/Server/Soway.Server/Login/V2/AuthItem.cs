using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Soway.Service.Login.V1;

namespace Soway.Service.Login.V2
{
    public class AuthItem : global::Soway.Service.Login.V1.AuthItem
    {
        public String AuthNo { get; set; }
    }
}