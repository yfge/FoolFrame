using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data
{
    public interface IDBContext <T> :IInerfaceDBContext<T,T> where T:class ,new()
    {
    }



}
