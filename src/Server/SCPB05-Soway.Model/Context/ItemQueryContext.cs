using Soway.Data;
using Soway.Model.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model
{
    /// <summary>
    /// 单独的选择列
    /// </summary>
    public class ItemQueryContext
    {

        /// <summary>
        /// 总记录
        /// </summary>
        public long TotalRecord { get; internal set; }
        /// <summary>
        /// 当前的记录
        /// </summary>
        public long CurrentIndex { get; internal set; }


        /// <summary>
        /// 当前的记录
        /// </summary>
        public IObjectProxy Current { get; private set; }

        /// <summary>
        /// 下一个
        /// </summary>
        /// <returns></returns>
        public bool Next() {

            this.CurrentIndex++;
            return Query();

        }


        public View.View ListView { get; private set; }

        /// <summary>
        /// 前一个
        /// </summary>
        /// <returns></returns>
        public bool Previous() {

            this.CurrentIndex--;
            return this.Query();
        }


        public QueryContext QueryContext { get; set; }




        public ItemQueryContext(Soway.Model.View.View  listView,QueryContext query,long current,SqlCon con,Context.ICurrentContextFactory conFac ) {
            this.ListView = listView;
            this.QueryContext = query;
            this.CurrentIndex = current;
            this.Con = con;
            this.ConFac = conFac;
            Query();
        
        }
        private SqlCon Con { get; set; }
        private ICurrentContextFactory ConFac { get;  set; }

        public ItemQueryContext(Soway.Model.View.View view, IObjectProxy current)
        {
            this.ListView = view;
            this.QueryContext = null;
            this.Current = current;
            this.CurrentIndex = 1;
            this.TotalRecord = 1;
        }

        private bool Query()
        {



            string orderby = "";

            List<String> select = new List<string>();

            string querySql = "";
            var tableSql = "";
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.Connection = new System.Data.SqlClient.SqlConnection(SqlHelper.GetSqlCon(this.Con,this.ListView.Model).ToString());
            command.Connection.Open();
            querySql = "SELECT ";
            tableSql = this.ListView.Model.DataTableName;
            List<String> keys = new List<string>();
            foreach (var selectedItem in this.ListView.Items)
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


                            querySql += string.Format("[{0}].[{1}] as [{0}_{1}],", this.ListView.Model.DataTableName, selectedItem.Property.DBName);
                            if (this.QueryContext.OderByItem == selectedItem)
                             orderby = string.Format("[{0}].[{1}]", this.ListView.Model.DataTableName, selectedItem.Property.DBName);
                        }
                        else
                        {
                            var cols = EncryptionClass.GetPropertyCols(selectedItem.Property);
                            foreach (var col in cols)
                                querySql += string.Format("[{0}].[{1}] as [{0}_{1}],", this.ListView.Model.DataTableName, col);

                        }
                    }
                }
                else if (selectedItem.Property.IsMultiMap)
                {
                    //如果为多重映射
                    foreach (var db in selectedItem.Property.DBMaps)
                    {
                        querySql += string.Format("[{0}].[{1}] as [{2}_{3}],",
                         this.ListView.Model.DataTableName,
                         db.DBColName,
                         selectedItem.Property.Name,
                         db.PropertyName);



                    }

                    if (this.QueryContext.OderByItem == selectedItem &&
                       selectedItem.Property.Model.ShowProperty != null
                        && selectedItem.Property.DBMaps.Count(p => p.PropertyName == selectedItem.Property.Model.ShowProperty.Name) > 0)
                    {
                        orderby = string.Format("[{0}].[{1}]",
                         this.ListView.Model.DataTableName,
                         selectedItem.Property.DBMaps.First(p => p.PropertyName == selectedItem.Property.Model.ShowProperty.Name).DBColName);// ,
                    }
                }
                else
                {
                    //不为多重映射
                    if (selectedItem.Property.Model.IdProperty != null
                        && selectedItem.Property.Model.IdProperty == selectedItem.Property.Model.ShowProperty)
                    {
                        //这里还没有搞定 
                        //主键就是要显示的值 ,直接忽略
                        querySql += string.Format("[{0}].[{1}] as   [{2}_{3}],",
                            this.ListView.Model.DataTableName,
                            selectedItem.Property.DBName,
                            selectedItem.Property.Name,
                            selectedItem.Property.Model.ShowProperty.DBName);

                        if (selectedItem == this.QueryContext.OderByItem)
                            orderby = string.Format("[{0}].[{1}]",
                            this.ListView.Model.DataTableName,
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
                        tableSql += string.Format(" left outer JOIN [{0}] as [{1}] ON {1}.{2}={3}.{4} ",
                            selectedItem.Property.Model.DataTableName,
                            selectedItem.Property.Name,
                            SqlHelper.GetKeyCol(selectedItem.Property.Model),
                            this.ListView.Model.DataTableName,
                            selectedItem.Property.DBName
                            );
                        if (this.QueryContext.OderByItem == selectedItem)
                        {
                            orderby = string.Format("[{0}].[{1}]",
                           selectedItem.Property.Name,
                           selectedItem.Property.Model.ShowProperty.DBName);
                        }
                    }
                }


            }
            if ((ListView.Model.IdProperty != null&&
                ListView.Items.Count(p => p.Property.Name == ListView.Model.IdProperty.Name) == 0)
                ||ListView.Model.AutoSysId ==true)
                querySql += string.Format("[{0}].[{1}] as [{0}_{1}],", 
                    ListView.Model.DataTableName, 
                    SqlHelper.GetKeyCol(this.ListView.Model));


          //  string orderby = "";



            if (this.QueryContext.OderByItem != null)
            {

                //if (QueryContext.OderByItem.Property.PropertyType == PropertyType.BusinessObject)
                //    orderby = string.Format("[{0}].[{1}]",
                //        QueryContext.OderByItem.Property.Name,
                //        QueryContext.OderByItem.Property.Model.ShowProperty.DBName);
                //else
                //    orderby = string.Format("[{0}].[{1}]",
                //        this.ListView.Model.DataTableName, QueryContext.OderByItem.Property.DBName);


            }
            else
            {
                orderby = string.Format("[{0}].[{1}]", this.ListView.Model.DataTableName, SqlHelper.GetKeyCol(this.ListView.Model));
            }

            querySql += "ROW_NUMBER() OVER(ORDER BY " + orderby + " " + this.QueryContext.OrderByType.ToString() + ") AS [QUERY_ROW_INDEX],";
            querySql = querySql.Substring(0, querySql.Length - 1);
            querySql += " FROM " + tableSql;

            

            string viewfiler = this.ListView.Filter;
            if (string.IsNullOrEmpty(viewfiler))
                viewfiler = "1=1";
            querySql += " WHERE " + viewfiler;


            if (String.IsNullOrEmpty(this.QueryContext.Filter) == false)
            {
                var items = this.ListView.Items.Where(p => p.ReadOnly);
                if (items.Count() > 0)
                {
                    querySql += " AND (";
                    foreach (var item in this.ListView.Items.Where(p => p.ReadOnly))
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
                                            this.ListView.Model.DataTableName,
                                            item.Property.DBName);
                                    else if (item.Property.DBMaps.Count(p => p.PropertyName == item
                                        .Property.Model.ShowProperty.Name) > 0)
                                    {
                                        querySql += string.Format("{0} LIKE '%'+@P1+'%' OR ",
                                     this.ListView.Model.DataTableName + "." + item.Property.DBMaps.First(p => p.PropertyName == item.Property.Model.ShowProperty.Name).DBColName);
                                    }

                                }
                            }
                        }
                        else
                            querySql += string.Format("{0} LIKE '%'+@P1+'%' OR ",
                                this.ListView.Model.DataTableName + "." + item.Property.DBName);

                    }
                    querySql += " 1=2)";
                    command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@P1", this.QueryContext.Filter));
                }
            }


            /////// System.Diagnostics.Trace.WriteLine(querySql);
            command.CommandText = "SELECT COUNT (*) FROM (" + querySql + ")T";
          
            this.TotalRecord= (int)command.ExecuteScalar();
            if(this.CurrentIndex >this.TotalRecord )
                return false;
            command.CommandText = "SELECT  "+
                string.Format("[{0}_{1}] ", ListView.Model.DataTableName, SqlHelper.GetKeyCol(this.ListView.Model))+" FROM (" + querySql + ")T  WHERE QUERY_ROW_INDEX ='"+ this.CurrentIndex.ToString()+"'";
 
            var id = command.ExecuteScalar();
            /////// System.Diagnostics.Trace.WriteLine(String.Format(" Item ID:{0}", id));
            this.Current = new dbContext(SqlHelper.GetSqlCon(this.Con,this.ListView.Model),this.ConFac).GetDetail
                (this.ListView.Model,id);
            return true;
        }








    }
}
