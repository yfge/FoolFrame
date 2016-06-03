using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data;
using System.Text.RegularExpressions;

namespace Soway.Query
{
    class SqlScriptFac
    {
        public static string GetTableSql(SelectedTables table)
        {
            string sql = GetSelectedTableSql(table.Tables[0]);
            if (table.joins.Count > 0)
            {
                foreach (var joinTable in table.joins)
                {
                    sql += string.Format(" JOIN [{0}] ON 1=1", GetSelectedTableSql(joinTable.RightTable));
                    foreach (var condition in joinTable.Conditions)
                    {
                        sql += string.Format(" AND [{0}].[{1}]=[{2}].[{3}]", joinTable.LeftTable.SelectedTableName,
                            condition.LeftCol,
                            joinTable.RightTable.SelectedTableName,
                            condition.RightCol);
                    }
                }
            }
            return sql;
        }

        private static string GetSelectedTableSql(SelectedTable table)
        {
            return string.Format("[{0}] as [{1}]", table.Table.DBName, table.SelectedTableName);
        }

        public static string GetSelectedColSlq(SelectedCol col )
        {
            var colstr = string.Format(" {0}  AS [{1}]", string.Format(col.SelectType.DBExp, col.SelectedTable.SelectedTableName, col.DataCol.DBName), col.SelectedName);


            if (col.DataCol.DataType != PropertyType.Enum)
                return colstr;
            else
            {

                string TableCol = string.Format(col.SelectType.DBExp, col.SelectedTable.SelectedTableName, col.DataCol.DBName);
                string stateStr = string.Format("(CASE");
                if(col.Values != null )
                foreach (var value in col.Values)
                {
                    stateStr += string.Format(" WHEN {0}={1} THEN '{2}' ", TableCol, value.DBName, value.ShowName);
                }
                stateStr += string.Format(" ELSE '' END) AS [{0}]", col.SelectedName);
                return stateStr;
            }
                  

        }


        public static System.Data.SqlClient.SqlCommand GetSql(QueryInstance ins,string rowIndex="RowIndex")
        {
            var command = new System.Data.SqlClient.SqlCommand();
            string sql = "SELECT distinct ";
            foreach (var col in ins.SelectedCols.OrderBy(p=>p.SelectedIndex))
            {
                sql +=string.Format("{0},", GetSelectedColSlq(col));
           

            }
       //     sql = sql.Substring(0, sql.Length - 1);

             var cols = ins.SelectedCols.Where(p=>p.OrderType != OrderType.NULL).OrderBy(q=>q.SelectedIndex);
             string rowCol = "ROW_NUMBER() OVER  (ORDER BY  ";
            if(cols.Count() >0){
               
                foreach(var col in cols)
                    rowCol += string.Format("{0} {1},",
                      string.Format(col.SelectType.DBExp, col.SelectedTable.SelectedTableName, col.DataCol.DBName),
                        col.OrderType.ToString());
                rowCol = rowCol.Substring(0,rowCol.Length -1);
                
            }else{
                var col = ins.SelectedCols.First();
                rowCol += string.Format("{0} {1}",
                                          string.Format(col.SelectType.DBExp, col.SelectedTable.SelectedTableName, col.DataCol.DBName) ,
                      OrderType.ASC);
            }
            rowCol += string.Format(") AS [{0}]",rowIndex);
            sql += rowCol;
            sql += string.Format(" FROM {0}", GetTableSql(ins.SelectedTables));
            

            if (ins.BoolExp != null && ins.BoolExp.Exp != null)
            {
                var sqlPart = ins.BoolExp.Exp.GetSqlPart(0);
                if (sqlPart != null&&string.IsNullOrEmpty(sqlPart.Stript) == false)
                {
                    sql += " WHERE " + sqlPart.Stript;
                    if(sqlPart.Parameters != null)
                    foreach (var param in sqlPart.Parameters)
                    {
                        command.Parameters.Add(param.SqlParam);
                    }
                   
                        if(ins.ReportParams!= null)
                        {
                            for (int i = 0;i<ins.ReportParams.Count;i++)

                            {

                                string paramName = "@p" + i;
                                if (String.IsNullOrEmpty(ins.ReportParams[i].Exp) == false)
                                    paramName = ins.ReportParams[i].Exp;
                                if (paramName[0] != '@')
                                    paramName = "@" + paramName;
                                if(command.Parameters.IndexOf(paramName)<0)
                            {
  command.Parameters.AddWithValue(
                                   paramName, ins.ReportParams[i].Value);

                            }
                              
                            }
                        }
                   
                } 
                //sql += ins.BoolExp.Exp.GetSqlPart(0).Stript;
            }
           
 

            if (ins.SelectedCols.Count(p => p.SelectType.RequireGroupCol) > 0
                && ins.SelectedCols.Count(p=>p.SelectType.RequireGroupCol ==false )>0)
            {
                string groupStr = " GROUP BY ";
                foreach (var col in ins.SelectedCols.Where(p => p.SelectType.RequireGroupCol == false).OrderBy(p => p.SelectedIndex))
                {
                    groupStr +=
                       string.Format(col.SelectType.DBExp, col.SelectedTable.SelectedTableName, col.DataCol.DBName) + ",";
                    
                }
                groupStr = groupStr.Substring(0,groupStr.Length - 1);
                sql += groupStr;

            }


            command.CommandText = sql;
            // // System.Diagnostics.Trace.WriteLine(sql);
            return command;
        }



        public static QueryResult Result(string conStr,QueryInstance ins,
            int pageSize=20, 
            int startPage =1, 
            string rowIndex = "RowIndex",
            bool inCludeRowIndex=false)
        {
            var command = GetSql(ins, rowIndex);


            string sql = " ";
            foreach (var col in ins.SelectedCols.OrderBy(p => p.SelectedIndex))
            {
                sql += string.Format("[{0}],", col.SelectedName);
            }
            if (inCludeRowIndex)
                sql += string.Format("[{0}],", rowIndex);
            sql = sql.Substring(0, sql.Length - 1);

            command.CommandText = string.Format(@"SELECT COUNT(*) FROM ({0})A
            SELECT {1}  FROM ({0})A WHERE {2}>(@page-1) * @pagesize AND {2}<=@page*@pagesize ORDER BY {2}
            ", command.CommandText, sql, rowIndex);
            command.CommandText = fmtSql(command.CommandText);
            command.Parameters.AddWithValue("@page", startPage);
            command.Parameters.AddWithValue("@pagesize", pageSize);

       


     
            return new QueryResult(conStr, command);

             
           


        }

      
        
        private static string fmtSql(string sql)
        {

            string regStr = @"(\[.+?\[.+?(\])\])";

            Regex regex = new Regex(regStr);
            if (regex.IsMatch(sql))
            {
                string str1 = regex.Replace(sql, "$1]");
                return str1;
            }
            return sql;
        }
        
    }
}
