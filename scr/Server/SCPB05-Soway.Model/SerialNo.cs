using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    /// <summary>
    /// 表示一个流水化的序列号
    /// </summary>
    public class SerialNo
    {

        public string Format { get; set; }
        public List<Property> Source { get; set; }
    }
}
