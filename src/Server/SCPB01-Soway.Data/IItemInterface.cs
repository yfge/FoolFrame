using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{
    
    　public 
         interface IItemInterface < 
    out
        T>
    {
        T Parent { get;  }

 　 void SetParent(object  ob);

//        IItemInterface(T ob);
    }
}
