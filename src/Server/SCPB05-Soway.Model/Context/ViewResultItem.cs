using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Context
{
    public class ViewResultItem
    {
        public IObjectProxy ObjectProxy { get; internal set; }
        public long RowIndex { get; internal set; }
    }
}
