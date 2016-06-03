using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Query;
using System.Collections;
using Soway.Query.BoolExp;
using Soway.Query.Entity;
using System.IO;  



/// <summary>
/// 查询的命名空间
/// </summary>
namespace Soway.Query
{
    /// <summary>
    /// 表示一个查询的上下文.
    /// </summary>
    /// <remarks>
    /// <p>QueryContext类用于表示一个查询的上下文，是自定义查询组件的中心类。</p>
    /// <p>可以向这个类中加添加查询的表，列，以及布尔表达式，最终生成用于查询的SqlCommand及查询结果.</p>
    /// 在使用此类时，先要实体化一个查询工厂，然后在构造函数中传入
    /// 在初始时，工厂的所有表都在候选表中
    /// 当加入一个表到查询时,只有与当前有联系的表才会显示在候选表
    /// 即与当前表连接的表才儒在候选表中
    /// </remarks>
    public class QueryContext 
    {


        /// <summary>
        /// 实体工厂
        /// </summary>
        private IQueryFactory Facotry {get;set;}
        /// <summary>
        /// 是否可以多表联查
        /// </summary>
        public bool CanJoinSelected{get;set;}
        

        /// <summary>
        /// 初始化一个查询
        /// </summary>
        /// <param name="factory">传入的实体工厂</param>
        public QueryContext(IQueryFactory factory,String queryConStr) {
            this.Facotry = factory;
            this.Ins = new QueryInstance();
            this.QueryConStr = queryConStr;
            
        }

        public QueryInstance Ins { get; private set; }
        public string QueryConStr { get; private set; }







        /// <summary>
        /// 增加一个表
        /// </summary>
        /// <param name="Table"></param>
        /// <param name="fromTable"></param>
        /// <param name="joinType"></param>
        public void   Add(IQueryTable Table,
            SelectedTable fromTable = null)
        {
        
            if (fromTable == null) { 
               
                this.Ins.SelectedTables = new SelectedTables(new SelectedTable(){
                    Table = Table,
                    SelectedTableName = Table.ShowName 
                },this.Facotry);

            }
            else
            {



                var count = this.Ins.SelectedTables.Tables.Count(p=>p.Table.DBName == Table.DBName);
                string selectedName = Table.ShowName +(count ==0?"":count .ToString());

                this.Ins.SelectedTables.Add(new SelectedTable(){Table = Table,
                        SelectedTableName = selectedName},fromTable);

              
            }
        }
    
            
         
        
        
        /// <summary>
        /// 清除查询
        /// </summary>
        public void Clear()
        {

            this.Ins = new QueryInstance(); 
 
            
        }

 

        
        /// <summary>
        /// 将查询保存在数据库中
        /// </summary>
        public void Save()
        {
            throw new System.NotImplementedException();
        }

        private System.Data.SqlClient.SqlCommand GetSql(String AttachSql)
        {

            foreach (var col in this.Ins.SelectedCols.Where(p => p.DataCol.DataType == Data.PropertyType.Enum && (p.Values == null && p.Values.Count == 0)))
                col.Values = this.Facotry.GetStateValues(col.DataCol);
            return SqlScriptFac.GetSql(this.Ins);
        }

        /// <summary>
        /// 返回查询的SqlCommand，注意，不是分页的
        /// </summary>
        /// <returns></returns>
        public  System.Data.SqlClient.SqlCommand  GetSql()
        {
            return GetSql(""); 
        }


        /// <summary>
        /// 返回查询的分布结果
        /// </summary>
        /// <param name="connectionString">连接的目标字符串</param>
        /// <param name="PageSize">分页的大小</param>
        /// <returns>查询结果</returns>
        /// <remarks>
        /// <p>GetResult在实现上，返回QueryResult中含有一个未关闭的SqlCommnad</p>
        /// <p>GetResult在执行时会先得到当前会话在数据库中的线程ID，之后以这个ID在数据库中创建全局临时表。</p>
        /// <p>查询的结果会先Into到这个临时表中，之后QueryResult的每一次更新数据都从这个临时表中提取。</p>
        /// </remarks>
        /// 

        public  QueryResult  GetResult(String connectionString, int PageSize)
        {
            foreach (var col in this.Ins.SelectedCols.Where(p => p.DataCol.DataType == Data.PropertyType.Enum && (p.Values == null ||p.Values.Count == 0)))
                col.Values = this.Facotry.GetStateValues(col.DataCol);
            return SqlScriptFac.Result(connectionString, this.Ins, PageSize);
        }



        public QueryResult GetResult(int pageSize) {
            return GetResult(this.QueryConStr, pageSize);

        }
    }
}
