using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.DB
{
    public class SimpleDbInterfaceContext<I,T> :Soway.Data.IInerfaceDBContext<I,T> 
        where T:class,I,new ()
    {

        private string ConStr{get;set;}
        public SimpleDbInterfaceContext(String conStr) { ConStr = conStr; }
        public List<I> Get()
        {
            return new DBContext(ConStr).Get<T, I>();
        }

        public void Save(I ob)
        {
           
            T ob1 = ob as T;
            new DBContext(ConStr).Save<T>(ob1);
        }

        public void Delete(I ob)
        {

            T ob1 = ob as T;
            new DBContext(ConStr).Delete<T>(ob1);
        }

        public void Create(I ob)
        {
            new DBContext(ConStr).Create<T>(ob as T);
        }

        public I GetDetail(object key)
        {
            return new DBContext(ConStr).GetDetail<I, T>(key);
        }
    }
}
