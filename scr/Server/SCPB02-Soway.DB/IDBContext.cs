using System;
using Soway.Data;
namespace Soway.DB
{
    public interface IDBContext
    {
        void Attatch(System.Data.SqlClient.SqlCommand command);
        void Attatch(string sql);
        void Attatch<T>(T ob, OperationType SubmitType);
        string ConnectionString { get; set; }
        void Create<P,T>(P ob)
            where P:Soway.Data.ObjectWithSubItem<T>
            where T : IItemInterface<P>,new();
        void Create<T>(T ob);
//   void CreateComplexData<Parent, Item>(Parent ob) where Parent : System.Collections.Generic.ICollection<Item>;
     //   void Delelte<Parent, Item>(Parent ob) where Parent : System.Collections.Generic.ICollection<Item>;
        void Delete<T>(T ob);
        void Delete<P,T>(P ob)
            where P : Soway.Data.ObjectWithSubItem<T>
            where T : IItemInterface<P>, new();
      
        void Save<T>(T ob);
        void Save<P, T>(P ob)
            where P : Soway.Data.ObjectWithSubItem<T>
            where T : IItemInterface<P>, new();
        System.Collections.Generic.List<I> GenerualQuery<I, T>(string str) where T : class, I, new();
        System.Collections.Generic.List<T> GenerualQuery<T>(string str) where T : class, new();
        System.Collections.Generic.List<I> Get<T, I>() where T : class, I, new();
        System.Collections.Generic.List<I> Get<T, I>(System.Data.SqlClient.SqlCommand selectCommand) where T : class, I, new();
        System.Collections.Generic.List<I> Get<T, I>(string SqlScript) where T : class, I, new();
        System.Collections.Generic.List<T> Get<T>() where T : class, new();
        System.Collections.Generic.List<T> Get<T>(System.Data.SqlClient.SqlCommand selectCommand) where T : class, new();
        System.Collections.Generic.List<T> Get<T>(string SqlScript) where T : class, new();
          I GetDetail<I, T>(object key) where T : class, I, new();
        T GetDetail<T>(object key) where T : class, new();
        object GetExist<T>(T ob);
        System.Collections.Generic.List<T> GetItems<T>(object ob, string ParentProperty = null);
       
        bool SubmitChanges();
        string TablePreStr { get; set; }

        String GetSerialNo(String Pre, int Len, String DataFormatStr,String Source);
        String GetSerialNo(String Pre, int Len, String DataFormatStr);
    }
}
