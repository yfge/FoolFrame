using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Login.V1
{
    public class AuthItem
    {
        //名称
        public string Text { get; set; }
       
        //说明
        public string Note { get; set; }
        //图片
        public string ImageUrl { get; set; }

        //类型
        public AuthType AuthType { get; set; }
        //ViewID
        public long ViewId { get; set; }
    
        //通知数量
        public int NotifyCount { get; set; }
        //视图的类型
        public ViewType ViewType { get; set; }

        //顺序
        public int Index { get; set; }

        //子类型
        public List<AuthItem> SubItems { get; set; }
    }
}

