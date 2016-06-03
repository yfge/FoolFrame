using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Discription.Display
{
    public class EntityAttribute :Attribute
    {
        public string Name { get; set; }
        public EntityAttribute() { }
        public EntityAttribute(String name) { Name = name; }
    }
}
