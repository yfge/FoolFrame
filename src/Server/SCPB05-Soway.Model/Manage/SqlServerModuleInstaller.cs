using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model.Manage
{
    public class SqlServerModuleInstaller : IDbModuleInstaller
    {
        public ICurrentContextFactory ConFac { get; private set; }

        //public SqlCon SqlCon;
        public SqlServerModuleInstaller(Context.ICurrentContextFactory conFac)
        {

            this.ConFac = conFac;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source">模型</param>
        /// <param name="ModelSqlCon">存储模型的数据库</param>
        /// <param name="DataBaseSqlCon">要创建的数据库的</param>
        public void InstallModules(IModuleSource Source,
            SqlCon ModelSqlCon, SqlCon DataBaseSqlCon)
        {
            ConnectionType ConnectionType = ConnectionType.AppSys;
            if (ModelSqlCon.ToString() == DataBaseSqlCon.ToString())
                ConnectionType = ConnectionType.AppSys;
            else
                ConnectionType = ConnectionType.Current;

            //System.Diagnostics.Trace.WriteLine("MODEL:"+ModelSqlCon.ToString());
            //System.Diagnostics.Trace.WriteLine("CON:"+ModelSqlCon.ToString());
            var modelDb = new SqlServer.dbContext(ModelSqlCon, this.ConFac);
            var DataDb = new SqlServer.dbContext(DataBaseSqlCon, this.ConFac);
            var modelMode = Global.ModeMode;
            var moduleMode = Global.ModuleMode;
            var viewMode = Global.ViewMode;
            var relationMode = Global.RelationModel;


            List<IObjectProxy> insertItems = new List<IObjectProxy>();
            ModelSqlServerFactory sqlFac = new ModelSqlServerFactory(Source);
            var viewFac = new Soway.Model.View.AutoViewFactory(ModelSqlCon, this.ConFac);
            Dictionary<object, System.Data.IDbCommand> commands = new Dictionary<object, System.Data.IDbCommand>();

            Dictionary<object, System.Data.IDbCommand> relationsCommands = new Dictionary<object, System.Data.IDbCommand>();
            var helper = new ModelHelper(this.ConFac);
            var modules = Source.GetModules();
            CreateDataBase(DataBaseSqlCon);

            foreach (var module in modules.Distinct())
            {
                module.SqlCon = DataBaseSqlCon;
                IObjectProxy moduleproxy = new ObjectProxy(moduleMode, this.ConFac);
                helper.SetProxy(ref moduleproxy, module);

                if (modelDb.IsExits(moduleproxy) == true)
                {

                }
                else
                {
                    insertItems.Add(moduleproxy);
                }

                foreach (var model in Source.GetModels(module))
                {


                    IObjectProxy proxy = new ObjectProxy(modelMode, this.ConFac, LoadType.Complete);
                    helper.SetProxy(ref proxy, model);
                    if (modelDb.IsExits(proxy) == false)
                        insertItems.Add(proxy);
                    if (model.ModelType != ModelType.Enum)
                    {


                        var command = sqlFac.GerateCreateSql(model);
                        if (commands.ContainsKey(model))
                            commands[model] = command;
                        else
                            commands.Add(model, command
                             );
                        foreach (var relation in model.Relations)
                        {
                            if (relationsCommands.ContainsKey(relation) == false)

                                relationsCommands.Add(relation, sqlFac.GetRelationSql(relation));
                        }
                        var itemView = viewFac.CreateDefaultItemView(model);
                        itemView.ConnectionType = ConnectionType;
                        IObjectProxy itemViewProxy = new ObjectProxy(viewMode, this.ConFac);
                        var view = viewFac.CreateDefaultListView(model);
                        IObjectProxy viewProxy = new ObjectProxy(viewMode, this.ConFac);
                        view.ConnectionType = ConnectionType;
                        helper.SetProxy(ref itemViewProxy, itemView);
                        insertItems.Add(itemViewProxy);
                        helper.SetProxy(ref viewProxy, view);
                        insertItems.Add(viewProxy);

                    }


                }


            }



            //创建表
            foreach (var command in commands)

            {
                DataDb
                .Excute(command.Value as System.Data.SqlClient.SqlCommand);
            }

            foreach (var command in relationsCommands)

            {
                DataDb
                .Excute(command.Value as System.Data.SqlClient.SqlCommand);
            }
            //插入数据
            Soway.Data.Graphic.Graphic<IObjectProxy> proxyMaps = new Data.Graphic.Graphic<IObjectProxy>();
            foreach (var item in insertItems)
            {
                proxyMaps.Add(item);
                foreach (var ob in item.Model.Properties)
                {
                    if (item[ob] is IObjectProxy)
                    {
                        proxyMaps.AddEdge(item[ob] as IObjectProxy, item);
                    }
                }
            }

            while (proxyMaps.Nodes.Count > 0)
            {


                var ob = proxyMaps.GetTopNode();
                if (ob == null)
                    ob = proxyMaps.Nodes[0].Data;
                proxyMaps.Remove(ob);
                if (modelDb.IsExits(ob) == false)
                {
                    modelDb.Create(ob);

                }
                else
                {

                }
            }
        }
        public void UpdateModules(IModuleSource Source,
       SqlCon ModelSqlCon, SqlCon DataBaseSqlCon)
        {
            ConnectionType ConnectionType = ConnectionType.AppSys;
            if (ModelSqlCon.ToString() == DataBaseSqlCon.ToString())
                ConnectionType = ConnectionType.AppSys;
            else
                ConnectionType = ConnectionType.Current;
            var modelDb = new SqlServer.dbContext(ModelSqlCon, this.ConFac);
            var DataDb = new SqlServer.dbContext(DataBaseSqlCon, this.ConFac);
            var modelMode = Global.ModeMode;
            var moduleMode = Global.ModuleMode;
            var viewMode = Global.ViewMode;
            var relationMode = Global.RelationModel;


            List<IObjectProxy> insertItems = new List<IObjectProxy>();
            List<IObjectProxy> updateItems = new List<IObjectProxy>();
            ModelSqlServerFactory sqlFac = new ModelSqlServerFactory(Source);
            var viewFac = new Soway.Model.View.AutoViewFactory(ModelSqlCon, this.ConFac);
            Dictionary<object, System.Data.IDbCommand> commands = new Dictionary<object, System.Data.IDbCommand>();

            Dictionary<object, System.Data.IDbCommand> relationsCommands = new Dictionary<object, System.Data.IDbCommand>();
            var helper = new ModelHelper(this.ConFac);
            var modules = Source.GetModules();
            CreateDataBase(DataBaseSqlCon);

            foreach (var module in modules.Distinct())
            {
                module.SqlCon = DataBaseSqlCon;
                IObjectProxy moduleproxy = new ObjectProxy(moduleMode, this.ConFac);
                helper.SetProxy(ref moduleproxy, module);

                if (modelDb.IsExits(moduleproxy) == true)
                {

                    updateItems.Add(moduleproxy);
                }
                else
                {
                    insertItems.Add(moduleproxy);
                }

                foreach (var model in Source.GetModels(module))
                {


                    IObjectProxy proxy = new ObjectProxy(modelMode, this.ConFac, LoadType.Complete);
                    helper.SetProxy(ref proxy, model);
                    if (modelDb.IsExits(proxy) == false)
                        insertItems.Add(proxy);

                    if (model.ModelType != ModelType.Enum)
                    {


                        var command = sqlFac.GerateCreateSql(model);
                        if (commands.ContainsKey(model))
                            commands[model] = command;
                        else
                            commands.Add(model, command
                             );
                        foreach (var relation in model.Relations)
                        {
                            if (relationsCommands.ContainsKey(relation) == false)

                                relationsCommands.Add(relation, sqlFac.GetRelationSql(relation));
                        }
                        var itemView = viewFac.CreateDefaultItemView(model);
                        itemView.ConnectionType = ConnectionType;
                        IObjectProxy itemViewProxy = new ObjectProxy(viewMode, this.ConFac);
                        var view = viewFac.CreateDefaultListView(model);
                        IObjectProxy viewProxy = new ObjectProxy(viewMode, this.ConFac);
                        view.ConnectionType = ConnectionType;
                        helper.SetProxy(ref itemViewProxy, itemView);
                        insertItems.Add(itemViewProxy);
                        helper.SetProxy(ref viewProxy, view);
                        insertItems.Add(viewProxy);

                    }


                }


            }



            //创建表
            foreach (var command in commands)

            {
                DataDb
                .Excute(command.Value as System.Data.SqlClient.SqlCommand);
            }

            foreach (var command in relationsCommands)

            {
                DataDb
                .Excute(command.Value as System.Data.SqlClient.SqlCommand);
            }
            //插入数据
            Soway.Data.Graphic.Graphic<IObjectProxy> proxyMaps = new Data.Graphic.Graphic<IObjectProxy>();
            foreach (var item in insertItems)
            {
                proxyMaps.Add(item);
                foreach (var ob in item.Model.Properties)
                {
                    if (item[ob] is IObjectProxy)
                    {
                        proxyMaps.AddEdge(item[ob] as IObjectProxy, item);
                    }
                }
            }

            while (proxyMaps.Nodes.Count > 0)
            {


                var ob = proxyMaps.GetTopNode();
                if (ob == null)
                    ob = proxyMaps.Nodes[0].Data;
                proxyMaps.Remove(ob);
                if (modelDb.IsExits(ob) == false)
                {
                    modelDb.Create(ob);

                }
                else
                {

                }
            }
        }
        private static void CreateDataBase(SqlCon DataBaseSqlCon)
        {

            var createCommand = new System.Data.SqlClient.SqlCommand();
            createCommand.CommandText = String.Format(
                @"if not exists(select * from master..sysdatabases  where name='{0}')
                begin
                    create database {0}
                end
                ", DataBaseSqlCon.InitialCatalog);
            using (var con = new System.Data.SqlClient.SqlConnection(
                new System.Data.SqlClient.SqlConnectionStringBuilder()
                {
                    DataSource = DataBaseSqlCon.DataSource,
                    UserID = DataBaseSqlCon.UserID,
                    Password = DataBaseSqlCon.Password
                }.ToString()))
            {
                con.Open();
                createCommand.Connection = con;
                createCommand.ExecuteNonQuery();


            }
        }


        public void InstallModel(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon)
        {


            ModelSqlServerFactory sqlFac = new ModelSqlServerFactory(null);

            IObjectProxy proxy = new ObjectProxy(Global.ModeMode, this.ConFac, LoadType.Complete);
            var helper = new ModelHelper(this.ConFac);
            var modelDb = new SqlServer.dbContext(modelSqlCon, this.ConFac);

            model.SqlCon = dataSqlCon;
            if (model.Module != null)
                model.Module.SqlCon = dataSqlCon;
            helper.SetProxy(ref proxy, model);
            if (modelDb.IsExits(proxy) == false)
            {
                modelDb.Create(proxy);
            }
            else
            {
                modelDb.Save(proxy);
            }

            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(dataSqlCon.ToString()))
            {
                con.Open();

                var command = sqlFac.GerateCreateSql(model);
                command.Connection = con;
                command.ExecuteNonQuery();
                foreach (var relation in model.Relations)
                {
                    var relationcommand = sqlFac.GetRelationSql(relation, model);
                    if (relationcommand != null)
                    {
                        relationcommand.Connection = con;
                        relationcommand.ExecuteNonQuery();
                    }

                }
            }
        }

        public void UpdateModel(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon)
        {

            ModelSqlServerFactory sqlFac = new ModelSqlServerFactory(null);

            IObjectProxy proxy = new ObjectProxy(Global.ModeMode, this.ConFac, LoadType.Complete);
            var helper = new ModelHelper(this.ConFac);
            var modelDb = new SqlServer.dbContext(modelSqlCon, this.ConFac);

            model.SqlCon = dataSqlCon;
            if (model.Module != null)
                model.Module.SqlCon = dataSqlCon;
            helper.SetProxy(ref proxy, model);
            if (modelDb.IsExits(proxy) == false)
            {
                modelDb.Create(proxy);
            }
            else
            {
                modelDb.Save(proxy);
            }

            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(dataSqlCon.ToString()))
            {
                con.Open();

                var command = sqlFac.GenerateUpdate(model);
                command.Connection = con;
                command.ExecuteNonQuery();
                foreach (var relation in model.Relations)
                {
                    var relationcommand = sqlFac.GetRelationSql(relation, model);
                    if (relationcommand != null)
                    {
                        relationcommand.Connection = con;
                        relationcommand.ExecuteNonQuery();
                    }

                }
            }

        }

        public void DeleteMode(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon)
        {
            throw new NotImplementedException();
        }

        public void CreateSysDataBase(SqlCon sysCon)
        {
            new Soway.Model.Manage.SqlServerModuleInstaller(this.ConFac).InstallModules(
                new Soway.Model.AssemblyModuleSource(Global.fac), sysCon, sysCon);
        }

        public bool IsModelInstalled(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon)
        {

            var db = new SqlServer.dbContext(modelSqlCon, this.ConFac);
            IObjectProxy obj = new ObjectProxy(typeof(Model), this.ConFac);
            new ModelHelper(this.ConFac).SetProxy(ref obj, model);
            if (db.IsExits(obj))
            {
                var chkCommand = new System.Data.SqlClient.SqlCommand();
                chkCommand.CommandText = string.Format("SELECT * FROM SYS.TABLES WHERE NAME='{0}'", model.DataTableName.Replace("[", "").Replace("]", ""));
                using (var con = new System.Data.SqlClient.SqlConnection(
                    new System.Data.SqlClient.SqlConnectionStringBuilder()
                    {
                        DataSource = dataSqlCon.DataSource,
                        UserID = dataSqlCon.UserID,
                        Password = dataSqlCon.Password
                    }.ToString()))
                {
                    con.Open();
                    chkCommand.Connection = con;
                    if (chkCommand.ExecuteScalar() != null)
                        return true;


                }
            }
            return false;
        }

        public bool IsModuleInstalled(Module module, SqlCon modelSqlCon, SqlCon dataSqlCon)
        {
            return false;
        }

        public bool IsModelRegistered(Model model, SqlCon modelSqlCon)
        {
            var db = new SqlServer.dbContext(modelSqlCon, this.ConFac);
            IObjectProxy obj = new ObjectProxy(typeof(Model), this.ConFac);
            new ModelHelper(this.ConFac).SetProxy(ref obj, model);
            if (db.IsExits(obj))
            {
                return true;
            }
            return false;
        }

        public bool IsModelInstalled(Model model, SqlCon dataSqlCon)
        {
            var chkCommand = new System.Data.SqlClient.SqlCommand();
            chkCommand.CommandText = string.Format("SELECT count(*) FROM SYS.TABLES WHERE NAME='{0}'", model.DataTableName.Replace("[", "").Replace("]", ""));
            using (var con = new System.Data.SqlClient.SqlConnection(
                new System.Data.SqlClient.SqlConnectionStringBuilder()
                {
                    DataSource = dataSqlCon.DataSource,
                    UserID = dataSqlCon.UserID,
                    Password = dataSqlCon.Password
                }.ToString()))
            {
                con.Open();
                chkCommand.Connection = con;
                if (chkCommand.ExecuteScalar() != null)
                    return true;


            }
            return false;
        }
    }
}