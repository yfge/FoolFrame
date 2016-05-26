using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    internal class PropertyData
    {

        public PropertyData(Property property)
        {
            this.Property = property;
        }

        public LoadType IsLoad { get; set; }

        private object data;
        public object Data
        {
            get
            {
                return data;
            }
            set
            {

                if (data != value)
                    Old = data;
                data = value;
               
               
                 
            }
        }


        public object Old { get; private set; }


        public object Origin { get; private set; }
        public Property Property { get; private set; }

        public void UpdateToNew()
        {
            this.Old = this.data;
            this.Origin = this.data;
        }
    }
}
