using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    public class TokenKeyGenerator
    {
        public string GetTokenKey(User.UserInfo user,User.AppInfo app)
        {
            return user.UserId + "&" + app.AppId;
        }
    }
}
