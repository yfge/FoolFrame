using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    /// <summary>
    /// 用于标记属性以与数据库中的列对应
    /// </summary>
    /// <remarks>
    /// 在当前的版本中，这是一个允许多重映射的特性，即为了数据库中的冗余存储作考虑
    /// 见如下例子
    /// <example>
    /// <code>
    /// 
    /// [Table(Name="A")]
    /// public class A{
    /// [Column(IsKey =true)]
    /// public int Id{get;set;}
    /// public string Str1{get;set;}
    /// public string Str2{get;set;}
    /// }
    /// 
    /// //当构造B时，会从表A中查询相应的值
    /// public class B{
    /// [Column(ColName="B_A")] //Store testA.Id in Column B_A
    /// public A testA{get;set;}
    /// }
    /// 
    /// //当构造C时，不会查询表A，testA的所有属性由表C中的列进行赋值
    /// public class C{
    /// [Column(ColName="C_AId",PropertyName="Id")] //Store testA.Id in Colunm C_AId
    /// [Column(ColName="C_AStr1",PropertyName="Str1")]//Store testA.Str1 in Column C_AStr1
    /// [Column(ColNmae="C_AStr2",PropertyName="Str2")]//Store testA.Str2 in Column C_AStr2
    /// public A testA{get;set;}
    /// }
    /// </code></example>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=true)]
    public class ColumnAttribute :Attribute 
    {
        public bool KeyCanBeNullOrEmpty { get; set; }

        public ColumnAttribute()
        {
            EncrpytType = EncryptType.NoEncrpty;
    
        }
        /// <summary>
        /// 数据库中的列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 主键名，支持多组主键
        /// </summary>
        /// <remarks>
        /// Column标记是支持多组惟一键值定义的
        /// 这种唯一键的标记用KeyGroupName来进行
        /// 当KeyGroup为空，且IsKey为True时，为实体的默认主键；
        /// 当KeyGroup不为空时，IsKey为True并且KeyGroup相同的为判断唯一的键值</remarks>
        public string KeyGroupName { get; set; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsKey { get; set; }


        /// <summary>
        /// 用在判生类中，列名前缀的插入点
        /// </summary>
        public int PreIndex { get; set; }

        /// <summary>
        /// 用在子类中，列名前缀的插入长度
        /// </summary>
        public int PreLen { get; set; }

        /// <summary>
        /// 是否自动生成，自动生成的方式
        /// </summary>
        public GenerationType IsAutoGenerate { get; set; }

        
        /// <summary>
        /// 存储的类型
        /// </summary>
        public System.Data.SqlDbType SqlType { get; set; }


        /// <summary>
        /// 是否是增长列
        /// </summary>
        public bool IsIdentify { get; set; }    

        /// <summary>
        /// 生成的表达式，如getdate()之类
        /// </summary>
        public String GenerationExp { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get;
            set;
        }


        /// <summary>
        /// 不做映射 
        /// </summary>
        public bool NoMap { get; set; }




        /// <summary>
        /// 属性名称，当映射的是复杂类型时使用
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 是否覆盖基类的定义
        /// </summary>
        public bool OverideParent { get; set; }


        /// <summary>
        /// 数据格式 
        /// </summary>
        public String FormatStr { get; set; }


        public EncryptType EncrpytType { get; set; }
    }
}
