using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public abstract class PropertyOperation
    {
        public abstract object Get(object ob);

        public abstract  void Set(object ob, object value);
        public Property Property { get; set; }
    }
}
