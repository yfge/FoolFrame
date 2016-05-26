using Soway.Data;
using System;
using System.Collections;
using System.Text;

namespace Soway.Data
{
    public interface IInterfaceMultiDbContext <I,T>: IInerfaceDBContext <I,T> 
        where T:class,I,new()
        where I:ICollection
        
    {


        
    }
}
