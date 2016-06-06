using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model.Manage
{
    public class SqlServerModelFactory : IModelFactory
    {
        public SqlCon Con { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }

        public Model GetModel(Type type)
        {

            String sql = String.Format(@"
            SELECT   [MODEL_ID]
          FROM [SW_SYS_MODEL]
          where[MODEL_CLASS] = '{0}' ", type.FullName);

            var dbtable = new Soway.DB.DBContext(this.Con.ToString()).GetDataTable(sql);
            if (dbtable.Rows.Count == 0)
                return null;
            var obj = new Soway.Model.SqlServer.DynamicContext(
                this.Con.ToString(), this.ConFac).GetById(
                typeof(Model), dbtable.Rows[0][0]);
            return (Model)new Soway.Model.ModelHelper(this.ConFac).GetFromProxy(obj);
            //return new Soway.Model.SqlServer.ObjectContext<Model>(this.Con,this.ConFac).GetDetail(dbtable.Rows[0][0]);

        }

        public List<Model> GetModels(Module Module)
        {
            throw new NotImplementedException();
        }

        public List<Module> GetModules()
        {
            throw new NotImplementedException();
        }

        public List<Module> GetRefrenceModules(Module Module)
        {
            throw new NotImplementedException();
        }

        public void InstallModel(Module Module)
        {
            throw new NotImplementedException();
        }

        public SqlServerModelFactory(Soway.Model.SqlCon con,Context.ICurrentContextFactory conFac)
        {
            this.Con = con;
            this.ConFac = conFac;
        }
    }
}
