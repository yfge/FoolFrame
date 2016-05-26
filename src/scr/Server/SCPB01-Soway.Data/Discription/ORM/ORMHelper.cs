using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
namespace Soway.Data.Discription.ORM
{


    /// <summary>
    /// 一个辅助类，用于得到类及属性的数据库信息
    /// </summary>
    public class ORMHelper
    {
        private static Hashtable typeHash;
        private static Hashtable propertyHash;


        static ORMHelper()
        {
            typeHash = new Hashtable();
            propertyHash = new Hashtable();
        }



        /// <summary>
        /// 得到一个类的被标记为主键的属性
        /// </summary>
        /// <param name="TargetType">类的类型</param>
        /// <returns>该类的主键属性</returns>
        public PropertyInfo GetKeyPropertyInfo(Type TargetType)
        {
            var Properties = TargetType.GetProperties();

            for (int i = 0; i < Properties.Length; i++)
            {
                var attribute = Properties[i].GetCustomAttributes(typeof(ColumnAttribute), true);
                if (attribute.Length > 0)
                {
                    var Col = (ColumnAttribute)attribute[0];
                    if (Col.NoMap)
                        continue;

                    if (Col.IsKey && String.IsNullOrEmpty(Col.KeyGroupName))
                    {



                        return Properties[i];
                    }
                }


            }
            return null;
        }



        /// <summary>
        /// 得到一个属性的值
        /// </summary>
        /// <param name="info">属性值</param>
        /// <param name="ob">目标的对象</param>
        /// <param name="propertyName">当info.Property为复杂类型时，ProertyName为得到对应复杂类型的属性名称</param>
        /// <returns>属性的值</returns>
        public object GetDbObject(PropertyInfo info, object ob, string propertyName = null)
        {




            object returnvalue=null;

            var propertyValue = info.GetValue(ob, new object[] { });
            if (propertyValue != null)
            {

                if (propertyValue.GetType().IsEnum)
                    returnvalue= (int)propertyValue;
                else
                if (propertyName != null)
                {
                    returnvalue= propertyValue.GetType().GetProperty(propertyName).GetValue(propertyValue, new object[] { });
                }else
                if (info.PropertyType.GetCustomAttributes(typeof(TableAttribute), true).Length > 0)
                {

                    returnvalue =GetKeyPropertyInfo(info.PropertyType).GetValue(propertyValue, new object[] { });
                }
                else returnvalue= propertyValue;
            }

            if (returnvalue is DateTime &&( (DateTime)returnvalue == DateTime.MinValue))
                returnvalue = DBNull.Value;
             
            return returnvalue;
        }


        /// <summary>
        /// 判断一个类型是否为实体化的类型
        /// </summary>
        /// <param name="info">要判断的类型</param>
        /// <returns>
        /// 如果该类型标记了Table属性，返回真，否则返回假</returns>
        public bool IsBusinessType(Type info)
        {
            if (info.IsArray)
                return false;
            return info.GetCustomAttributes(typeof(TableAttribute), true).Length > 0;
        }


        public void SetDbObject(PropertyInfo info, object ob, object value)
        {



            //if (propertyValue != null)
            //{
            if (value != DBNull.Value)
                if (info.PropertyType.GetCustomAttributes(typeof(TableAttribute), true).Length > 0)
                {


                    var member = info.PropertyType.Assembly.CreateInstance(info.PropertyType.FullName);
                    GetKeyPropertyInfo(info.PropertyType).SetValue(member, value is DBNull ? "" : value, new object[] { });
                    info.SetValue(ob, member, new object[] { });


                }
                else
                    info.SetValue(ob, value, new object[] { });
            //}

        }



        /// <summary>
        /// 得到一个类型在数据库存储中对应的表名
        /// </summary>
        /// <param name="TargetType"></param>
        /// <returns></returns>
        public string GetTable(Type TargetType, string preStr = "")
        {

            return "[" + preStr + GetTableAttribute(TargetType).Name + "]";
        }



        /// <summary>
        /// 得到一个类型的TableAttribule
        /// </summary>
        /// <param name="TargetType"></param>
        /// <returns></returns>
        public TableAttribute GetTableAttribute(Type TargetType)
        {
            TableAttribute TableAttr;
            if (!typeHash.Contains(TargetType))
            {
                var table = (TableAttribute)Attribute.GetCustomAttribute(TargetType, typeof(TableAttribute), true);
                if (table == null)
                    table = new TableAttribute() { Name = "", ColPreStr = "" };
                typeHash.Add(TargetType, table);


            }
            return typeHash[TargetType] as TableAttribute;
        }

        /// <summary>
        /// 得到一个属性中列类型
        /// </summary>
        /// <param name="attri">要得到的类型Col定义</param>
        /// <param name="property">属性</param>
        /// <returns></returns>
        public String GetColName(ColumnAttribute attri, System.Reflection.PropertyInfo property)
        {


            var table = GetTableAttribute(property.ReflectedType);
            var index = attri.PreIndex;
            var len = attri.PreLen;

            if (index == -1 || String.IsNullOrEmpty(table.ColPreStr) ||attri.OverideParent ==true )
                return attri.ColumnName;
            else
            {
                var str = table.ColPreStr;
                if (len != 0)
                    str = str.Substring(0, len);
                return attri.ColumnName.Insert(index, str);
            }

        }

        /// <summary>
        /// 得到一个类型的的属性的列名
        /// </summary>
        /// <param name="type"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public string GetColName(Type type, String PropertyName)
        {


            var property = type.GetProperty(PropertyName);
            var cols = GetColNameAttributes(property);


            if (cols.Count ==1 )
                return GetColName(cols[0],property);
            else if(cols.Count > 1)
            {
                var key = this.GetKeyPropertyInfo(property.PropertyType);
                var col = cols.FirstOrDefault(p => p.PropertyName == key.Name);
                if (col != null)
                    return GetColName(col, property);
       
                  
            }
         　
                return property.Name;
        }

        /// <summary>
        /// 得到一个属性的Col标记
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public List<ColumnAttribute> GetColNameAttributes(System.Reflection.PropertyInfo property)
        {

            ColumnAttribute colAttr;
            if (propertyHash.Contains(property) == false)
            {
                var attribute = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                List<ColumnAttribute> col = new List<ColumnAttribute>();

                if (attribute.Length > 0)
                {
                    foreach (var attr in attribute)
                        col.Add(attr as ColumnAttribute);
                }
                else
                {

                    col.Add(new ColumnAttribute() { ColumnName = property.Name,NoMap=true });
                }
                propertyHash.Add(property, col);


            }

            return propertyHash[property] as List<ColumnAttribute>;

        }

    }
}
