using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.SqlServer
{
    internal class SqlServerCommandCreator{
    //{
    //    public System.Data.IDbCommand GenerateCreatorCommnad(ObjectProxy proxy)
    //    {
    //        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
    //        string values = "\r\n VALUES(";
    //        command.CommandText = "INSERT " + proxy.ArgModel.DataTableName + "(";


    //        var items = GetColsAndValues(proxy, trans);
    //        foreach (var item in items)
    //        {

    //            command.CommandText += "[" + item.Key + "]\r\n,";
    //            values += "@" + item.Key + "\r\n,";
    //            command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + item.Key, item.Value));

    //        }
    //        if (String.IsNullOrEmpty(ParentCol) == false)
    //        {

    //            command.CommandText += "[" + ParentCol + "]\r\n,";
    //            command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + ParentCol, id));
    //            values += "@" + ParentCol + "\r\n,";
    //        }
    //        command.CommandText = command.CommandText.Substring(0, command.CommandText.Length - 1);
    //        command.CommandText += ")\r\n" + values.Substring(0, values.Length - 1) + ")\r\n select @@IDENTITY \r\n";



    //        return command;
    //    }

    //    private Dictionary<String, object> GetColsAndValues(ObjectProxy proxy, System.Data.SqlClient.SqlTransaction trans = null)
    //    {
    //        Dictionary<String, object> result = new Dictionary<string, object>();
    //        ///简单数据
    //        foreach (var property in proxy.ArgModel.Properties.Where(p => p.IsArray == false
    //            && p.AutoGenerationType != Data.Discription.ORM.GenerationType.OnInSert))
    //        {

    //            if (property.PropertyType != PropertyType.BusinessObject)
    //            {
    //                if (String.IsNullOrEmpty((property.DBName ?? "").Replace("[", "").Replace("]", "")) == false)
    //                    result.Add(property.DBName, proxy[property] ?? "");

    //            }
    //            else
    //            {
    //                ObjectProxy propertyProxy = proxy[property] as ObjectProxy;
    //                if (propertyProxy != null)
    //                {
    //                    if (propertyProxy.ID == null)
    //                    {
    //                        //不存在,先加到里面
    //                        if (IsExits(propertyProxy, trans) == false)
    //                        {
    //                            //Create(propertyProxy, trans);
    //                            if (trans != null)
    //                                Create(propertyProxy, trans);
    //                            else
    //                                Create(propertyProxy);
    //                        }
    //                    }
    //                    if (property.IsMultiMap)
    //                    {
    //                        foreach (var propertymaps in property.DBMaps)
    //                        {
    //                            result.Add(propertymaps.DBColName, propertyProxy[propertyProxy.ArgModel.Properties.First(p => p.Name == propertymaps.PropertyName)] ?? "");


    //                        }
    //                    }
    //                    else
    //                    {
    //                        result.Add(property.DBName, propertyProxy.ID);


    //                    }

    //                }
    //            }
    //        }
    //        return result;
    //    }
 
    //    public System.Data.IDbCommand GenerateSaveCommnad();
    //    public System.Data.IDbCommand GenerateDelteCommnad();
    //    public System.Data.IDbCommand GenerateGetCommnad();

    //    public System.Data.IDbCommand GenerateQueryCommnad();

    }
}
