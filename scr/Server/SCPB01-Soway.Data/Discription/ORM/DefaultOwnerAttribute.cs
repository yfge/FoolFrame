using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    /// <summary>
    /// 默认主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultOwnerAttribute :Attribute
    {
    }
}
