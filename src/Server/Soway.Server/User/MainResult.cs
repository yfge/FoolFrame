using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    public class MainResult :TokenResult
    {
        public AppInfo App { get; set; }
        public UserInfo User { get; set; }
        public List<Login.V2.AuthItem> TopMenu { get; set; }
    }
}
