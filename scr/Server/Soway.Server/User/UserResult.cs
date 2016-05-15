using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    public class UserResult:TokenResult
    {
        public UserInfo User { get; set; }
    }
}
