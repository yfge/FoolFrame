using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
namespace Soway.Event
{
    public enum MsgState
    {
        [Enum("已经生成")]
        Generate=0,
        [Enum("已推送")]
        Push=1,
        [Enum("已经读取")]
        Readed = 2,
        [Enum("已经处理")]
        Dealed= 3,
        [Enum("已经超时")]
        TimeOut  = 4
        
    }
}
