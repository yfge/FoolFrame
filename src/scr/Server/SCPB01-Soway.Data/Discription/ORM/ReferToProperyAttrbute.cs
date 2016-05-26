using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple =false)]
    public class ReferToProperyAttrbute :Attribute
    {
        public String PropertyName { get; private  set; }
        public ReferToProperyAttrbute(String propertyName) { this.PropertyName = propertyName; }
    }
}
