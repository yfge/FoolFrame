using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.SqlServer
{

    /// <summary>
    /// 将数据从DataTable加载到ObjectProxy
    /// </summary>
    class SqlDataLoader
    {

 
        public static IObjectProxy getProxy(Model mode, object id,SqlCon con,Context.ICurrentContextFactory conFac)
        {
            if (id == null)
                return null;

            var ob = MemoryCache.StoreDbCache.GetDbMemoryCache(con, mode, id,conFac);
            if(ob== null)
            {
                var proxy = new SqlDataProxy(mode, conFac, LoadType.Null);
                MemoryCache.StoreDbCache.UpdateOrAddDbMemoryCache(con, mode, id, proxy);
                return proxy;
            }

            return ob;

        }
       public static void   LoadSqlData(IObjectProxy proxy, System.Data.DataRow row,SqlCon con,Context.ICurrentContextFactory conFac)
        {
           
           if(proxy != null&&proxy.Model.ModelType != ModelType.Enum)
            foreach (var property in proxy.Model.Properties.Where(p => p.IsArray == false &&
                (string.IsNullOrEmpty(p.DBName)==false ||p.IsMultiMap==true )))
            {
                dynamic propertyOb = null;
                if (property.PropertyType == PropertyType.RadomDECS)
                {
                    propertyOb = EncryptionClass.GetDecrptyString(property, row);

                }
                else
                    if (property.PropertyType != PropertyType.BusinessObject)
                    {

                        if (String.IsNullOrEmpty((property.DBName??"").Trim()) == false
                            &&row.Table.Columns.Contains(property.DBName)==true
                            && row[property.DBName] != DBNull.Value)
                            propertyOb = row[property.DBName];
                        else
                            propertyOb = GetDefaultValue(property);
                    }
                    else
                    {
                        IObjectProxy tempProxy = null;/// new SqlDataProxy(property.Model,conFac, LoadType.Null,con);
                        if (property.IsMultiMap == false)
                        {
                            if (String.IsNullOrEmpty(property.DBName) == false && row[property.DBName] != DBNull.Value)
                            {
                                tempProxy = getProxy(property.Model, row[property.DBName],con,conFac);

                            }
                            else
                                tempProxy = null;
                        }
                        else
                        {


                            var modelKeyProperty = property.Model.IdProperty;
                            if (modelKeyProperty != null)
                            {
                                var itemKeyProperrty = property.DBMaps.FirstOrDefault(p => p.PropertyName == modelKeyProperty.PropertyName);
                                if (itemKeyProperrty != null)
                                {
                                    var rowOb = row[itemKeyProperrty.DBColName];
                                    if (rowOb != DBNull.Value)
                                    {
                                        tempProxy = getProxy(property.Model, rowOb,con,conFac);

                                        tempProxy.IsLoad =  LoadType.Null ;
                                    }
                                }
                            }

                            bool IsSet = false;
                            foreach (var map in property.DBMaps)
                            {
                                var rowOb = row[map.DBColName];
                                if (rowOb != DBNull.Value)
                                {
                                    tempProxy[tempProxy.Model.Properties.First(p => p.Name == map.PropertyName
                                        || p.PropertyName == map.PropertyName)] = rowOb;

                                    IsSet = true;
                                }
                            }
                            if (IsSet == false)
                                tempProxy = null;
                            else 
                            tempProxy.IsLoad = LoadType.Partial;

                        }
                        propertyOb = tempProxy;
                    }


                    proxy[property] = propertyOb;
                    proxy.UpdateToNew(property);
            }

           if (proxy.ID == null ||(proxy.ID is long &&System.Convert.ToInt64(proxy.ID)==0))
           {
               if (proxy.Model.AutoSysId == false && proxy.Model.IdProperty != null)
                   proxy.ID = proxy[proxy.Model.IdProperty];
               else
                   proxy.ID = row["SysId"];

          

               
           }


           

            MemoryCache.StoreDbCache.UpdateOrAddDbMemoryCache(con, proxy.Model, proxy.ID, proxy); 
           // System.Diagnostics.Trace.WriteLine( "Id:" + proxy.ID);
        }

       private static  object GetDefaultValue(Property property)
       {
           switch (property.PropertyType)
           {
               case PropertyType.Boolean:
                   return false;

               case PropertyType.Byte:

                   decimal a = 0;
                   return a;
               case PropertyType.Char:
                   char c = '\0';
                   return c;
               case PropertyType.Date:

               case PropertyType.DateTime:
                   return null;
               case PropertyType.Decimal:
                   decimal d = 0;
                   return d;
               case PropertyType.Double:
                   double e = 0;
                   return e;
               case PropertyType.Enum:

                   int enumI = property.Model.EnumValues.First().Value;
                   return enumI;
               case PropertyType.Float:
                   float f = 0;
                   return f;
               case PropertyType.Int:
                   int intresult = 0;
                   return intresult;
               case PropertyType.Long:
               case PropertyType.IdentifyId:
                   long longresult = 0;
                   return longresult;
               case PropertyType.String:
               case PropertyType.SerialNo:
                   return "";
               case PropertyType.Time:
                   return DateTime.MinValue;
               case PropertyType.UInt:
                   uint uintResult = 0;
                   return uintResult;
               case PropertyType.ULong:
                   ulong ulongResult = 0;
                   return ulongResult;
               default:
                   return null;
           }


       }
        
       public static IObjectProxy LoadSqlDataQuery(Soway.Model.View.View view,
           System.Data.DataRow row,SqlCon con,Context.ICurrentContextFactory conFac){
           IObjectProxy proxy = new SqlDataProxy(view.Model,conFac,LoadType.Null,con);

           foreach (var viewItem in view.Items)
           {
               if(viewItem.Property .PropertyType == PropertyType.RadomDECS){
                   proxy [viewItem.Property] = EncryptionClass.GetDecrptyString(
                       viewItem.Property,row,string.Format("{0}_",proxy.Model.DataTableName));
               }else
               if (viewItem.Property.PropertyType != PropertyType.BusinessObject)
               {
                   if(string.IsNullOrEmpty(viewItem.Property.DBName)==false )
                   proxy[viewItem.Property]=row[string.Format("{0}_{1}",proxy.Model.DataTableName,viewItem.Property.DBName)];
               }else {

                   var propertyProxy = new SqlDataProxy(viewItem.Property.Model,conFac, LoadType.Null,con);


                   if (viewItem.Property.IsMultiMap == false)
                   {
                       if (viewItem.Property.Model.ShowProperty != null
                           &&
                           row[string.Format("{0}_{1}", viewItem.Property.Name, viewItem.Property.Model.ShowProperty.DBName)]
                           != DBNull.Value)
                       {
                           if (viewItem.Property.Model.IdProperty != null)
                               propertyProxy[viewItem.Property.Model.IdProperty] = row[string.Format("{0}_{1}", viewItem.Property.Name, viewItem.Property.Model.IdProperty.DBName)];
                           if (viewItem.Property.Model.ShowProperty != null)
                               propertyProxy[viewItem.Property.Model.ShowProperty] = row[string.Format("{0}_{1}", viewItem.Property.Name, viewItem.Property.Model.ShowProperty.DBName)];
                       }
                       else
                       {
                          
                           propertyProxy = null;
                          

                       }
                   }
                   else
                   {
                       foreach (var map in viewItem.Property.DBMaps)
                       {
                           propertyProxy[viewItem.Property.Model.Properties.First(p => p.Name == map.PropertyName||p.PropertyName == map.PropertyName)] = row[string.Format("{0}_{1}",
                              viewItem.Property.Name ,
                              map.PropertyName)];
                       }

                   }
                       
                   proxy[viewItem.Property]=propertyProxy;
               }

           }
           if (proxy.Model.AutoSysId == true)
               proxy.ID =row[string.Format("{0}_{1}", proxy.Model.DataTableName, "SYSID")];
           else
               proxy.ID = row[string.Format("{0}_{1}", proxy.Model.DataTableName, proxy.Model.IdProperty.DBName)];
           return proxy;        
       }



       public static  System.Data.DataTable GetSqlData(System.Data.SqlClient.SqlCommand command)
       {
           System.Data.DataTable table = new System.Data.DataTable();
           System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(command);
           adapter.Fill(table);
           return table;
       }

    }
}
