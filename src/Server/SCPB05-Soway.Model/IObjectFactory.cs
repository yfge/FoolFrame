using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    class IObjectFactory
    {
        internal static  object GetPropertyValue(ObjectProxyClass ob,String exp)
        {
            var strs = exp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var first = strs.First();


            string nextStr = "";
            for (int i = 1; i < strs.Length; i++)
            {
                if (strs[i] != "#")
                    nextStr += ".";
                nextStr += strs[i];

            }

            if (first.Trim().ToUpper() == "#" && ob.Owner != null)
            {

                 
                
                return GetPropertyValue(ob.Owner as ObjectProxyClass, nextStr); 
            }
            bool GetOld = false ;
            if (first[0] == '^')
            {
                GetOld = true;
                first = first.Substring(1);
            }
            var propery = ob.Model.Properties.FirstOrDefault(p => p.Name == first || p.PropertyName == first);
            if (propery != null)
            {
                if (strs.Length == 1)
                {

                    if (GetOld == false)
                        return ob[propery];
                    else
                        return ob.KeyPairs[propery].Old;
                }
                else
                {
                   
                    var proxy = ob[propery] as ObjectProxyClass;
                    if (GetOld == true)
                    {
                        proxy = ob.KeyPairs[propery].Old as ObjectProxyClass;
                    }
                    while (nextStr.StartsWith("."))
                        nextStr = nextStr.Substring(1);
                    if(proxy !=null)
                        return proxy[nextStr];
                }
            }
            return null;
        }
        internal static void SetPropertyValue(IObjectProxy ob, string exp, object value)
        {
            var strs = exp.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var first = strs.First();
            if (first.Trim().ToLower() == "#" && ob.Owner != null)
                SetPropertyValue(ob, exp.Substring(1), value);
            var propery = ob.Model.Properties.FirstOrDefault(p => p.Name == first || p.PropertyName == first);
            if (propery != null)
            {
                if (strs.Length == 1)

                    ob[propery] = value;
                else
                {

                    var proxy = ob[propery] as ObjectProxy;
                    proxy[exp.Substring(first.Length)] = value;
                }
            }


        }


        internal static object GetPropertyValue(ObjectProxyClass ob, Property property)
        {
            if (ob.Model.Properties.Contains(property))
                return ob.KeyPairs[property].Data;
            else
                return GetPropertyValue(ob.Owner as ObjectProxyClass, property);
        }
        internal static void SetPropertyValue(ObjectProxyClass ob, Property property, object value)
        {
            if (ob.Model.Properties.Contains(property))
            {
              

                ob.KeyPairs[property].Data = value;

                if(value is IObjectProxy)
                {
                    (ob.KeyPairs[property].Data as IObjectProxy).Owner = ob;
                }

               
            }
            else
                SetPropertyValue(ob.Owner as ObjectProxyClass, property, value);
        }


    }
}
