using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service.Message
{
    public class GetNotifyResult :Result
    {
        public List<NotifyInfo> Notifies { get; set; }
    }
}