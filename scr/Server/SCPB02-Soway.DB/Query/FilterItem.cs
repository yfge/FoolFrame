using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.DB.Query
{
    public class FilterItem
    {
        public System.Reflection.PropertyInfo Property { get; set; }
        public Object Value { get; set; }
    }
}
