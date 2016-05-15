using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;
using Soway.Model.Manage;

namespace Soway.Model.SqlServer
{
    public class SqlContext <T>
    {
        public SqlContext(SqlCon con,Context.ICurrentContextFactory conFac){
             this.Con = con;
            this.ConFac = conFac;
            this.Fac = new Manage.SqlServerModelFactory(con,this.ConFac);
            this.Model = Fac.GetModel(typeof(T));
            }

        public T GetDetail(object id,Soway.Model.SqlCon con=null)
        {
            var objectProxy = new Soway.Model.SqlServer.dbContext(GetCon(con),this.ConFac).GetDetail(Model, id, true);
            if (objectProxy.IsLoad == LoadType.NoObj)
                return default(T);
   
            return (T)new ModelHelper(this.ConFac).GetFromProxy(objectProxy);

        }
        public T GetTheOne(object id ,Soway.Model.SqlCon con = null)
        {
            var objectProxy = new Soway.Model.SqlServer.dbContext(GetCon(con),this.ConFac).GetDetail(Model, id, false);
            if (objectProxy.IsLoad == LoadType.NoObj)
                return default(T);
            return (T)new ModelHelper(this.ConFac).GetFromProxy(objectProxy);
        }

        public void  Create(T obj,  Soway.Model.SqlCon con = null)
        {
            


            IObjectProxy proxy = new ObjectProxy(this.Model,this.ConFac);
            new  ModelHelper(this.ConFac).SetProxy(ref proxy, obj);
            new Soway.Model.SqlServer.dbContext(GetCon(con),this.ConFac).Create(proxy);
        

        }

        private SqlCon GetCon(SqlCon con=null)
        {
      
            if (this.Model.ConnectionType == ConnectionType.Default)
            {
                if (this.Model.SqlCon != null)
                    return this.Model.SqlCon;
                if (this.Model.Module.SqlCon != null)
                    return this.Model.Module.SqlCon;
            }
            else if(this.Model.ConnectionType == ConnectionType.Current)
            {
                return this.Con;
            }

            if (con != null)
                return con;
            return this.Con;
        }

        public SqlCon Con { get; private set; }
        public SqlServerModelFactory Fac { get; private set; }
        public Model Model { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }
    }
}
