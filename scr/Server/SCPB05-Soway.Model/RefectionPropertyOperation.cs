using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public class ReflectionPropertyOperation :PropertyOperation
    {
        public override object Get(object ob)
        {

            var result = PropertyInfo.GetValue(ob, new object[] { });
            if (result == null)
            {
                if (this.Property.IsArray)
                {
                    result = PropertyInfo.PropertyType.GetConstructor(new Type[]{}).Invoke(new object[]{});

                    Set(ob, result);
                }
                 
            }
            return result;
        }
        public override void Set(object ob, object value)
        {
            var objValue = value;
            if (PropertyInfo.PropertyType == typeof(Guid))
            {
                var guid = Guid.Empty;
                if(Guid.TryParse((value ?? "").ToString(), out guid) ==false )
                {
                     
                }
                objValue = guid;
            }

            if(PropertyInfo.CanWrite )
                PropertyInfo.SetValue(ob, objValue, new object[] { });
        }
        internal ReflectionPropertyOperation(Property Property,Model Model)
        {
            this.Property = Property;

            this.PropertyInfo = System.Reflection.Assembly.Load(Model.Module.Assembly).GetType(Model.ClassName).GetProperty(Property.PropertyName);
        }

        private System.Reflection.PropertyInfo PropertyInfo { get; set; }
    }
}
