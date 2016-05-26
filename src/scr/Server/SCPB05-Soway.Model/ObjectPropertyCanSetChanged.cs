using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public class ObjectPropertyCanSetChanged:System.EventArgs
    {
        public Property proerty { get; set; }
        public bool CanSet { get; set; }
    }
}
