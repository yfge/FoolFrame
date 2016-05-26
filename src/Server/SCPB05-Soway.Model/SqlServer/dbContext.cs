using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Soway.Model.Context;
using System.Data; 

namespace Soway.Model.SqlServer
{
    public class dbContext
    {
       

       


       
        private SqlCon sqlCon = null;

        public ICurrentContextFactory ConFac { get; private set; }

        public dbContext(SqlCon con,Context.ICurrentContextFactory conFac)
        {
          
            sqlCon = con;
            ConFac = conFac;
        
             
        }

        #region 判断一个实例是否已经存在
        public bool IsExits(IObjectProxy proxy,
            System.Data.SqlClient.SqlTransaction trans = null, string parentCol = null, object id = null)
        {

            if (proxy.IsSave == SaveType.Exists)
                return true;
            if (proxy.Model != null && proxy.Model.ModelType == ModelType.Enum && proxy.ID != null)
            {
                proxy.IsSave = SaveType.Exists;
                return true;
            }
            if (proxy.Model != null&&proxy.Model.IdProperty !=null && proxy.Model.IdProperty.PropertyType == PropertyType.Guid
                )
            {
                if (String.IsNullOrEmpty((proxy.ID ??"").ToString().Trim())  || (Guid)proxy.ID == Guid.Empty)
                {
                    if (proxy.Model.Properties.Count
                        (p => p.IsCheck == true && String.IsNullOrEmpty(p.IXGroup) == false) == 0)
                    {
                        proxy.IsSave = SaveType.UnExists;
                        return false;
                    }
                }
            }
            if (proxy.Model !=null && proxy.Model.AutoSysId == true)
            {

                if (proxy.ID !=null &&System.Convert.ToInt64(proxy.ID) != 0  
                    )
                {
                    if (proxy.Model.Properties.Count(p => p.IsCheck
                    && p != proxy.Model.IdProperty) == 0)
                    {
                        proxy.IsSave = SaveType.Exists;
                        return true;
                    }

                }
            }
            {


                object ob = null;
                System.Data.SqlClient.SqlConnection con = null;
                if (trans != null)
                {
                    con = trans.Connection;
                }
                else
                {
                    con = new System.Data.SqlClient.SqlConnection(this.GetSqlCon(proxy.Model).ToString());
                    con.Open();
                }

                var commnad = con.CreateCommand();
                if (trans != null)
                    commnad.Transaction = trans;


                if (proxy.Model.ID == 0)
                {

                    commnad.CommandText = string.Format("SELECT * FROM SYS.TABLES WHERE NAME='{0}'", proxy.Model.DataTableName.Replace("[", "").Replace("]", ""));
                    if (commnad.ExecuteScalar() == null)
                    {
                        if (trans == null)
                            con.Close();
                        proxy.IsSave = SaveType.UnExists;
                        return false;
                    }
                }

             
                commnad.CommandText = "SELECT [" + SqlHelper.GetKeyCol(proxy.Model) + "] FROM " + 
                    proxy.Model.DataTableName + " WHERE '1'='2'";

                var keyGroups = proxy.Model.Properties.Where(p => p.IsCheck 
                    && p != proxy.Model.IdProperty).Select(p => p.IXGroup).Distinct();
                if (keyGroups.Count() > 0)
                    foreach (var i in keyGroups)
                    {

                        var goupProperty = proxy.Model.Properties.Where(p => p.IsCheck && p.IXGroup == i );//&& items.ContainsKey(p.DBName));
                        if (goupProperty.Count() > 0)
                        {

                            commnad.CommandText += "\r\n OR ( '1'='1'  ";
                            foreach (var property in goupProperty)
                            {


                                if (property.PropertyType == PropertyType.BusinessObject && proxy[property] !=null)
                               {
                                   if(IsExits(proxy[property] as IObjectProxy,trans)==false )
                                       Create(proxy[property] as IObjectProxy);
                               }


                               if (property.PropertyType == PropertyType.BusinessObject
                                   && proxy[property] as IObjectProxy != null
                                   &&IsExits(proxy[property] as IObjectProxy) == false)
                                   return false;
                               var value = property.PropertyType == PropertyType.BusinessObject ?
                                  (proxy[property] as IObjectProxy == null ? DBNull.Value : (proxy[property] as IObjectProxy).ID) : (proxy[property] ?? DBNull.Value);
                                    ;
                                commnad.Parameters.Add(new System.Data.SqlClient.SqlParameter(property.DBName,
                                   value));
                                if(value != DBNull.Value)
                                    
                                commnad.CommandText += "\r\nAND [" + property.DBName + "]=@" + property.DBName;
                                else

                                    
                                commnad.CommandText += "\r\nAND [" + property.DBName + "] is NULL";// + property.DBName;

                            }
                            commnad.CommandText += ")";
                        }
                    }
                else
                {
                    var items = GetColsAndValues(proxy, trans, OperationType.CheckIsExits);
                    if(items==null)
                    {
                        proxy.IsSave = SaveType.UnExists;
                        return false;
                    }
                    commnad.CommandText += "\r\n or ( '1'='1'  ";
                    foreach (var i in items.Keys.Where(p => p.ToUpper() != SqlHelper.GetKeyCol(proxy.Model).ToUpper()))
                    {
                       
                        var value = items[i] ?? DBNull.Value;
                        commnad.Parameters.Add(new System.Data.SqlClient.SqlParameter(i, items[i]??DBNull.Value));
                        if(value != DBNull.Value)
                            commnad.CommandText += "\r\nAND [" + i + "]=@" + i;
                        else
                            commnad.CommandText += "\r\nAND [" + i + "] IS NULL";// +i;
                    } 
                    commnad.CommandText += ")";
                }

                if (string.IsNullOrEmpty(parentCol) == false
                    && commnad.Parameters.Contains(parentCol )==false )
                {
                    commnad.CommandText += string.Format(" AND {0}=@{0}", parentCol);
                    commnad.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + parentCol, id ?? DBNull.Value));
                }
                var dataTable = new System.Data.DataTable();
                new System.Data.SqlClient.SqlDataAdapter(commnad).Fill(dataTable);

                if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0] != DBNull.Value)
                    ob = dataTable.Rows[0][0];

                if (trans == null)
                    con.Close();


                if (ob != null)
                {
                    proxy.ID = ob;
                    if (!proxy.Model.AutoSysId)
                    {
                        proxy[proxy.Model.IdProperty] = ob;

                    }
                    proxy.IsSave = SaveType.Exists;
                    return true;

                }
                else
                {
                    proxy.IsSave = SaveType.UnExists;
                    return false;
                }
            }
        }
        #endregion
        #region 创建相关
        public void Create(IObjectProxy proxy)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.sqlCon.ToString()))
            {
              
                    con.Open();
                    System.Data.SqlClient.SqlTransaction trans = con.BeginTransaction();
                    Create(proxy, trans);
                    trans.Commit();
                    proxy.IsSave =  SaveType.Exists;
            }

            foreach (var trigger in proxy.Model.Triggers.Where(p => p.ModelTriggerType == ModelTriggerType.Create))
                new ModelMethodContext(this.sqlCon,this.ConFac).ExcuteOperation(proxy, trigger);
            MemoryCache.StoreDbCache.UpdateOrAddDbMemoryCache(
                this.sqlCon, proxy.Model, proxy.ID, proxy);
        }


        private void Create(IObjectProxy proxy, 
            System.Data.SqlClient.SqlTransaction trans, String parentCol = "", object id = null)
        {
            var commnad = BuildCreateCommand(proxy, trans, parentCol, id);
            commnad.Transaction = trans;
            commnad.Connection = trans.Connection;
            if (proxy.IsSave != SaveType.Exists)
            {
                var ob= commnad.ExecuteScalar();
                long tempID;
                long.TryParse((proxy.ID ?? "").ToString(), out  tempID);

                if (ob != null && ob != DBNull.Value && string.IsNullOrEmpty(ob.ToString()) == false
                    &&(proxy.ID ==null|| tempID == 0||(proxy.ID??"").ToString() ==Guid.Empty.ToString()))
                {

        
                    proxy.ID =System.Convert.ToInt64( ob);
                }
                proxy.IsSave = SaveType.Exists;
            }
            else
            {
                save(proxy, trans, parentCol, id);
            }
            if (proxy.Model.AutoSysId == false &&proxy.Model.IdProperty!= null)
                proxy[proxy.Model.IdProperty] = proxy.ID;
             
            foreach (var property in proxy.Model.Properties.Where(p => p.IsArray))
            {
                dynamic items = proxy[property];
                var relation = proxy.Model.Relations.First(p => p.Property == property);

                if (relation.RelationType == RelationType.One2Many
                    ||relation.RelationType == RelationType.Many2One)
                {
                    foreach (IObjectProxy itemProxy in items)
                    {
                        if ((proxy as ObjectProxyClass).KeyPairs.
                            Count(p => p.Value .Data== itemProxy||p.Value.Old == itemProxy) > 0)
                        {
                         
                            save(itemProxy, trans, relation.TargetColumn, proxy.ID);
                        }
                        else
                        {

                            if (IsExits(itemProxy, trans, relation.TargetColumn, proxy.ID) == false)
                            {
                                if(itemProxy.IsSave != SaveType.Exists)
                                    Create(itemProxy, trans, relation.TargetColumn, proxy.ID);
                                else
                                    save(itemProxy, trans, relation.TargetColumn, proxy.ID);
                            }
                            else
                                save(itemProxy, trans, relation.TargetColumn, proxy.ID);
                        }
                    }
                }
                else if(relation.RelationType == RelationType.Many2Many
                    ||relation.RelationType == RelationType.Recurve)
                {

                 
                    foreach (IObjectProxy item in items)
                    {
                        if (IsExits(item,trans) == false)
                            Create(item,trans);
                        CreateComplexRelationBuild(proxy, trans, relation, item);
                    }

                }
            }

        }

        #endregion
        #region 保存相关
        public void Save(IObjectProxy proxy)
        {
            
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.sqlCon.ToString()))
            {
                con.Open();
                 
                System.Data.SqlClient.SqlTransaction trans = con.BeginTransaction();
                save(proxy, trans);
                trans.Commit();
                MemoryCache.StoreDbCache.UpdateOrAddDbMemoryCache(this.sqlCon,
                    proxy.Model, proxy.ID, proxy);
            }
            foreach (var trigger in proxy.Model.Triggers.Where(p => p.ModelTriggerType == ModelTriggerType.Save))
                new ModelMethodContext(this.sqlCon,this.ConFac).ExcuteOperation(proxy, trigger);
        }
        private void save(IObjectProxy proxy, System.Data.SqlClient.SqlTransaction trans = null, string tagetCol = null, object targetId = null)
        {
            var items = GetColsAndValues(proxy, trans, OperationType.Update);
            System.Data.SqlClient.SqlConnection con = null;
            if (trans != null)
            {
                con = trans.Connection;
            }
            else
            {
                con = new System.Data.SqlClient.SqlConnection(this.GetSqlCon(proxy.Model).ToString());
                con.Open();
            }

            var commnad = con.CreateCommand();
            if (trans != null)
                commnad.Transaction = trans;

            commnad.CommandText = string.Format("UPDATE {0} SET ", proxy.Model.DataTableName);
            var str = SqlHelper.GetKeyCol(proxy.Model);
            foreach (var key in items.Keys.Where(p=>p!=str))
            {
               
                commnad.CommandText += String.Format("\r\n {0}=@{0},", key);
                commnad.Parameters.AddWithValue(key, items[key]);

            }
            if (string.IsNullOrEmpty(tagetCol) == false)
            {
                if (commnad.Parameters.Contains(tagetCol) == false)
                {
                    commnad.Parameters.Add(new System.Data.SqlClient.SqlParameter(tagetCol, targetId));
                    commnad.CommandText += String.Format("\r\n {0}=@{0},", tagetCol);
                }
                
            }
                
            commnad.CommandText = commnad.CommandText.Substring(0, commnad.CommandText.Length - 1);
            commnad.CommandText += string.Format(" WHERE {0}=@{0}", SqlHelper.GetKeyCol(proxy.Model));
            commnad.Parameters.Add(new System.Data.SqlClient.SqlParameter(SqlHelper.GetKeyCol(proxy.Model), proxy.OldId??
                proxy.GetOld(proxy.Model.IdProperty)));
            commnad.ExecuteNonQuery();
            foreach (var property in proxy.Model.Properties.Where(
                p => p.IsArray))
            {
                if (proxy.GetLoadType(property) == LoadType.Complete)
                {
                    dynamic proxyItems = proxy[property];
                    var relation = proxy.Model.Relations.First(p => p.Property == property);

                    if (relation.RelationType == RelationType.One2Many)
                    {
                        foreach (IObjectProxy itemProxy in proxyItems)
                        {
                            if (IsExits(itemProxy, trans, relation.TargetColumn, proxy.ID) == false)
                            {
                                Create(itemProxy, trans, relation.TargetColumn, proxy.ID);
                            }
                            else
                            {
                                save(itemProxy, trans, relation.TargetColumn, proxy.ID);
                            }
                        }
                        foreach (IObjectProxy deleteProxy in proxyItems.ToRemove)
                        {
                            delete(deleteProxy, trans);
                        }
                    }
                    else if (relation.RelationType == RelationType.Many2Many
                        || relation.RelationType == RelationType.Recurve)
                    {



                        foreach (IObjectProxy item in proxyItems)
                        {
                            //if (IsExits(item, trans) == false)
                            //    Create(item, trans);
                            CreateComplexRelationBuild(proxy, trans, relation, item);
                        }

                        foreach (IObjectProxy item in proxyItems.ToRemove)
                        {
                            //if (IsExits(item, trans) == false)
                            //    Create(item, trans);
                            DeleteComplexRelationBuild(proxy, trans, relation, item);
                        }



                    }
                }
            }

        }
        #endregion
       
        
        private void  CreateComplexRelationBuild(IObjectProxy proxy,
            System.Data.SqlClient.SqlTransaction trans, 
            Relation relation, IObjectProxy itemProxy)
        {
            if (IsExits(itemProxy,trans)==false )
            {

                Create(itemProxy, trans);
 
            }

            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
 
            command.CommandText = String.Format(@"IF NOT EXISTS(SELECT * FROM {0}  WHERE {1}=@{1} AND {2} =@{2}) 
                BEGIN
                    INSERT {0}  ({1},{2}) VALUES (@{1},@{2})
                END",
                relation.RelationTable, relation.PropertyColumn, relation.TargetColumn);


            if (relation.RelationType == RelationType.Recurve)
            {
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.PropertyColumn, proxy.ID));
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.TargetColumn, itemProxy.ID));
            }
            else
            {
                //这个应该是对的。。。
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.PropertyColumn, itemProxy.ID));
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.TargetColumn,  proxy.ID));

            }
            command.Transaction = trans;
            command.Connection = trans.Connection;
            command.ExecuteNonQuery();
        }
        private void DeleteComplexRelationBuild(IObjectProxy proxy,
           System.Data.SqlClient.SqlTransaction trans,
           Relation relation, IObjectProxy itemProxy)
        {

            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            command.CommandText = String.Format(@"DELETE {0}  WHERE {1}=@{1} AND {2} =@{2}",
                relation.RelationTable, relation.PropertyColumn, relation.TargetColumn);


            if (relation.RelationType == RelationType.Recurve)
            {
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.PropertyColumn, proxy.ID));
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.TargetColumn, itemProxy.ID));
            }
            else
            {
                //这个应该是对的。。。
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.PropertyColumn, itemProxy.ID));
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + relation.TargetColumn, proxy.ID));
            }
            command.Transaction = trans;
            command.Connection = trans.Connection;
            command.ExecuteNonQuery();
        }


        private void delete(IObjectProxy proxy, System.Data.SqlClient.SqlTransaction trans)
        {
            System.Data.SqlClient.SqlConnection con = null;
            if (trans != null)
            {
                con = trans.Connection;
            }
            else
            {
                con = new System.Data.SqlClient.SqlConnection(this.GetSqlCon(proxy.Model).ToString());
                con.Open();
            }

            var commnad = con.CreateCommand();
            if (trans != null)
                commnad.Transaction = trans;

            commnad.CommandText = string.Format("DELETE  {0} WHERE {1}=@{1} ", proxy.Model.DataTableName,SqlHelper.GetKeyCol(proxy.Model));
            commnad.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + SqlHelper.GetKeyCol(proxy.Model), proxy.ID));
            commnad.ExecuteNonQuery();
            foreach (var property in proxy.Model.Properties.Where(p => p.IsArray))
            {
                dynamic proxyItems = proxy[property];
                var relation = proxy.Model.Relations.First(p => p.Property == property);

                if (relation.RelationType == RelationType.One2Many)
                {

                    foreach (IObjectProxy deleteProxy in proxyItems.ToRemove)
                    {
                        delete(deleteProxy, trans);
                    }
                }
            }
            


        }



         public void Delete(IObjectProxy proxy) {


             foreach (var trigger in proxy.Model.Triggers.Where(p => p.ModelTriggerType == ModelTriggerType.Delete))
                 new ModelMethodContext(this.sqlCon,this.ConFac).ExcuteOperation(proxy, trigger);
             using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.GetSqlCon(proxy.Model).ToString()))
             {
                 con.Open();
                 System.Data.SqlClient.SqlTransaction trans = con.BeginTransaction();
                 delete(proxy, trans);
                 trans.Commit();
             }
             
         }
        

     



        private SqlCon GetSqlCon(Model model)
        {
            if (model.SqlCon != null)
                return model.SqlCon;
            else if (model.Module.SqlCon != null)
                return model.Module.SqlCon;
            else
            return this.sqlCon;
        }

 
        public IObjectProxy GetDetail(Model model, object id,bool LoadDetail=true)
        {

             
            if (String.IsNullOrEmpty((id ?? "").ToString()))
                return null;
            IObjectProxy proxy = SqlDataLoader.getProxy(model, id,this.sqlCon,this.ConFac);
            LoadDataDetail(model, id, LoadDetail, proxy);
            return proxy;
        }
        private SqlCon GetPropertyCon(Property property)
        {
            if (property.PropertySqlCon != null)
                return property.PropertySqlCon;
            else
                return this.sqlCon;
        }
        private bool CheckDecimal(object ob)
        {
            var result =  ob is int || ob is long || ob is uint || ob is ulong || ob is short;
            if (result)
                return result;
            long lob = 0;
            result |=long.TryParse((ob ?? "").ToString(), out lob);
            if (result)
                return result;
            decimal dob = 0;
            result |= decimal.TryParse((ob ?? "").ToString(), out dob);
            return result;

        }

        public void LoadDataDetail(Model model, object id, bool LoadDetail, IObjectProxy proxy)
        {
          
            if (proxy.IsLoad == LoadType.Null ||proxy.IsLoad == LoadType.NoObj|| proxy.IsLoad == LoadType.Partial)
            {
                proxy.IsLoad = LoadType.Complete;
                if (model.ModelType == ModelType.Enum)
                {
                    proxy.ID = id;
                }
                else
                {
                    using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(SqlHelper.GetSqlCon(this.sqlCon, proxy.Model).ToString()))
                    {
                        con.Open();

                        var command = SqlHelper.GenerateGetItemsCommand(model);
                        command.Connection = con;
                        var keycols = SqlHelper.GetKeyCols(model);
                        int j = 0;
                        bool isAdd = false;
                        for (int i =0;i<keycols.Length;i++)// keycol in keycols)
                        {
                            var keycol = keycols[i];

                            
                             

                            var property = model.Properties.FirstOrDefault(p => p.DBName == keycol);
                            if(property != null)
                            {
                                Guid guid;
                                DateTime date;
                                if (property.PropertyType == PropertyType.DateTime && DateTime.TryParse((id ?? "").ToString(), out date)){
                                    isAdd = true;
                                }
                                else if (property.PropertyType == PropertyType.Time && DateTime.TryParseExact((id ?? "").ToString(), property.Format, null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out date)){
                                    isAdd = true;
                                }
                                else if (property.PropertyType == PropertyType.Time && DateTime.TryParseExact((id ?? "").ToString(), property.Format, null, System.Globalization.DateTimeStyles.AllowInnerWhite, out date)){
                                    isAdd = true;
                                }else
                                if (PropertyTypeAdaper.GetPropertyType(id.GetType()) == property.PropertyType ||
                                   ( property.PropertyType == PropertyType.IdentifyId && CheckDecimal(id))||property.PropertyType == PropertyType.SerialNo)
                                {
                                    isAdd = true;
                                }else if (
                                    property.PropertyType == PropertyType.Guid 
                                    && Guid.TryParse ((id??"").ToString(),out guid) == true)
                                {
                                    isAdd = true;
                                }
                            }else 
                            if (keycols.Length == 1)
                                isAdd = true;
                            else
                            {

                                isAdd = CheckDecimal(id);
                               
                            }

                            if (isAdd)
                            {
                                string pre = j == 0 ? " WHERE " : " OR ";
                                command.CommandText += pre + "  [" + keycol + "]=@" + keycol;

                                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + keycol, id));
                                j++;
                                isAdd = false;
                            }
                        }

                     
                        var table = SqlDataLoader.GetSqlData(command);

                        if (table.Rows.Count > 0)
                        {
                            SqlDataLoader.LoadSqlData(proxy, table.Rows[0],this.sqlCon,this.ConFac);


                            if (LoadDetail)
                            {
                                proxy.IsLoad = LoadType.Complete;
                                foreach (var property in 
                                    model.Properties.Where(p => p.IsArray == false
                                    && p.PropertyType == PropertyType.BusinessObject))
                                {

                                    ObjectProxyClass propertyProxy = proxy[property] as ObjectProxyClass;
                                    if ((propertyProxy != null 
                                        && propertyProxy.IsLoad != LoadType.Complete)
                                        ||(property.PropertySqlCon!=null &&property.PropertySqlCon.ToString()!= propertyProxy.Con.ToString()))
                                    {
                                        proxy[property] = new dbContext(GetPropertyCon(property),this.ConFac).  GetDetail(propertyProxy.Model, propertyProxy.ID, false);
                                    }
                                }

                            }
                        }
                        else
                        {
                            proxy.IsLoad = LoadType.NoObj;
                        }


                    }

                }
            }


        }

       
        internal     List<IObjectProxy> LoadArrayProperty(
           IObjectProxy ParentProxy,
           Property property)
        {

            var sql =  SqlHelper.GetSqlCon(property, this.sqlCon, (property.Model ==null?ParentProxy.Model:property.Model));
            if (sql != null)
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(
                    sql.ToString()
          ))
                {
                    con.Open();
                    var sqlCommand = SqlHelper.generatGetItemsCommand(property, ParentProxy.Model, ParentProxy.ID);
                    string itemKeyCol = SqlHelper.GetKeyCol(property.Model);
                    sqlCommand.Connection = con;

                    System.Diagnostics.Trace.WriteLine(sqlCommand.CommandText);
                    var table2 = SqlDataLoader.GetSqlData(sqlCommand);
                    
                    List<IObjectProxy> result = new List<IObjectProxy>();
                    if (table2.Rows.Count > 0)
                    {
                        for (int i = 0; i < table2.Rows.Count; i++)
                        {
                            var itemproxy = SqlDataLoader.getProxy(property.Model, table2.Rows[i][itemKeyCol],this.sqlCon,this.ConFac                                         );
                            
                            if (itemproxy != null)
                            {
                                itemproxy.Owner = ParentProxy;
                                result.Add(itemproxy);
                                SqlDataLoader.LoadSqlData(itemproxy, table2.Rows[i],this.sqlCon,this.ConFac);
                               
                                itemproxy.IsLoad =  LoadType.Complete;

                            }
                        }
                    }
                    return result;
                }
            else return new List<IObjectProxy>();
        }


        public void Excute(System.Data.SqlClient.SqlCommand command)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.sqlCon.ToString()))
            {
                con.Open();
                command.Connection = con;
                command.ExecuteNonQuery();
                
            }

        }

         

        private   System.Data.SqlClient.SqlCommand BuildCreateCommand(IObjectProxy proxy,
            System.Data.SqlClient.SqlTransaction trans,
            string ParentCol = "",
            object id = null)
        {


            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();

            string values = "\r\n VALUES(";
            command.CommandText = "INSERT " + proxy.Model.DataTableName + "(";

   foreach (var property in proxy.Model.Properties.Where(p => p.PropertyType == PropertyType.SerialNo))
            {

                if (String.IsNullOrEmpty((property.DBName ?? "").Replace("[", "").Replace("]", "")) == false)
                {
                    var serialNo = createSerialNo(proxy, property);
                    proxy[property] = serialNo;

                    if(proxy.Model.AutoSysId ==false &&proxy.Model.IdProperty == property)
                    {
                        proxy.ID = serialNo;
                    }
                }
                 
            }
            var items = GetColsAndValues(proxy, trans, OperationType.Insert);
         
            foreach (var item in items)
            {

                command.CommandText += "[" + item.Key + "]\r\n,";
                values += "@" + item.Key + "\r\n,";
                command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + item.Key, item.Value));

            }
            if (String.IsNullOrEmpty(ParentCol) == false )
            {

                if (command.Parameters.Contains("@" + ParentCol) == false)
                {
                    command.CommandText += "[" + ParentCol + "]\r\n,";

                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + ParentCol, id));
                    values += "@" + ParentCol + "\r\n,";
                }
                else
                    command.Parameters["@" + ParentCol].Value = id;
            }
            command.CommandText = command.CommandText.Substring(0, command.CommandText.Length - 1);
            command.CommandText += ")\r\n" + values.Substring(0, values.Length - 1) + ")\r\n ";
            if(proxy.Model.Properties.Count(p=>p.PropertyType == PropertyType.IdentifyId )>0 ||proxy.Model.AutoSysId)
                command.CommandText += "select @@IDENTITY \r\n";



            return command;

        }


        private enum OperationType
        {
            Insert =0,
            CheckIsExits=1,
            Update =2
        }
          private    Dictionary<String, object> 
              GetColsAndValues(IObjectProxy proxy, System.Data.SqlClient.SqlTransaction trans, OperationType operationType)
        {
            Dictionary<String, object> result = new Dictionary<string, object>();
            List<string> temp = new List<string>();
            ///简单数据
            ///

            //当check为真是用来校验是否已经存在，这时
            foreach (var property in proxy.Model.Properties.Where(p => 
            
                p.IsArray == false 
                && 
                (string.IsNullOrEmpty(p.DBName) == false || p.IsMultiMap == true)
                ))
            {
                if (temp.Contains(property.Name))
                {
                   
                    //已经包含
                    continue;
                }
                else
                {

                    temp.Add(property.Name);
                }
                if (property.PropertyType == PropertyType.RadomDECS)
                {
                    //加密，加入

                    var enDic = EncryptionClass.GetEncryptionValus(property, proxy[property]);
                    foreach (var str in enDic.Keys)
                    {
                        result.Add(str, enDic[str]);
                    }

                }
                else if (property.PropertyType == PropertyType.MD5)
                {
                    result.Add(property.DBName, EncryptionClass.ToMD5((proxy[property] ?? "").ToString()).ToString());
                }
                else if (property.PropertyType == PropertyType.DateTime
                    || property.PropertyType == PropertyType.Date
                    || property.PropertyType == PropertyType.Time)
                {

                    //日期
                    if (property.AutoGenerationType == Data.Discription.ORM.GenerationType.OnInSert
                        && (operationType== OperationType.Insert || IsExits(proxy) == false))
                    {
                        result.Add(property.DBName, DateTime.Now);

                    }
                    else if ((property.AutoGenerationType == Data.Discription.ORM.GenerationType.OnInsertAndUpate
                       || property.AutoGenerationType == Data.Discription.ORM.GenerationType.OnUpdate) &&( operationType== OperationType.Update ) )
                    {
                        result.Add(property.DBName, DateTime.Now);
                    }
                    else if((property.AutoGenerationType == Data.Discription.ORM.GenerationType.OnInSert)
                        && operationType!= OperationType.Insert)
                    {
                        continue;
                    }else 
                    {

                        var str = (proxy[property] ?? DateTime.Now).ToString();
                        object ob = DBNull.Value;
                        if (String.IsNullOrEmpty(str))
                            ob = DBNull.Value;
                        else
                        {
                            DateTime fmt;
                            if(DateTime.TryParseExact(str.Replace('/',' ').Replace(':',' ').Replace(" ",""),"yyyyMMddHHmmss",null,System.Globalization.DateTimeStyles.AssumeLocal,out fmt) == true)
                            {
                                ob = fmt;
                            }
                        }
                        result.Add(property.DBName, ob);
                    }
                }else if(property.AutoGenerationType == Data.Discription.ORM.GenerationType.OnInSert
                    && operationType!= OperationType.CheckIsExits && property.PropertyType!= PropertyType.SerialNo)
                {

                    continue;
                }
                else if (property.PropertyType == PropertyType.Guid &&
                    (String.IsNullOrEmpty((proxy[property] ?? "").ToString()) == true
                    || Guid.Parse((proxy[property] ?? "").ToString()) == Guid.Empty)
                    && operationType!= OperationType.CheckIsExits)
                {
                    proxy[property] = Guid.NewGuid();//.ToString();
                    result.Add(property.DBName, proxy[property]);


                }
                else


                    if (property.PropertyType != PropertyType.BusinessObject)
                {
                    if (String.IsNullOrEmpty((property.DBName ?? "").Replace("[", "").Replace("]", "")) == false)
                    {

                        var valueOb = proxy[property];
                         if (valueOb is DateTime && (DateTime)valueOb == DateTime.MinValue)
                            result.Add(property.DBName, DBNull.Value);
                        else
                            result.Add(property.DBName, valueOb ?? "");
                    }

                }
                else
                {
                    var objValue = proxy[property];

                    IObjectProxy propertyProxy = proxy[property] as IObjectProxy;
                    if (propertyProxy != null)
                    {
                        if (propertyProxy.ID == null
                            || (propertyProxy.ID is long && System.Convert.ToInt64(propertyProxy.ID) == 0)
                            || (propertyProxy.ID is Guid && (Guid)propertyProxy.ID == Guid.Empty))
                        {


                            //???
                            if (operationType== OperationType.CheckIsExits)
                                return null;
                            //不存在,先加到里面 
                            bool chk = IsExits(propertyProxy, trans);
                            if (chk == false)
                            {
                                if (propertyProxy.IsSave != SaveType.Exists)
                                {
                                    if (trans != null)
                                        Create(propertyProxy, trans);
                                    else
                                        Create(propertyProxy);
                                }
                                else
                                {
                                    if (trans != null)
                                        save(propertyProxy, trans);
                                    else
                                        save(propertyProxy);

                                }
                            }
                        }
                        if (property.IsMultiMap)
                        {
                            foreach (var propertymaps in property.DBMaps)
                            {
                                result.Add(propertymaps.DBColName, propertyProxy[propertyProxy.Model.Properties.First(p =>
                                    p.Name == propertymaps.PropertyName
                                    || p.PropertyName == propertymaps.PropertyName)] ?? "");

                            }
                        }
                        else
                        {
                            result.Add(property.DBName, propertyProxy.ID);

                        }

                    } if(objValue != null)
                    {
                        result.Add(property.DBName, objValue);
                    }
                    else
                    {

                        if (property.IsMultiMap)
                        {
                            foreach (var propertymaps in property.DBMaps)
                            {
                                result.Add(propertymaps.DBColName, DBNull.Value);

                            }
                        }
                        else
                        {
                            result.Add(property.DBName, DBNull.Value);

                        }
                    }
                } }
            return result;
        }

          private String createSerialNo(IObjectProxy proxy,Property property )
          {
              var str = property.Format;
              var nostrs = str.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
              string Fomate = "";
              int len = 0;
              var dataFormat = "";
            int prelen = 0;
              foreach (var item in nostrs)
              {
                  if (item == "S")
                  {
                      
                  }
                  else if (item[0] == 'S')
                  {

                      //时间
                      var strs = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                      len = System.Convert.ToInt32(strs[0].Substring(1));
                      if (strs.Length > 1)
                          prelen = System.Convert.ToInt32(strs[1]);



                  }
                  else if (item[0] == 'D')
                  {

                      var date = new Soway.DB.DBContext(GetSqlCon(proxy.Model).ToString()).GetServerDateTime().ToString(item.Substring(1));
                      Fomate += date;
                  }
                  else if (item[0] == 's')
                  {
                      Fomate += item.Substring(1);
                  }
                  else
                      Fomate += item.Trim();


              }
              string getPreStr = Fomate;
        
              if (prelen == 0)
                  prelen = Fomate.Length;
              if(string.IsNullOrEmpty(Fomate)==false )
              getPreStr = Fomate.Substring(0, prelen);
          //   //// // System.Diagnostics.Trace.WriteLine("preLen:" + getPreStr);
              var s = new Soway.DB.DBContext(GetSqlCon(proxy.Model).ToString()).GetSerialNo(getPreStr, len, "");
              return s.Insert(prelen, Fomate.Substring(prelen, Fomate.Length - prelen));
          }

 

        /// <summary>
        /// 得到一个父对像
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
          public IObjectProxy GetParent(IObjectProxy proxy)
        {

            //1 得到关系
            if (
            proxy.Model.Owner != null)
            {
                var property = proxy.Model.Owner.Properties.FirstOrDefault(
                    p => p.Model == proxy.Model && p.IsArray == true);
                if (property != null)
                {
                    var relation = property.Model.Owner.Relations.FirstOrDefault(p => p.Property == property);
                    if (relation != null)
                    {
                        var sql = string.Format(@"SELECT {0} FROM {1} WHERE {2}=@id",
                              relation.TargetColumn,
                              relation.RelationTable,
                              SqlHelper.GetKeyCol(property.Model)

                             );
                        var command = new System.Data.SqlClient.SqlCommand();
                        command.CommandText = sql;
                        command.Parameters.AddWithValue("@id", proxy.ID);
                        using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(this.sqlCon.ToString()))
                        {

                            con.Open();
                            command.Connection = con;
                            var projid = command.ExecuteScalar();
                            con.Close();
                            return GetDetail(proxy.Model.Owner, projid, false);

                        }
                    }
                }
            }
            return null;
            //2 得到
        }



        public List<dynamic> GetBySqlCommand(Model model,System.Data.SqlClient.SqlCommand command)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(SqlHelper.GetSqlCon(this.sqlCon, model).ToString()))
            {
                con.Open();
                command.Connection = con;
                var table = SqlDataLoader.GetSqlData(command);

                var keyCol = SqlHelper.GetKeyCol(model);

                List<dynamic> items = new List<dynamic>();
                con.Close();
                if (table.Columns.Contains(keyCol) == false)
                    throw new Exception("Can't Gerneration Query Because The Id Column Isn't Included!");
                if (table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        IObjectProxy proxy = SqlDataLoader.getProxy(model, row[keyCol], this.sqlCon, this.ConFac);
                        SqlDataLoader.LoadSqlData(proxy, row, this.sqlCon, this.ConFac);
                        proxy.IsSave = SaveType.Exists;
                        items.Add(proxy);
                    }
                }
                return items;
            }


            }

        public List<dynamic> GetBySqlCommand(Model model,String sqlScript)
        {
            return GetBySqlCommand(model,new System.Data.SqlClient.SqlCommand() { CommandText = sqlScript });
        }
        
    }
}
