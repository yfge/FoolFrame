using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.DB.MSSQL
{
    //public class SqlServerGeneratoryProxy
    //{

    //    public System.Data.SqlClient.SqlCommand GeneratorCreate(Soway.Model.ObjectProxy proxy)
    //    {

    //        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
    //        command.CommandText = "INSERT " + proxy.Model.DataTableName + "(";
    //        string sqlParams = "VALUES(";
    //        foreach (var property in proxy.Model.Properties.Where(p => p.IsArray == false))
    //        {

    //            if (property.IsMultiMap == false)
    //            {

    //                if (property.AutoGenerationType != Data.Discription.ORM.GenerationType.OnInSert)
    //                {
    //                    command.CommandText += "[" + property.DBName + "],";
    //                    sqlParams += "@" + property.DBName + ",";
                        


    //                }
    //            }
    //            else
    //            {
    //            }
    //        }
    //        return null;
         
    //    }
      

    //}
}
