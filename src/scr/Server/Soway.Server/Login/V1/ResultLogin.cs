using Soway.Service.Handlers1._0.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service.Login.V1
{
    public class ResultLogin : TokenResult
    {
        
        public bool LoginSucess { get; set; }
       
        internal ResultLogin()
        {
            
        }


    }
}