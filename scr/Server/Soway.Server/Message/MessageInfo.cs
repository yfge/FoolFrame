using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Soway.Service.Message
{

    public class MessageInfo
    {
        public String MessageID { get; set; }
        public DateTime GernerationTime { get; set; }
        public String MessageContent { get; set; }
        public long ResultView { get; set; }
        public String ObjId { get; set; }
        public String ResultViewType { get; set; }
        public String ResultKey { get; set; }
        public bool IsRead { get; set; }

        public bool IsTimeOut { get; set; }
        public DateTime ReadDateTime { get; set; }
        public MessageInfo()
        {
            ReadDateTime = DateTime.Now;
            GernerationTime = DateTime.Now;
        }
    }
}