using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public class QueryContext
    {
        public Model Model { get; internal set; }
        public String Filter { get; set; }
        public ViewItem OderByItem { get; set; }
        public OrderByType OrderByType { get; set; }
    }
}
