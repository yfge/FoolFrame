using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public class ModelSqlServerFactory : IModelSqlFactory
    {


        private List<Model> generateCreate = new List<Model>();
        private System.Collections.Hashtable addedSql = new System.Collections.Hashtable();
        public ModelSqlServerFactory(IModuleSource fac)
        {
            this.ModelFactory = fac;
        }


        private string GetArrayDbCol(Property property, Model model)
        {
            if (string.IsNullOrEmpty(property.DBName))
                return property.Name.ToUpper() + "_" + model.DataTableName.ToUpper().Replace("[", "").Replace("]", "") + "_" +
                   (model.AutoSysId ? "SYSID" : model.IdProperty.DBName.ToUpper());
            return property.DBName.ToUpper();
        }
        public System.Data.Common.DbCommand GerateCreateSql(Model Model)
        {

            if (Model.ModelType == ModelType.Enum)
                return null;
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            string sql = "";
            if (Model.IsView == false)
                sql = GenerateCreateTableSql(Model);
            else
                sql = GenerateCreateViewSql(Model);
            command.CommandText = sql;
 

            if(Model.BaseModel != null && Model.BaseModel .IsView ==true )
                command.CommandText += @"
"+ GenerateCreateViewSql(Model.BaseModel);
            return command;



        }

        public System.Data.Common.DbCommand GenerateUpdate(Model Model)
        {

            if (Model.ModelType == ModelType.Enum)
                return null;
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            string sql = "";
            if (Model.IsView == false)
                sql = GenerateAlterTableSql(Model);
            else
                sql = GenerateAlterTableSql(Model);
            command.CommandText = sql;


//            if (Model.BaseModel != null && Model.BaseModel.IsView == true)
//                command.CommandText += @"
//" + GenerateCreateViewSql(Model.BaseModel);
            return command;



        }
        private string  GenerateCreateViewSql(Model Model){
           var childrenModels = this.ModelFactory.GetModels().Where(p=>p.BaseModel == Model &&p.IsView ==false );
           if (childrenModels.Count() == 0)
               return "select 1";
           string sql = "";
           string asSql = "";

           foreach(var model in childrenModels){
               
               string chsql = "SELECT ";
               string emptySql = "SELECT ";
  
               foreach (var property in Model.Properties.Where(p=>p.IsArray==false ))
               {
                   var chidrenProperty = model.Properties.FirstOrDefault(p => p.PropertyName == property.PropertyName || p.Name == property.Name);

                   if (property.IsMultiMap == false && string.IsNullOrEmpty(property.DBName)==false )
                   {
                       if (chidrenProperty == null  || string.IsNullOrEmpty(chidrenProperty.DBName) == true)
                       {
                           chsql += string.Format("{0} AS [{1}],", "NULL", property.DBName);

                       }
                       else
                       {
                           chsql += string.Format("[{0}] AS [{1}],", chidrenProperty.DBName, property.DBName);
                       }

                       emptySql += string.Format("{0} AS [{1}],", "NULL", property.DBName);

                   }
                   else
                   {
                       foreach (var map in property.DBMaps)
                       {

                           var cmap = chidrenProperty.DBMaps.FirstOrDefault(p => p.PropertyName == map.PropertyName);
                           if(cmap != null)
                           { chsql += string.Format("{0} AS [{1}],", "NULL", map.DBColName);
                           emptySql += string.Format("{0} AS [{1}],", "NULL", map.DBColName); 
                           }
                           else
                           {
                               chsql += string.Format("[{0}] AS [{1}],",cmap.DBColName,map.DBColName);
                           }
                       }
                   }
               }
               


                   chsql = chsql.Substring(0, chsql.Length - 1);
                   emptySql = emptySql.Substring(0, emptySql.Length - 1);
                   chsql += string.Format(" FROM {0}", model.DataTableName);
               // set   @p =  (case when  exists(select * from sys.tables where name = 'Customer') then 
//('SELECT [cCusCode] AS [cCode],[cCusName] AS [cName],[cCusAbbName] AS [cAbbName],[CusClass] AS [cCCode] FROM [Customer]')
//else ('select null as cCode,null as cName,null as cAbbName,null as cCode') end)
                   chsql = string.Format(@"set @p =@p+(case when exists(select * from sys.tables where
                        name ='{0}') then ('{1}') else ('{2}') end);", model.DataTableName.Replace("[","").Replace("]",""), chsql, emptySql);


                
                   sql += chsql;
           
                   
           
               if (childrenModels.Last() != model)
                   sql += "set @p = @p +' union ';";
              


           }
           sql = string.Format(@"
                declare @p nvarchar(max);
                set @p = '';
                {0}
                IF NOT EXISTS(SELECT * FROM SYS.all_views WHERE NAME ='{1}')
                BEGIN 
                
                set @p = 'create view {1} as '+@p;
               
                END 
                else 
                
                begin
                set @p = 'alter view {1} as '+@p;
                end 
             exec sp_executesql @p",sql,Model.DataTableName.Replace("[","").Replace("]",""));
           return sql;
 

        }

        private string GenerateCreateTableSql(Model Model)
        {
            string sqlTable = Model.DataTableName.Replace("[", "").Replace("]", "").ToUpper();
            string sql = @"CREATE TABLE " + sqlTable + "\t(";
            var properties = Model.Properties;
            System.Collections.Hashtable hash = new System.Collections.Hashtable();
            string primeKeyGroup = "__" + sqlTable;
            foreach (var property in properties.Where(p => p.IsArray == false))
            {
                if (property.IsArray == false && String.IsNullOrEmpty(property.DBName)
                    && property.IsMultiMap == false)
                    continue;
                sql += "\r\n" + GetSql(property) + ",";

                if (property.IsCheck)
                {
                    string key = "_" + (property.IXGroup ?? "").Trim().ToUpper() + "_" + sqlTable;
                    if (hash.ContainsKey(key) == false)
                        hash.Add(key, new List<String>());
                    (hash[key] as List<String>).Add(property.DBName);

                }
            }
            if (Model.AutoSysId == true)
            {
                sql += "\r\n[SysId] [BIGINT] IDENTITY(1,1) NOT NULL,";
            }
            foreach (string key in hash.Keys)
            {

                
                List<String> items = hash[key] as List<String>;
                if (key == primeKeyGroup && items.Count == 1)
                    sql += "\r\n CONSTRAINT [PK" + key + "]  PRIMARY KEY (";
                else
                    sql += "\r\n CONSTRAINT [IX" + key + "] UNIQUE(";
                foreach (var st in items)
                {
                    sql += st + ",";
                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += "),";
            }

            if (Model.AutoSysId == true)
                sql += "\r\n  CONSTRAINT [PK_SYSID_" + sqlTable + "] PRIMARY KEY ([SysId] ASC)";
            else
                sql = sql.Substring(0, sql.Length - 1);
            sql += " )\r\n";
            sql = "IF(NOT EXISTS(SELECT * FROM SYS.tables WHERE NAME = '" + Model.DataTableName.Replace("[", "").Replace("]", "") + @"'))
BEGIN
" + sql + @"
END";
           
            return sql;
        }

        private string GenerateAlterTableSql(Model Model)
        {
            string sqlTable = Model.DataTableName.Replace("[", "").Replace("]", "").ToUpper();
            string sql = "";
            var properties = Model.Properties;
            System.Collections.Hashtable hash = new System.Collections.Hashtable();
            string primeKeyGroup = "__" + sqlTable;
            foreach (var property in properties.Where(p => p.IsArray == false))
            {
                if (property.IsArray == false && String.IsNullOrEmpty(property.DBName)
                    && property.IsMultiMap == false)
                    continue;

                sql += string.Format(@"IF NOT EXISTS (SELECT * FROM SYS.COLUMNS WHERE 
                        OBJECT_ID IN(SELECT OBJECT_ID FROM SYS.TABLES
                        WHERE NAME ='{0}') AND NAME ='{1}')
                        BEGIN 
                            ALTER TABLE {0} 
                            ADD {2}
                        END ELSE BEGIN 
                            ALTER TABLE {0} 
                            ALTER COLUMN {2}
                        END
                        ", sqlTable, property.DBName,GetSql(property)); 
                //sql += "\r\n" + GetSql(property) + ",";

                //if (property.IsCheck)
                //{
                //    string key = "_" + (property.IXGroup ?? "").Trim().ToUpper() + "_" + sqlTable;
                //    if (hash.ContainsKey(key) == false)
                //        hash.Add(key, new List<String>());
                //    (hash[key] as List<String>).Add(property.DBName);

                //}
            }
            if (Model.AutoSysId == true)
            {
                sql += string.Format(@"IF NOT EXISTS (SELECT * FROM SYS.COLUMNS WHERE 
                        OBJECT_ID IN(SELECT OBJECT_ID FROM SYS.TABLES
                        WHERE NAME ='{0}') AND NAME ='{1}')
                        BEGIN 
                            ALTER TABLE {0} 
                            ADD {2}
                        END
                        ", sqlTable, "SysId", "[SysId] [BIGINT] IDENTITY(1,1) NOT NULL");
                //   sql += "\r\n[SysId] [BIGINT] IDENTITY(1,1) NOT NULL,";
            }
            else
            {
                sql += string.Format(@"IF NOT EXISTS (SELECT * FROM SYS.COLUMNS WHERE 
                        OBJECT_ID IN(SELECT OBJECT_ID FROM SYS.TABLES
                        WHERE NAME ='{0}') AND NAME ='{1}')
                        BEGIN 
                            ALTER TABLE {0} 
                            DROP COLUMN {1}
                        END
                        ", sqlTable, "SysId", "[SysId] [BIGINT] IDENTITY(1,1) NOT NULL");
            }
//            foreach (string key in hash.Keys)
//            {


//                List<String> items = hash[key] as List<String>;
//                if (key == primeKeyGroup && items.Count == 1)
//                    sql += "\r\n CONSTRAINT [PK" + key + "]  PRIMARY KEY (";
//                else
//                    sql += "\r\n CONSTRAINT [IX" + key + "] UNIQUE(";
//                foreach (var st in items)
//                {
//                    sql += st + ",";
//                }
//                sql = sql.Substring(0, sql.Length - 1);
//                sql += "),";
//            }

//            if (Model.AutoSysId == true)
//                sql += "\r\n  CONSTRAINT [PK_SYSID_" + sqlTable + "] PRIMARY KEY ([SysId] ASC)";
//            else
//                sql = sql.Substring(0, sql.Length - 1);
//            sql += " )\r\n";
//            sql = "IF(NOT EXISTS(SELECT * FROM SYS.tables WHERE NAME = '" + Model.DataTableName.Replace("[", "").Replace("]", "") + @"'))
//BEGIN
//" + sql + @"
//END";

            return sql;
        }

        public System.Data.Common.DbCommand GetRelationSql(Relation relation,Model Model=null)
        {

            //System.Diagnostics.Trace.WriteLine(String.Format("Get Relations of :{0} ", relation.Property.Name));
            var dbCommnad = new System.Data.SqlClient.SqlCommand();
            if(Model ==null)           
                Model = this.ModelFactory.GetModels().First(p => p.Properties.Contains(relation.Property));
            if (relation.RelationType == RelationType.One2Many)
            {

                dbCommnad.CommandText =String.Format(@"IF NOT EXISTS (SELECT * FROM SYS.ALL_COLUMNS A JOIN SYS.TABLES 
B ON A.OBJECT_ID = B.OBJECT_ID
AND B.NAME = '{0}' AND A.NAME ='{1}')
BEGIN
ALTER TABLE {0} ADD {1} {2}
END", relation.RelationTable.Replace("[", "").Replace("]", ""), 
    relation.TargetColumn, GetModeKeyTypeSql(Model).Replace("NOT", ""));


            }
            else if (relation.RelationType == RelationType.Recurve || relation.RelationType == RelationType.Many2Many)
            {
                
               dbCommnad .CommandText= string.Format(@"IF NOT EXISTS (SELECT *FROM SYS.TABLES WHERE NAME='{0}' )
BEGIN
CREATE TABLE {0} ({1} {2},{3} {4})
END", relation.RelationTable,
                               relation.PropertyColumn,
                               GetModeKeyTypeSql(Model),
                               relation.TargetColumn,
                               relation.TargetProperty != null ? 
                               GetModeKeyTypeSql(relation.TargetProperty.Model) 
                               : GetSql(PropertyType.Long));
            }

            return dbCommnad;
        }

        private string GetModeKeyTypeSql(Model Model)
        {
            if (Model.AutoSysId == true)
                return GetSql(PropertyType.Long);

            var property = Model.IdProperty.PropertyType;
            if (property == PropertyType.IdentifyId)
                return GetSql(PropertyType.Long);
            else
                return GetSql(property);

            //return (ArgModel.AutoSysId ? 
            //    GetSql(PropertyType.Long) :
            //    GetSql(ArgModel.Properties.First(p => p.Name == ArgModel.IdProperty).PropertyType
            //                            != PropertyType.IdentifyId ?
            //                            ArgModel.Properties.First(p => p.Name == ArgModel.IdProperty).PropertyType 
            //                            : PropertyType.Long));
        }


        private string GenerationAlterViewSql(Model model)
        {
            if (model.IsView)
            {

                var modes = this.ModelFactory.GetModels().Where(p => p.BaseModel == model);
                string sql = string.Format(@"IF(EXISTS(SELECT * FROM SYS.VIEW WHERE NAME ='{0}')) THEN 
    BEGIN 
        {1} 
    END 
        ELSE
    BEGIN
        {2}
    END");
                 
                //if (modes.Count() > 0)
                //{
                //    foreach (var model in modes)
                //    {
                       

                       

                //    }
                //}
                //else
                    return null;
            }
                 
            else return null;
        }

        public IModuleSource ModelFactory
        {
            get;
            private set;
        }

        private string GetSql(PropertyType type,bool IsMulMap=false)
        {
            switch (type)
            {
                case PropertyType.Boolean:
                    return "[BIT] NOT NULL";
                case PropertyType.Byte:
                case PropertyType.Enum:
                    return "[SMALLINT] NOT NULL";
                case PropertyType.Char:
                    return "[NCHAR] NOT NULL";
                case PropertyType.Date:
                    return "[DATE] NOT NULL";
                case PropertyType.DateTime:
                    return "[DATETIME] NOT NULL";
                case PropertyType.Decimal:
                    return "[DECIMAL] (18,2) NOT NULL";
                case PropertyType.Double:
                case PropertyType.Float:
                    return "[FLOAT] NOT NULL";
                case PropertyType.IdentifyId:
                    if(IsMulMap==false)
                    return "[BIGINT] IDENTITY(1,1) NOT NULL";
                    else
                        return "[BIGINT] NOT NULL";
                case PropertyType.Int:
                case PropertyType.UInt:
                    return "[INT] NOT NULL";
                case PropertyType.Long:
                case PropertyType.ULong:
                    return "[BIGINT] NOT NULL";
                case PropertyType.String:
                case PropertyType.SerialNo:
                    return "[NVARCHAR](200) NOT NULL";
                case PropertyType.Time:
                    return "[TIME] NOT NULL";
                case PropertyType.MD5:
                    return "[NVARCHAR](200) NOT NULL";
                case PropertyType.Guid:
                    return "[uniqueidentifier] NOT NULL ";
                   
                default:
                    return "";
            }


        }

        private string GetSql(Property property)
        {

            if (property.PropertyType == PropertyType.RadomDECS)
            {
                return @String.Format(@"[{0}{2}] {1} NOT NULL,
                                        [{0}{3}] {1} NOT NULL,
                                        [{0}{4}] {1} NOT NULL,
                                        [{0}{5}] {1} NOT NULL,
                                        [{0}{6}] VARCHAR(200) NOT NULL", 
                                       property.DBName,
                                       "[varbinary](8)",
                                       EncryptionClass.IVStr,
                                       EncryptionClass.IVIndexStr,
                                       EncryptionClass.ENStr,
                                       EncryptionClass.ENIndexStr,
                                       EncryptionClass.PwdStr);
            }
            if (property.PropertyType != PropertyType.BusinessObject )
                return "[" + property.DBName + "]\t" + GetSql(property.PropertyType).Replace("NOT", property.AllowDBNull ? "" : "NOT");
            else if (property.IsMultiMap)
            {
                var properties = property.Model.Properties;
                string sql = "";
                foreach (var map in property.DBMaps)
                {
                    sql += "[" + map.DBColName + "]\t" + GetSql(properties.FirstOrDefault(p => p.PropertyName == map.PropertyName).PropertyType,true) + ",\r\n";
                }
                return sql.Substring(0, sql.Length - 3).Replace("NOT", property.AllowDBNull ? "" : "NOT");
            }
            else
            {
                var P = property.Model.Properties.FirstOrDefault(
                    p => string.IsNullOrEmpty(p.IXGroup) && p.IsCheck);

                return "[" + property.DBName + "]\t" + GetModeKeyTypeSql(property.Model).Replace("NOT", property.AllowDBNull ? "" : "NOT");
            }


        }

        IModelFactory IModelSqlFactory.ModelFactory
        {
            get { throw new NotImplementedException(); }
        }



    }

}
