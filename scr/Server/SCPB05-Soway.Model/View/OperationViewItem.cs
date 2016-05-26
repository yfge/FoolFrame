using Soway.Data.Discription.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.View
{
    [Table(Name = "SW_SYS_OPERATIONVIEW_ITEM", ColPreStr = "SW_SYS_OPVIEWITEM_")]
    public class OperationViewItem
    {
        [Column(ColumnName = "NAME")]
        public string ParamName { get; set; }
        [Column(ColumnName = "INDEX")]
        public int Index { get; set; }
        [Column(ColumnName = "PARAM")]
        public OperationParam Param { get; set; }
    }
}
