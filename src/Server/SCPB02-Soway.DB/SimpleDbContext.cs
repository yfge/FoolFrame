using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.DB
{
    public class SimpleDbContext<T>:SimpleDbInterfaceContext<T,T> where T:class ,new()
    {

        public SimpleDbContext(String con) : base(con) { }
    }
}
