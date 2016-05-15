using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Service
{
    public class ViewOperation
    {
        public long ID;
        public string Name;

        public bool RequireSelect { get;   set; }
        public long ViewID { get;   set; }
    }
}
