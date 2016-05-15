using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Context
{
    public interface ICurrentContextFactory
    {
         CurrentContext GetCurrentContext();
        object GetValue(String key);
         DateTime GetDateTime();
    }
}
