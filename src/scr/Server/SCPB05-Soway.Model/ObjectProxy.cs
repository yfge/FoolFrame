using Soway.Model.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model
{
    public class ObjectProxy : ObjectProxyClass
    {


        public bool SaveInDB { get; set; }
     

        public override   object this[String exp]
        {
            get
            {
                return IObjectFactory.GetPropertyValue(this, exp);
            }
            set
            {
                IObjectFactory.SetPropertyValue(this, exp, value);

            }
        }
        public ObjectProxy(Model model,Context.ICurrentContextFactory conFac, LoadType isLoad= LoadType.Null) : base(model,conFac, isLoad) {
 
        }
        public override object this[Property index]
        {
            get
            {
                return IObjectFactory.GetPropertyValue(this, index);

            }
            set
            {

                var old = this.KeyPairs[index].Data;

                IObjectFactory.SetPropertyValue(this, index, value);

                if (value != old  )
                {
                    Notify(index);
                    var triggers = index.Triggers.Where(p => p.PropertyTriggerType == PropertyTriggerType.Set);
                    if (triggers.Count() > 0)
                    {
                        var method = new ModelMethodContext(this.Con,this.ConFac);
                        foreach (var trigger in triggers)
                        {
                            method.ExcuteOperation(this, trigger);
                        }
                    }


                }

                ///序列号的情况 
                if (
                    (index.PropertyType == Data.PropertyType.SerialNo  
                    &&value != null
                    && string.IsNullOrEmpty(value.ToString())==false
                    )||(
                    index.PropertyType == Data.PropertyType.IdentifyId //自增ID 
                    ))
                {
                    this.KeyPairs[index].UpdateToNew();

                }

            
            }
        }
    }
}