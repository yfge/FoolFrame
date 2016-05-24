using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model.SqlServer
{
    class DynamicContext
    {
        private ICurrentContextFactory Fac;
        private string ConStr;
        private static List<Model> models = new List<Model>();
        private SqlCon SqlCon;
        private static Model getModel(Type type)
        {
            var model = models.FirstOrDefault(p => p.ClassName == type.Name);
            if(model == null)
            {
                var itemsToAdd = new AssemblyModuleSource(new AssemblyModelFactory(type)).GetModels();
                foreach(var item in itemsToAdd)
                {
                    if(models.Count(p=>p.ClassName == item.ClassName) == 0)
                    {
                        models.Add(item);
                    }

                    model = models.FirstOrDefault(p => p.ClassName == type.Name);
                }
            }
            return model;
        }
        public DynamicContext(String connectiongString,Soway.Model.Context.ICurrentContextFactory fac)
        {
            this.Fac = fac;
            this.ConStr = connectiongString;
            var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(this.ConStr);

            this.SqlCon = new SqlCon()
            {
                DataSource = builder.DataSource,
                InitialCatalog = builder.InitialCatalog,
                IntegratedSecurity = builder.IntegratedSecurity,
                IsLocal = false,
                Password = builder.Password,
                UserID = builder.UserID
            };
           

        }
        public List<dynamic> Get(Type refType,String sqlscript)
        {
            return Get(refType, new System.Data.SqlClient.SqlCommand()
            {
                CommandText = sqlscript
            });
        }

        public List<dynamic> Get(Type refType,System.Data.SqlClient.SqlCommand command)
        {
            var model = getModel(refType);
            return new Soway.Model.SqlServer.dbContext(this.SqlCon, this.Fac).GetBySqlCommand(
          model, command);
        }
        public bool Create(dynamic ob)
        {

            new Soway.Model.SqlServer.dbContext(this.SqlCon, Fac).Create(ob);
            return true;
        }
        public bool Save(dynamic ob)
        {
            new Soway.Model.SqlServer.dbContext(this.SqlCon, Fac).Save(ob);
            return true;
        }
        public bool Delete(dynamic ob)
        {
            new Soway.Model.SqlServer.dbContext(this.SqlCon, Fac).Delete(ob);
            return true;

        }


        public dynamic GetById(Type refType,object id)
        {
            var model = getModel(refType);
            return new Soway.Model.SqlServer.dbContext(this.SqlCon, this.Fac).GetDetail(
          model, id);
        }
    }
}
