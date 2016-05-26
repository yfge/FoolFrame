using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Context
{
    public class CurrentContext
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }

       
        public SqlCon SysCon { get; set; }
        public SqlCon ModelCon { get; set; }
        public SqlCon AppCon { get; set; }
        public SqlCon CurrentCon { get; set; }

    }
}
