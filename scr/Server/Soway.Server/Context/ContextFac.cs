using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soway.Model.Context;

namespace Soway.Service.Context
{
    class ContextFac : Soway.Model.Context.ICurrentContextFactory
    {
        public CurrentContext GetCurrentContext()
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime()
        {
            throw new NotImplementedException();
        }

        public object GetValue(string key)
        {
            throw new NotImplementedException();
        }
    }
}
