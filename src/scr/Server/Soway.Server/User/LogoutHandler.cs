using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service.User
{
    internal class LogoutHandler : Handler
    {
        private PostDataOption Opt;
        public LogoutHandler(PostDataOption opt)
        {
            this.Opt = opt;
            this.Result = new Result();
        }

        protected override void ImplementBusinessLogic()
        {
            try
            {
                new CacheStore().Remove(this.Opt.Token);
            }
             catch
            {

            }
        }
    }
}