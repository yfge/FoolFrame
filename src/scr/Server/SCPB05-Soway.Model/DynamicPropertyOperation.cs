using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{


    public class DynamicPropertyOperation :PropertyOperation 
    {
        public override object Get(object ob)
        {


            dynamic ob1 = ob;
            return ob1[Property];

        }
        public override void Set(object ob, object value)
        {
            dynamic ob1 = ob;
            ob1[Property] = value;
        }

        internal DynamicPropertyOperation(Property property)
        {
            this.Property = property;
        }
    }
}
