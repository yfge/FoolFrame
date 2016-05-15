using Soway.Service.WebInvoke.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class Authenticate
    {
        public static int SESSION_TIMEOUT = 7200;
        public bool IsValidat()
        {


            sessionClient client = new sessionClient();
            if (this.PostData != null)
            {
                this.SesstionState = client.CheckSession(Guid.Parse(PostData.Token), SESSION_TIMEOUT);
                this.SessionId = Guid.Parse(PostData.Token);
            }
            else
            {

                this.SessionId = client.RegSession(SESSION_TIMEOUT);
                this.SesstionState = SessionState.Success;

            }

            return this.SesstionState ==  SessionState.Success;
        }
        public Guid SessionId { get; set; }
      
        public  SessionState SesstionState { get; private set; }
      
        private PostDataOption PostData { get; set; } 
        public Authenticate (PostDataOption postData)
        {
            this.PostData = postData;
        }
       
  
    }
}