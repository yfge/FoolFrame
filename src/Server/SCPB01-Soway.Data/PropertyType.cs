using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{
    public enum PropertyType
    {


        /// <summary>
        /// 自增长ID
        /// </summary>
        IdentifyId=0,
        /// <summary>
        /// 整型
        /// </summary>
        Int=1,
        UInt=2,
        Long=3,
        ULong=4,
        Float=5,
        Double=6,
        Decimal=7,
        Boolean=8,
        Char=9,
        Byte=10,
        String=11,
        Date=12,
        Time=13,
        DateTime=14,
        Enum=15,
        /// <summary>
        /// 自定义类型
        /// </summary>
        BusinessObject = 16,
        SerialNo=17,
        MD5=18,
        Radom=19,
        RadomDECS = 20,
        Guid = 21


        
    }
}
