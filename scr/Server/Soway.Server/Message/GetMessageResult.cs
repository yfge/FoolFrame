using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service.Message
{
    
    public class GetMessageResult:Result
    {
        public List<MessageInfo> Messages { get; set; }
    }
}