using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    class ObjectInvokeMemberBinder : System.Dynamic.InvokeMemberBinder
    {



        public ObjectInvokeMemberBinder(String Name)
            : base(
                Name, true, new System.Dynamic.CallInfo(0, new string[] { })) { }

        public override System.Dynamic.DynamicMetaObject FallbackInvoke(System.Dynamic.DynamicMetaObject target, System.Dynamic.DynamicMetaObject[] args, System.Dynamic.DynamicMetaObject errorSuggestion)
        {
            return null;
        }

        public override System.Dynamic.DynamicMetaObject FallbackInvokeMember(System.Dynamic.DynamicMetaObject target, System.Dynamic.DynamicMetaObject[] args, System.Dynamic.DynamicMetaObject errorSuggestion)
        {
            return null;
        }
    }
}
