using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Soway.Data
{
    public interface IMultiDbContext<T> : IInterfaceMultiDbContext<T, T> where T:class,ICollection,new()
    {
    }
}
