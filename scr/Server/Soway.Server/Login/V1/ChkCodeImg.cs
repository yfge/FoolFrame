using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Login.V1
{
  public  class ChkCodeImg
    {
        public string CheckCode { get; set; }
        public System.Drawing.Image Image { get; set; }

        public string Key = Guid.NewGuid().ToString();
    }
}
