using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model.SqlServer
{
    public class ObjectContext<T>
    {
        private Model Model { get; set; }
        private Soway.Model.SqlCon SqlCon { get; set; }
        public ICurrentContextFactory ConFac { get; private set; }

        public ObjectContext(Soway.Model.SqlCon sqlCon,Context.ICurrentContextFactory conFac)
        {
            Model = new Soway.Model.AssemblyModelFactory(typeof(T).Assembly).GetModel(typeof(T));
            this.ConFac = conFac;
            this.SqlCon = sqlCon;

        }
        public T GetDetail(object id)
        {

            //var objectProxy = new ObjectProxy(Model);
            var dbContext = new dbContext(SqlCon,this.ConFac);
            var objectProxy =  dbContext.GetDetail(Model, id, true);
            if (objectProxy.IsLoad == LoadType.NoObj)
                return default(T);
            
            return (T)new ModelHelper(this.ConFac).GetFromProxy(objectProxy);
        
      
        }
        public T  Create(T ob)
        {
            IObjectProxy proxy = new ObjectProxy(Model,this.ConFac);
            new ModelHelper(this.ConFac).SetProxy(ref proxy, ob);
            new dbContext(SqlCon,this.ConFac).Create(proxy);
            return new ModelHelper(this.ConFac).GetFromProxy(proxy);
        }

        public void Save(T ob)
        {
            IObjectProxy proxy = new ObjectProxy(Model,this.ConFac);
            new ModelHelper(this.ConFac).SetProxy(ref proxy, ob);
            
            new dbContext(SqlCon,this.ConFac).Save(proxy);
        }

        public void Delete(T ob)
        {
            IObjectProxy proxy = new ObjectProxy(Model,this.ConFac);
            new ModelHelper(this.ConFac).SetProxy(ref proxy, ob);
            new dbContext(SqlCon,this.ConFac).Delete(proxy);
        }

    }
}
