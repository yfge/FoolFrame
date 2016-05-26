using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;

namespace Soway.Model
{
 
    public enum ModelRefType
    {
        [Enum("系统模型")]
        SysModel = 0,
        [Enum("应用模型")]
        AppModel =1,
        [Enum("数据集模型")]
        DataSetModel =2
    }
}
