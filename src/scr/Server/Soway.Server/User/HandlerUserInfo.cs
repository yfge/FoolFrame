using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    class HandlerUserInfo : Handler
    {
        UserResult result = new UserResult();
        public HandlerUserInfo(PostDataOption data)
        {
            this.PostData = data;
            this.IsNeedAuthenticate = true;
            this.Result = result;
        }
        protected override void ImplementBusinessLogic()
        {

                result.User = Info.User;
                result.Token = this.PostData.Token;
                this.Result = result;
           
        }
    }
}
