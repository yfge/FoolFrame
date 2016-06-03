using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{
    public interface IObjectFromDataBase
    {
        System.Data.DataRow Row { get; set; }
    }
}
