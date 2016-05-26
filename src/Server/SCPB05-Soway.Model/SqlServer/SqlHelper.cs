using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.SqlServer
{
    internal class SqlHelper
    {
        public static System.Data.SqlClient.SqlCommand 
            generatGetItemsCommand(Property property, Model parentMode, object id)
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.CommandText = "SELECT distinct ";
            foreach (var col in GetSelectCols(property.Model))
                command.CommandText += "[" + col + "],";

            var relation = parentMode.Relations.FirstOrDefault(p => p.Property == property);
            command.CommandText = command.CommandText.Substring(0, command.CommandText.Length - 1);


            command.CommandText += " FROM " + property.Model.DataTableName;
            if (relation != null)
            {
                if (relation.RelationType == RelationType.Recurve)
                {

                    command.CommandText += string.Format(@" JOIN {0} ON {1}={2} AND {3}=@{2} ",
                        relation.RelationTable, relation.TargetColumn, GetKeyCol(parentMode), relation.PropertyColumn);


                }
                else if (relation.RelationType == RelationType.Many2Many)
                {
                    command.CommandText += string.Format(@" JOIN {0} ON {1}={2} and {3} =@{4}",
                        relation.RelationTable, relation.PropertyColumn, GetKeyCol(property.Model),
                       relation.TargetColumn, GetKeyCol(parentMode));
                }
                else
                {
                    if (String.IsNullOrEmpty(relation.TargetColumn))
                    {


                    }
                    command.CommandText += string.Format(@" WHERE {0}=@{1}", relation.TargetColumn, GetKeyCol(parentMode));

                }
            }
            command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + GetKeyCol(parentMode), id));
            //// System.Diagnostics.Trace.WriteLine(command.CommandText);
            return command;
        }
        public static List<String> GetSelectCols(Model mode)
        {
            List<String> result = new List<string>();
            foreach (var property in mode.Properties.Where(p => p.IsArray == false && p.AutoGenerationType != Data.Discription.ORM.GenerationType.OnInSert))
            {

                if (property.PropertyType == PropertyType.RadomDECS)
                {
                    result.AddRange(EncryptionClass.GetPropertyCols(property));
                }
                else
                {
                    if (String.IsNullOrEmpty(property.DBName) == false)
                        result.Add(property.DBName);
                    result.AddRange(property.DBMaps.Where(p => String.IsNullOrEmpty(p.DBColName) == false).Select(q => q.DBColName));
                }
            }
            var keyCol = GetKeyCol(mode);
            if (result.Contains(keyCol) == false)
                result.Add(keyCol);
            return result;

        }

        internal static SqlCon GetSqlCon(Property index, SqlCon con, Model model)
        {

            if (index != null &&index.PropertySqlCon != null)
                return index.PropertySqlCon;
            else
                return GetSqlCon(con, model);
        }

        public static string GetKeyCol(Model model)
        {
            if (model.AutoSysId == true && model.IdProperty == null)
                return "SYSID";
            else
            {
                return model.IdProperty.DBName;
            }
        }

        public static string [] GetKeyCols(Model model)
        {
            List<String> result = new List<string>();
            if (model.AutoSysId == true && model.IdProperty == null)
                result.Add("SYSID");
            else
                result.Add(model.IdProperty.DBName);
            foreach(var property in model.Properties .Where(p=>p.IsCheck == true).GroupBy(p => p.IXGroup).Where(q=>q.Count()==1))
            {

                if(result.Contains(property.First().DBName)==false)
                result.Add(property.First().DBName);

            }
            return result.ToArray();
        }


        internal static  SqlCon GetSqlCon(SqlCon con,Model model)
        {
            if (con != null)
                return con;

            if (model != null)
            {
                if (model.SqlCon != null)
                    return model.SqlCon;
                else if (model.Module.SqlCon != null)
                    return model.Module.SqlCon;

            }
            if (con != null)
                return con;


            //if (Global.SqlCon != null)
            //        return Global.SqlCon;
            return null;
        }

        public static  System.Data.SqlClient.SqlCommand
    GenerateGetItemsCommand(Model mode)
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.CommandText = "SELECT ";
            foreach (var col in SqlHelper.GetSelectCols(mode))
                command.CommandText += "[" + col + "],";
            command.CommandText = command.CommandText.Substring(0, command.CommandText.Length - 1);
            command.CommandText += " FROM " + mode.DataTableName;
            return command;
        }





    }
}
