using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model.SqlServer
{
    public class DynamicContext
    {
        private ICurrentContextFactory Fac;
        private string ConStr;
        private static List<Model> models = new List<Model>();
        private SqlCon SqlCon;
        private static Model getModel(Type type)
        {
            var model = models.FirstOrDefault(p => p.ClassName == type.FullName);
            if(model == null)
            {
                var itemsToAdd = new AssemblyModuleSource(new AssemblyModelFactory(type)).GetModels();
                foreach (var item in itemsToAdd)
                {
                    if (models.Count(p => p.ClassName == item.ClassName) == 0)
                    {
                        models.Add(item);
                    }
                }
                model = models.FirstOrDefault(p => p.ClassName == type.FullName);

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
            return new Soway.Model.SqlServer.dbContext(GetTypeSqlCon(refType), this.Fac).GetBySqlCommand(
          model, command);
        }
        public bool Create(dynamic ob)
        {

            new Soway.Model.SqlServer.dbContext(ob.Con, Fac).Create(ob);
            return true;
        }
        public bool Save(dynamic ob)
        {
            new Soway.Model.SqlServer.dbContext(ob.Con, Fac).Save(ob);
            return true;
        }
        public bool Delete(dynamic ob)
        {
            new Soway.Model.SqlServer.dbContext(ob.Con, Fac).Delete(ob);
            return true;

        }


        public dynamic GetById(Type refType,object id)
        {
            var model = getModel(refType);
            return new Soway.Model.SqlServer.dbContext(GetTypeSqlCon(refType), this.Fac).GetDetail(
          model, id);
        }

        public dynamic InitNew(Type refType)
        {
            var model = getModel(refType);
            return new SqlDataProxy(model, this.Fac, LoadType.Complete, GetTypeSqlCon(refType));
        }

        public dynamic IsExists(dynamic ob)
        {
            return new Soway.Model.SqlServer.dbContext(ob.Con, Fac).IsExits(ob);
        }



        private static System.Collections.Concurrent.ConcurrentDictionary<
            String, System.Collections.Concurrent.ConcurrentDictionary<Type, SqlCon>>
            conStrCaches = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<Type, SqlCon>>();
        public SqlCon GetTypeSqlCon(Type refType)
        {
            if (conStrCaches.ContainsKey(this.SqlCon.ToString()) == false)
                while (false == conStrCaches.TryAdd(this.SqlCon.ToString(), new System.Collections.Concurrent.ConcurrentDictionary<Type, SqlCon>())) ;
            var sqlCons = conStrCaches[this.SqlCon.ToString()];
            if (!sqlCons.ContainsKey(refType))
            {
                var db = new Soway.Model.SqlServer.dbContext(this.SqlCon, this.Fac);
                var manager = new Soway.Model.Manage.SqlServerModuleInstaller(this.Fac);
                var sysmodel = getModel(typeof(Soway.Model.Model));
                SqlCon con = null;
                if (manager.IsModelInstalled(sysmodel, this.SqlCon))
                {
                    dynamic model = new Soway.Model.SqlServer.dbContext(this.SqlCon, this.Fac).GetDetail(sysmodel, refType.FullName);
                    if (model != null)
                    {
                        if (model.SqlCon != null)
                        {
                            con = new SqlCon()
                            {
                                DataSource = model.SqlCon.DataSource,
                                InitialCatalog = model.SqlCon.InitialCatalog,
                                UserID = model.SqlCon.UserID,
                                Password = model.SqlCon.Password
                            };

                        }
                        else if (model.Module != null && model.Module.SqlCon != null)
                        {
                            con = new SqlCon()
                            {
                                DataSource = model.Module.SqlCon.DataSource,
                                InitialCatalog = model.Module.SqlCon.InitialCatalog,
                                UserID = model.Module.SqlCon.UserID,
                                Password = model.Module.SqlCon.Password
                            };
                        }
                    }
                }
                else
                    con = this.SqlCon;
                while (false == sqlCons.TryAdd(refType, con)) ;
            }
            return sqlCons[refType];
        }


        
    }
}
