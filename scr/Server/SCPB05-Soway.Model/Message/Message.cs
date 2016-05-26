using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Message
{
    class Message
    {
       
        public Guid MessageId { get; set; }
        public DateTime GenerationTime { get; set; }
        public string MessageContent { get; set; }
        public long ViewId { get; set; }
        public object ObjId { get; set; }
    }
}
