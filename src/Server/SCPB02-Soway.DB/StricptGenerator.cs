using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace Soway.DB
{
    public interface  IDbCommandGenerater
    {

        /// <summary>
        /// 生成创建的SqlCommand
        /// </summary>
        IDbCommand  GenerateCreateSql<T>(T ob,IDBContext cxt);
        /// <summary>
        /// 生成保存的SqlCommand
        /// </summary>
        IDbCommand GenerateSaveSql<T>(T ob);
        /// <summary>
        /// 生成删除的SqlCommand
        /// </summary>
        IDbCommand GenerateDeleteSql<T>(T ob);


        /// <summary>
        /// 生成创建表的SqlCommand
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDbCommand GenerateCreateTableSql<T>();


        /// <summary>
        /// 生成查询一个实体是否已经存在的Sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ob"></param>
        /// <returns></returns>
        IDbCommand GenerateExitsSql<T>(T ob);


        /// <summary>
        /// 生成查询Sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDbCommand GenerateSelectSql<T>();


        /// <summary>
        /// 得到条目类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IDbCommand GenerateSelectItemsSql<T>(object parent,PropertyInfo ParentInfo);
        System.Data.IDbCommand GenerateCreateSql(object ob, IDBContext cxt);
       
        
    }
}
