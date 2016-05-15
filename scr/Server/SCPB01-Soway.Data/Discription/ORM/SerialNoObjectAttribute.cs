using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{

 
     /// <summary>
     /// 标记在属性上，表明这是一个自动生成的流水号
     /// </summary>
     /// <remarks>
     /// <example>
     /// <code>
     /// 
     /// // the No will be gerated as "TE1301010001" on inserted
     /// class  Test{
     /// [SerailNoObject(Len=3,SerialPre="TE",DateForamatStr="yyMMdd")]
     /// public string No{get;set;}
     /// }
     /// </code>
     /// </example></remarks>
     /// 
 
    public class SerialNoObject:Attribute 
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public int Len { get; set; }


        /// <summary>
        /// 时间的格式
        /// </summary>
        public String DateFormateStr { get; set; }

        /// <summary>
        /// 序列号的前缀
        /// </summary>
        public String SerialPre { get; set; }

        
    }
}
