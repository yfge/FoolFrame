using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Query.Entity
{
    /// <summary>
    /// 实体工厂
    /// </summary>
    /// <remarks>
    /// 由于数据字典的保存方式各不相同，开发者应该将序列化的内容进行自行定义，从而实现IQueryFactory的类，在构造QueryContext进行传递进去。</remarks>
    public interface IQueryFactory
    {
        /// <summary>
        /// 得到一个表所有连接表
        /// </summary>
        List<JoinTable> GetCanJoinedTables(IQueryTable Table,JoinQueryType JoinType);
        /// <summary>
        /// 得到所有表
        /// </summary>
        /// <returns></returns>
         List<IQueryTable> GetTables();

      
        /// <summary>
        /// 得到表
        /// </summary>
        /// <param name="TableName">表名称</param>
        /// <returns></returns>
         IQueryTable GetTable(String TableName);

         /// <summary>
         /// 得到一个表的所有列
         /// </summary>
         List<IQueryColumn> GetColumns(IQueryTable Table);

        /// <summary>
        /// 得到所有连接关系
        /// </summary>
        /// <returns></returns>
       //  List<JoinTables> GetJoins();

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="Table"></param>
       //  void CreateTable(IQueryTable Table);

        /// <summary>
        /// 保存表
        /// </summary>
        /// <param name="Table"></param>
      //   void SaveTable(IQueryTable Table);

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="Table"></param>
       //  void DeleteTable(IQueryTable Table);

         /// <summary>
         /// 创建连接
         /// </summary>
         /// <param name="Condition"></param>
      //   void CreateJoin(JoinCondition Condition);

         /// <summary>
         /// 保存连接 
         /// </summary>
         /// <param name="Condition"></param>
      //   void SaveJoin(JoinCondition Condition);

        /// <summary>
        /// 删除连接
        /// </summary>
        /// <param name="Condition"></param>
      //   void DeleteJoin(JoinCondition Condition);

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="col"></param>
   //      void DeleteColumn(IQueryColumn col);

        /// <summary>
        /// 新建列
        /// </summary>
        /// <param name="col"></param>
    // void CreateColumn(IQueryColumn col);



        /// <summary>
        /// 保存列
        /// </summary>
        /// <param name="col"></param>
   //      void SaveColumn(IQueryColumn col);


        /// <summary>
        /// 得到一个状态列的状态值
        /// </summary>
        /// <param name="Col"></param>
        /// <returns></returns>
        List<ColStateValue> GetStateValues(IQueryColumn Col)
      ;
        /// <summary>
        /// 得到一个列的状态值对应的实际数据库中的值
        /// </summary>
        /// <param name="Col"></param>
        /// <param name="value"></param>
        /// <returns></returns>
         String GetStateStr(IQueryColumn Col, String value);

    }
}
