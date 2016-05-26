using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;
using System.Linq;
namespace Soway.Query
{
    /// <summary>
    /// 选择的表的集合
    /// </summary>
    public class SelectedTables 
    {


        private List<SelectedTable> tables = null;
        internal List<JoinTable> joins = null;
        private Entity.IQueryFactory Fac { get; set; }
        
        public void Add(SelectedTable table,SelectedTable from)
        {

            if (this.tables.Contains(from) == false)
            {
                throw new Exception("要加的表没有选择！");
            }
            else
            {
                var condition = Fac.GetCanJoinedTables(from.Table, JoinQueryType.All);
                var con = condition.FirstOrDefault(p => (p.LeftTable.Table.DBName == table.Table.DBName && p.RightTable.Table.DBName == from.Table.DBName)
                    || (p.LeftTable.Table.DBName == from.Table.DBName && p.RightTable.Table.DBName == table.Table.DBName));
                if (con == null)
                    throw new Exception("未找到连接条件");

                var addJoin = new JoinTable();
                addJoin.LeftTable = from;
                addJoin.RightTable = table;
            
                if (con.LeftTable.Table.DBName == table.Table.DBName)
                {
                    foreach (var joincondition in con.Conditions)
                        addJoin.Conditions.Add(new JoinCondition(joincondition.RightCol, joincondition.LeftCol));

                 
                }
                else
                {
                    addJoin.Conditions.AddRange(con.Conditions.ToArray());

                }
                this.joins.Add(addJoin);
                this.tables.Add(table);
            }
        }

        public SelectedTables(SelectedTable first,Entity.IQueryFactory fac)
        {
            this.tables = new List<SelectedTable>();
            this.tables.Add(first);
            this.joins = new List<JoinTable>();
            this.Fac = fac;
        }

        public SelectedTable[] Tables
        {

            get
            {
                return this.tables.ToArray();
            }
        }


    }
}
