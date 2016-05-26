using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.Entity;

namespace Soway.Query.BoolExp
{
    /// <summary>
    /// 表示一个布尔操作符（二元），即 与 （and),或（or)
    /// </summary>
    /// 

    
    public class  BoolOp :IQueryAtom
    {
       
        public string ShowName
        {
            get;
            set;
        }

        public string DBName
        {
            get;
            set;
        }

        public static BoolOp And = new BoolOp() { DBName = " And ", ShowName = "并且" };
        public static BoolOp Or = new BoolOp() { DBName = " OR ", ShowName = "或者" };
    }
}
