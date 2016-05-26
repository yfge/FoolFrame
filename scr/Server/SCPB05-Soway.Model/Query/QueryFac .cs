using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;
using Soway.Query.Entity;

namespace Soway.Model.Query
{
    
   
    public class QueryFactory:Soway.Query.Entity.IQueryFactory
    {

        public QueryModel GetQueryModel(View.View view)
        {
            QueryModel result = new QueryModel();
            result.Table = new Soway.Query.Entity.IQueryTable()
            {
                DBName =view. Model.DataTableName,
                ShowName =view. Model.Name
            };
            result.Columns = new List<ModelQueryCol>();

            foreach (var viewItem in view.Items.Where(p => p.Property.IsArray == false))
            {

                var property = viewItem.Property;
                if (property.IsMultiMap == false)
                {
                    ModelQueryCol col = new ModelQueryCol();
                    // col.Property = property;
                    col.ShowName = viewItem.Name;
                    col.IsKey = property.IsCheck;
                    col.DBName = property.DBName;
                    col.FormatStr = property.Format;
                    col.IsIdentity = (property.IsCheck && string.IsNullOrEmpty(property.IXGroup));
                    col.Table = result.Table;
                    col.DataType = property.PropertyType;
                    col.ID = property.Name;
                    col.ModelId = (property.Model == null ? 0 : property.Model.ID).ToString();

                    if (property.PropertyType == PropertyType.Enum && property.Model != null)
                        col.States = property.Model.EnumValues;
                    result.Columns.Add(col);
                    if (cols.ContainsKey(col) == false)
                        cols.Add(col, property);
                }
                else
                    foreach (var dbmap in property.DBMaps)
                    {
                        ModelQueryCol col = new ModelQueryCol();
                        col.ShowName = viewItem.Name + "_" + property.Model.Properties.FirstOrDefault(p => p.PropertyName == dbmap.PropertyName).Name;
                        col.IsKey = false;// property.IsCheck;
                        col.DBName = dbmap.DBColName;
                        col.FormatStr = property.Format;
                        col.IsIdentity = false;
                        col.Table = result.Table;
                        var mapProperty = property.Model.Properties.FirstOrDefault(p => p.Name == dbmap.PropertyName || p.PropertyName == dbmap.PropertyName);
                        col.DataType = property.PropertyType;
                        result.Columns.Add(col);
                        if (cols.ContainsKey(col) == false)
                            cols.Add(col, property);

                    }

            }
            if (view.Model.AutoSysId == true)// || Model.IdProperty == null)
            {
                ModelQueryCol col = new ModelQueryCol();
                col.ShowName = "自动ID";
                col.IsKey = true;// property.IsCheck;
                col.DBName = "SysId";

                col.IsIdentity = true;// (property.IsCheck && string.IsNullOrEmpty(property.IXGroup));
                col.Table = result.Table;
                // var mapProperty = property.Model.Properties.FirstOrDefault(p => p.Name == dbmap.PropertyName);
                //col.Property = null;

                col.DataType = PropertyType.IdentifyId;
                if (cols.ContainsKey(col) == false)
                    cols.Add(col, null);

                result.Columns.Add(col);
            }
            return result;

        }
        public QueryModel GetQueryModel(Model Model)
        {
            QueryModel result = new QueryModel();
            result.Table = new Soway.Query.Entity.IQueryTable()
            {
                DBName = Model.DataTableName,
                ShowName = Model.Name
            };
            result.Columns = new List<ModelQueryCol>();
        
            foreach (var property in Model.Properties.Where(p => p.IsArray == false))
            {

                if (property.IsMultiMap == false)
                {
                    ModelQueryCol col = new ModelQueryCol();
                    // col.Property = property;
                    col.ShowName = property.Name;
                    col.IsKey = property.IsCheck;
                    col.DBName = property.DBName;
                    col.FormatStr = property.Format;
                    col.IsIdentity = (property.IsCheck && string.IsNullOrEmpty(property.IXGroup));
                    col.Table = result.Table;
                    col.DataType = property.PropertyType;
                    result.Columns.Add(col);
                    if (cols.ContainsKey(col) == false)
                        cols.Add(col, property);
                }
                else
                    foreach (var dbmap in property.DBMaps)
                    {
                        ModelQueryCol col =new ModelQueryCol();
                        col.ShowName = property.Name + "_" + property.Model.Properties.FirstOrDefault(p => p.PropertyName == dbmap.PropertyName).Name;
                        col.IsKey = false;// property.IsCheck;
                        col.DBName = dbmap.DBColName;
                        col.FormatStr = property.Format;
                        col.IsIdentity = false;
                        col.Table = result.Table;
                        var mapProperty = property.Model.Properties.FirstOrDefault(p => p.Name == dbmap.PropertyName || p.PropertyName == dbmap.PropertyName);
                        col.DataType = property.PropertyType;
                        result.Columns.Add(col);
                        if (cols.ContainsKey(col) == false)
                            cols.Add(col, property);

                    }

            }
            if (Model.AutoSysId == true)// || Model.IdProperty == null)
            {
                ModelQueryCol col = new ModelQueryCol();
                col.ShowName = "自动ID";
                col.IsKey = true;// property.IsCheck;
                col.DBName = "SysId";

                col.IsIdentity = true;// (property.IsCheck && string.IsNullOrEmpty(property.IXGroup));
                col.Table = result.Table;
                // var mapProperty = property.Model.Properties.FirstOrDefault(p => p.Name == dbmap.PropertyName);
                //col.Property = null;

                col.DataType = PropertyType.IdentifyId;
                if (cols.ContainsKey(col) == false)
                    cols.Add(col, null);

                result.Columns.Add(col);
            }
            return result;

        }
       
         
        private static View.View relationView;
        private static Model relatoinModel;
        public QueryFactory (SqlCon con,Context.ICurrentContextFactory conFac)
        {
            this.Con = con;
            this.ConFac = conFac;
            relationView = new View.AutoViewFactory(this.Con, this.ConFac).CreateDefaultListView(relatoinModel);
        }
        static QueryFactory()
        {
            relatoinModel = new AssemblyModelFactory(typeof(Relation).Assembly).GetModel(typeof(Relation));
          
        
        }
        public  List<Soway.Query.JoinTable>GetCanJoinedTables(Soway.Query.Entity.IQueryTable Table,
            Soway.Query.JoinQueryType JoinType)
        {

            var list = new List<Soway.Query.JoinTable>();
             var proxy =   new SqlServer.dbContext(this.Con,this.ConFac).GetDetail(Global.ModeMode,models[Table]);
             var Model = new ModelHelper(this.ConFac).GetFromProxy(proxy) as Model;
             if (JoinType == Soway.Query.JoinQueryType.Items || JoinType == Soway.Query.JoinQueryType.All)
             {
                 foreach (var relation in Model.Relations)
                 {
                     var codition = new Soway.Query.JoinCondition(SqlServer.SqlHelper.GetKeyCol(
                         Model), relation.TargetColumn);
                     var JoinTable = new Soway.Query.JoinTable();
                     JoinTable.Conditions.Add(codition);
                     JoinTable.LeftTable = new Soway.Query.SelectedTable() { Table = Table };
                     if (relation.RelationType == RelationType.Many2One || relation.RelationType == RelationType.One2Many)
                         JoinTable.RightTable = new Soway.Query.SelectedTable() { Table = GetTable(relation.RelationTable) };
                     else
                         JoinTable.RightTable = new Soway.Query.SelectedTable()
                         {
                             Table = new Soway.Query.Entity.IQueryTable()
                                 {
                                     DBName = relation.RelationTable,
                                     ShowName = relation.RelationTable
                                 }
                         };
                     list.Add(JoinTable);
                 }
             }
             if (JoinType == Soway.Query.JoinQueryType.Parent || JoinType == Soway.Query.JoinQueryType.All)
             {
                 relationView.Filter = "[SW_SYS_RELATION].[SW_SYS_RELATION_TABLE]='" + Table.DBName + "'";

                 var result = new Soway.Model.Context.ListViewQueryContext(this.Con,ConFac)
                 .Query(relationView, 0, 0);

                 foreach (var item in result.CurrentResult)
                 {
                     Relation relation =
                       new ModelHelper(this.ConFac).GetFromProxy(new Soway.Model.SqlServer.dbContext(this.Con,this.ConFac).GetDetail(relatoinModel, item.ObjectProxy.ID, true));

                     var codition = new Soway.Query.JoinCondition( relation.TargetColumn,
                         SqlServer.SqlHelper.GetKeyCol(Model));
                     var JoinTable = new Soway.Query.JoinTable();
                     JoinTable.Conditions.Add(codition);
                     JoinTable.LeftTable = new Soway.Query.SelectedTable() { Table = Table };
                     JoinTable.RightTable = new Soway.Query.SelectedTable() { Table = GetTable(
                         Model.DataTableName) };
                     list.Add(JoinTable);
                     
                 }

                 foreach (var property in Model.Properties.Where(p => p.Model != null 
                     && p.IsMultiMap == false&& p.PropertyType == PropertyType.BusinessObject && p.IsArray == false))
                 {

                     var codition = new Soway.Query.JoinCondition( 
                         property.DBName,
                         SqlServer.SqlHelper.GetKeyCol(
                         property.Model));
                     var JoinTable = new Soway.Query.JoinTable();
                     JoinTable.Conditions.Add(codition);
                     JoinTable.LeftTable = new Soway.Query.SelectedTable() { Table = Table };
                     JoinTable.RightTable = new Soway.Query.SelectedTable() { Table = GetTable(property.Model.DataTableName) };
                     list.Add(JoinTable);

                 }
             }

            return list;
        }

        private Dictionary<Soway.Query.Entity.IQueryColumn, Property> cols = new Dictionary<Soway.Query.Entity.IQueryColumn, Property>();
        public List<Soway.Query.Entity.IQueryColumn> GetColumns(Soway.Query.Entity.IQueryTable Table)
        {

            var table = Table;
            var proxy = new SqlServer.dbContext(this.Con, this.ConFac).GetDetail(Global.ModeMode, models[table]);

          
            var Model = new ModelHelper(this.ConFac).GetFromProxy(proxy) as Model;
            List<IQueryColumn> result = new List<IQueryColumn>();
            GetQueryModel(Model).Columns.ForEach(p => result.Add(p));
            return result;
           
        }

    
        public string GetStateStr(Soway.Query.Entity.IQueryColumn Col, string value)
        {


            var emunvalue = cols[Col].Model.EnumValues.FirstOrDefault(p => p.Value.ToString() == value);
            if (emunvalue == null)
                return "";
            else
                return emunvalue.String;
        }

        public List<Soway.Query.Entity.ColStateValue> GetStateValues(Soway.Query.Entity.IQueryColumn Col)
        {

            List<Soway.Query.Entity.ColStateValue> result = new List<Soway.Query.Entity.ColStateValue>();
            foreach (var i in cols[Col].Model.EnumValues)
            {
                result.Add(new Soway.Query.Entity.ColStateValue() { ShowName = i.String, DBName = i.Value.ToString() });
            }
            return result;
        }

        public Soway.Query.Entity.IQueryTable GetTable(string TableName)
        {
            if (models.Count == 0)
                GetTables();
            return models.Keys.First(p => p.DBName.ToUpper().Trim() == TableName.ToUpper().Trim());
        }

        private Dictionary<Soway.Query.Entity.IQueryTable, object> models = new Dictionary<Soway.Query.Entity.IQueryTable, object>();

        public SqlCon Con { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }

        public List<Soway.Query.Entity.IQueryTable> GetTables()
        {
            var result = new List<Soway.Query.Entity.IQueryTable>();
            if (models.Count == 0)
            {
                models.Clear();
                var view = new Soway.Model.View.AutoViewFactory(this.Con,this.ConFac).CreateDefaultListView(Global.ModeMode);
                view.Filter = "[SW_SYS_MODEL].MODEL_TYPE<>'" + System.Convert.ToString((int)ModelType.Enum) + "'";

                Soway.Model.Context.ListViewQueryContext cxt = new Context.ListViewQueryContext(this.Con,this.ConFac);
                var list = cxt.Query(view, 0, 0);
             
                foreach (var i in list.CurrentResult)
                {
                    Soway.Query.Entity.IQueryTable model = new Soway.Query.Entity.IQueryTable();
                    model.ShowName = i.ObjectProxy["Name"].ToString();
                    model.DBName = i.ObjectProxy["DataTableName"].ToString();

                    models.Add(model, i.ObjectProxy.ID);

                    result.Add(model);
                    // models.Add(model);
                }

            }
            result.AddRange(models.Keys.ToArray());
            return result;


        }
    }
}
