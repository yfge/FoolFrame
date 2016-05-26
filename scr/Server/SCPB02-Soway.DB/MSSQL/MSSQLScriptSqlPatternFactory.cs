using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Soway.Data.Discription.ORM;

namespace Soway.DB.MSSQL
{
    /// <summary>
    /// 一个辅助类，用于生成Sql语句的各个部分
    /// </summary>
    class MSSQLScriptSqlPatternFactory
    {


       


 

 

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Properties"></param>
    /// <param name="ob"></param>
    /// <param name="IsIncludeAutoGerateCols">在查询时是否包含自动生成列</param>
    /// <param name="ParamNamePre"></param>
    /// <returns></returns>
        public SqlCommandPaterns  GetExistSql<T>(System.Reflection.PropertyInfo[] Properties, T ob,
            bool IsIncludeAutoGerateCols = true,String ParamNamePre=null)
        {
             
            SqlCommandPaterns pataerns = new SqlCommandPaterns();
           
            //如果有主键，则生成键查找的条件，如果没有，则生成所有列的查询
            Hashtable hashTable = new Hashtable();
            hashTable.Add(0,""); //
            String ColName  = "";
            String parameterName = "";
            var helper = new ORMHelper();
            for (int i = 0; i < Properties.Length; i++)
            {
                var attribute = helper.GetColNameAttributes(Properties[i]);
                object keyName = 0;
                ColName = Properties[i].Name ;
                foreach(var Col in attribute )
                {
                    if (Col.NoMap)
                        continue;
                      
                    ColName =helper. GetColName(Col,Properties[i]);
                    
                    
                    parameterName = "@"
                        + ParamNamePre + ColName;
                  
                    if (IsIncludeAutoGerateCols == false && (Col.IsAutoGenerate!= GenerationType.Never))
                        continue;
                    if (Col.IsKey)
                    {

                        keyName = Col.KeyGroupName;
                        keyName = keyName ?? 1;
                        if (!hashTable.ContainsKey(keyName))
                        {
                            //如果不含主键组，则加入
                            hashTable.Add(keyName, "");
                        }
                        if (hashTable.ContainsKey(0))
                        {
                            //有主键了就不需要对所有的值进行判断了
                            hashTable.Remove(0);
                        }
                    }
                    else
                        keyName = 0;
          
                 if (hashTable.ContainsKey(keyName))
                {
                    var dbValue = new ORMHelper().GetDbObject(Properties[i], ob);
                    if (dbValue != null )
                    {
                      
                        pataerns.Params.Add(new System.Data.SqlClient.SqlParameter(parameterName,
                            dbValue));
                        string exp = "[" + ColName + "]=" + parameterName;
                        hashTable[keyName] = hashTable[keyName].ToString() + " AND [" + (ColName) + "]=" + parameterName;
                    }
                }
                }
                  
            }
            foreach (var item in hashTable.Values)
            {
                pataerns.SqlScript +="OR ("+item.ToString().Substring(4)+")";
            }
            pataerns.SqlScript=pataerns.SqlScript.Substring(3);
            return pataerns;


       
        }


 
        
    }
}
