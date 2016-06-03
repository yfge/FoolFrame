using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    class HandlerAppInfo : Handler
    {
      
   

        public HandlerAppInfo(PostDataOption data)
        {
            this.PostData = data;
            this.IsNeedAuthenticate = true;
        }
        protected override void ImplementBusinessLogic()
        {

            var info = new CacheStore().Get(this.PostData.Token);

            AppResult result = new AppResult(); ;
            if(info != null)
            {


              
                result.App= info.App;
                result.Token = this.PostData.Token;
                this.Result = result;
            }
            else
            {
                this.Result = new TokenResult()
                {
                    Token = this.PostData.Token,
                    Error = new ErrorInfo()
                    {
                        Code = ErrorDescription.TOKEN_INVALIDAT,
                        Message = ErrorDescription.TOKEN_INVALIDAT_MSG
                    }
                };
            }

        }
    }
}
