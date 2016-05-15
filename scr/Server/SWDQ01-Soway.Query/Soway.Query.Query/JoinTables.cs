using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;

namespace Soway.Query
{
    /// <summary>
    /// 表示选择的表
    /// </summary>
    public class JoinTable
    {
        /// <summary>
        /// 左表
        /// </summary>
        public SelectedTable LeftTable
        {
            get;
            set;
        }

        /// <summary>
        /// 连接的条件
        /// </summary>
        public JoinConditions Conditions
        {
            get;
            set;
        }

        /// <summary>
        /// 右表
        /// </summary>

        public SelectedTable RightTable
        {
            get;
            set;
        }
    
       


        /// <summary>
        /// 转化成向目标的
        /// </summary>
        /// <returns></returns>
        public JoinTable Convert()
        {
            JoinTable result = new JoinTable() { LeftTable = RightTable  ,RightTable=LeftTable,  Conditions = new JoinConditions() };
            foreach (var condition in this.Conditions)
            {
                result.Conditions.Add(new JoinCondition(condition.RightCol, condition.LeftCol ));
            }
            return result;
        }


        public JoinTable()
        {
            this.Conditions = new JoinConditions();
        }
    }
}
