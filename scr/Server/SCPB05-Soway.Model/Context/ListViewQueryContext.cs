using Soway.Data;
using Soway.Model.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Context
{
    /// <summary>
    ///通过View查询得到列表
    /// </summary>
    public class ListViewQueryContext
    {
        public SqlCon Con { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }

        public QueryResult Query(Soway.Model.View.View model, int COUNT, int perPage = 20,
            string filter = null, ViewItem OrderByItem = null, OrderByType orderByType = OrderByType.DESC)
        {

            QueryResult result = new QueryResult();
            result.CurrentPageIndex = COUNT;
            string querySql = "";
            var tableSql = "";
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.Connection = new System.Data.SqlClient.SqlConnection(SqlHelper.GetSqlCon(this.Con,model.Model).ToString());
            command.Connection.Open();
            querySql = "SELECT ";
            tableSql = string.Format("[{0}]", model.Model.DataTableName);


            
            string orderby = "";

            List<String> select = new List<string>();
            foreach (var selectedItem in model.Items)
            {

                if (select.Contains(selectedItem.Name))
                    continue;
                else
                    select.Add(selectedItem.Name);
                if (selectedItem.Property.PropertyType != PropertyType.BusinessObject)
                {
                    if (string.IsNullOrEmpty(selectedItem.Property.DBName) == false)
                    {
                        if (selectedItem.Property.PropertyType != PropertyType.RadomDECS)
                        {


                            querySql += string.Format("[{0}].[{1}] as [{0}_{1}],", model.Model.DataTableName, selectedItem.Property.DBName);
                           if(OrderByItem == selectedItem)
                               orderby = string.Format("[{0}].[{1}]", model.Model.DataTableName, selectedItem.Property.DBName);
                        }
                        else
                        {
                            var cols = EncryptionClass.GetPropertyCols(selectedItem.Property);
                            foreach (var col in cols)
                                querySql += string.Format("[{0}].[{1}] as [{0}_{1}],", model.Model.DataTableName, col);

                        }
                    }
                }
                else if (selectedItem.Property.IsMultiMap)
                {
                    //如果为多重映射
                    foreach(var db in selectedItem.Property.DBMaps)
                    {
                        querySql += string.Format("[{0}].[{1}] as [{2}_{3}],",
                         model.Model.DataTableName,
                         db.DBColName,
                         selectedItem.Property.Name,
                         db.PropertyName);

                        
 
                    }

                    if (OrderByItem == selectedItem && 
                       selectedItem.Property.Model.ShowProperty !=null
                        &&selectedItem.Property.DBMaps.Count(p => p.PropertyName == selectedItem.Property.Model.ShowProperty.Name) > 0)
                    {
                        orderby = string.Format("[{0}].[{1}]",
                         model.Model.DataTableName,
                         selectedItem.Property.DBMaps.First(p => p.PropertyName == selectedItem.Property.Model.ShowProperty.Name).DBColName);// ,
                    }
                }
                else
                {
                    //不为多重映射
                    if (selectedItem.Property.Model.IdProperty !=null
                        && selectedItem.Property.Model.IdProperty == selectedItem.Property.Model.ShowProperty)
                    {
                        //这里还没有搞定 
                        //主键就是要显示的值 ,直接忽略
                        querySql += string.Format("[{0}].[{1}] as   [{2}_{3}],", 
                            model.Model.DataTableName, 
                            selectedItem.Property.DBName,
                            selectedItem.Property.Name,
                            selectedItem.Property.Model.ShowProperty.DBName);

                        if(selectedItem == OrderByItem)
                            orderby = string.Format("[{0}].[{1}]",
                            model.Model.DataTableName,
                            selectedItem.Property.DBName);

                    }
                    else
                    {
                        //主健不是要显示的值,联查
                        querySql += string.Format("[{0}].[{1}] as [{0}_{1}],",
                            selectedItem.Property.Name,
                            SqlHelper.GetKeyCol(selectedItem.Property.Model));
                        querySql += string.Format("[{0}].[{1}] as [{0}_{1}],",
                           selectedItem.Property.Name,
                           selectedItem.Property.Model.ShowProperty.DBName);
                        tableSql += string.Format(" left outer JOIN [{0}] as [{1}] ON [{1}].[{2}]=[{3}].[{4}] ",
                            selectedItem.Property.Model.DataTableName,
                            selectedItem.Property.Name,
                            SqlHelper.GetKeyCol(selectedItem.Property.Model),
                            model.Model.DataTableName,
                            selectedItem.Property.DBName
                            );
                        if (OrderByItem == selectedItem)
                        {
                            orderby = string.Format("[{0}].[{1}]",
                           selectedItem.Property.Name, selectedItem.Property.Model.ShowProperty.DBName);
                        }
                    }
                }

            }
            if ((model.Model.IdProperty !=null
                &&model.Items.Count(p => p.Property.Name == model.Model.IdProperty.Name) == 0 )
                ||model.Model.AutoSysId == true )
                querySql += string.Format("[{0}].[{1}] as [{0}_{1}],",
                    model.Model.DataTableName, SqlHelper.GetKeyCol(model.Model));



            if (OrderByItem != null )
            {

                //if (OrderByItem.Property.PropertyType == PropertyType.BusinessObject)
                //{
                //    if (OrderByItem.Property.IsMultiMap == false)
                //    {
                //        if (OrderByItem.Property.Model.ShowProperty == OrderByItem.Property.Model.ShowProperty)
                //        {

                //            orderby = OrderByItem.Property.DBName;
                //        }
                //    }
                //    orderby = string.Format("[{0}_{1}]",
                //        OrderByItem.Property.Name,
                //        OrderByItem.Property.Model.ShowProperty.DBName);
                //}
                //else
                //    orderby = string.Format("[{0}_{1}]",
                //        model.Model.DataTableName, OrderByItem.Property.DBName);


            }
            else
            {
                orderby = string.Format("[{0}].[{1}]", model.Model.DataTableName, SqlHelper.GetKeyCol(model.Model));
            }




            //    if (string.IsNullOrEmpty(orderby))

            querySql += "ROW_NUMBER() OVER(ORDER BY " + orderby + " " + orderByType.ToString() + ") AS [QUERY_ROW_INDEX],";
            querySql = querySql.Substring(0, querySql.Length - 1);
            querySql += " FROM " + tableSql;



            string viewfiler = model.Filter;
            if (string.IsNullOrEmpty(viewfiler))
                viewfiler = "1=1";
            querySql += " WHERE " + viewfiler;
            if (String.IsNullOrEmpty(filter) == false)
            {
                var items = model.Items.Where(p => p.ReadOnly);
                if (items.Count() > 0)
                {
                    querySql += " AND (";
                    foreach (var item in model.Items.Where(p => p.ReadOnly))
                    {

                        if (item.Property.PropertyType == PropertyType.BusinessObject)
                        {
                            if (item.Property.Model.ShowProperty != null)
                            {
                                if (item.Property.Model.ShowProperty != item.Property.Model.IdProperty)
                                {// querySql += string.Format("{0} LIKE '%'+@P1+'%' OR ",
                                    // item.Property.Name + "." + item.Property.Model.ShowProperty.DBName);

                                    if (item.Property.Model.ShowProperty != null)
                                    {
                                        if (item.Property.IsMultiMap == false)
                                            querySql += string.Format("{0} LIKE '%'+@P1+'%' OR ",
                                                item.Property.Name + "." + item.Property.Model.ShowProperty.DBName);
                                        else if (item.Property.DBMaps.Count(p => p.PropertyName == item
                                            .Property.Model.ShowProperty.Name) > 0)
                                        {
                                            querySql += string.Format("{0} LIKE '%'+@P1+'%' OR ",
                                         item.Property.Name + "." + item.Property.DBMaps.First(p => p.PropertyName == item.Property.Name).DBColName);
                                        }

                                    }
                                }
                                else
                                {

                                    if (item.Property.IsMultiMap == false)
                                    querySql += string.Format("[{0}].[{1}] LIKE '%'+@P1+'%' OR ",
                                        model.Model.DataTableName,
                                        item.Property.DBName);
                                    else if (item.Property.DBMaps.Count(p => p.PropertyName == item
                                        .Property.Model.ShowProperty.Name) > 0)
                                    {
                                        querySql += string.Format("{0} LIKE '%'+@P1+'%' OR ",
                                     model.Model.DataTableName + "." + item.Property.DBMaps.First(p => p.PropertyName == item.Property.Model.ShowProperty.Name).DBColName);
                                    }

                                }
                            }
                        }
                        else
                            querySql += string.Format("{0} LIKE '%'+@P1+'%' OR ",
                                model.Model.DataTableName + "." + item.Property.DBName);

                    }
                    querySql += " 1=2)";
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@P1", filter));
                }



            }


        
            // System.Diagnostics.Trace.WriteLine(querySql);
            command.CommandText = "SELECT COUNT (*) FROM (" + querySql + ")T";

            result.TotalItemsCount = (int)command.ExecuteScalar();
            if(perPage >0)
            command.CommandText = "SELECT TOP " + perPage.ToString() + " * FROM (" + querySql + ")T  WHERE QUERY_ROW_INDEX >" + (perPage * (COUNT - 1)).ToString() + " ORDER BY QUERY_ROW_INDEX";
                else 
            command.CommandText = "SELECT  * FROM (" + querySql + ")T  WHERE QUERY_ROW_INDEX >" + (20 * (COUNT - 1)).ToString() + " ORDER BY QUERY_ROW_INDEX";
            if (perPage > 0)
                result.TotalPagesCount = result.TotalItemsCount / perPage + (result.TotalItemsCount % perPage > 0 ? 1 : 0);
            else
                result.TotalItemsCount = 1;

            var table = SqlDataLoader.GetSqlData(command);
            result.CurrentResult = new List<ViewResultItem>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                result.CurrentResult.Add( new ViewResultItem(){ObjectProxy=
                    SqlDataLoader.LoadSqlDataQuery(model, table.Rows[i],this.Con ,this.ConFac),
                                                               RowIndex = System.Convert.ToInt64(table.Rows[i]["QUERY_ROW_INDEX"])

                });
            }
            command.Connection.Close();
            return result;
        }
 
        public ListViewQueryContext(SqlCon con,Context.ICurrentContextFactory conFac)
        {

            this.Con = con;
            this.ConFac = conFac;
        }
    
    }
}
