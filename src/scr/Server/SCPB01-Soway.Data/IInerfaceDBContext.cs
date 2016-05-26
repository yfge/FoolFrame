using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data
{


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="I">接口</typeparam>
    public interface IInerfaceDBContext <I,T> where T:class,I,new()
      
    {




        List<I> Get();
        void Save(I ob);
        void Delete(I ob);
        void Create(I ob);
        I GetDetail(object key);
         
    }
}
