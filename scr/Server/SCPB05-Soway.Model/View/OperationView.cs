using Soway.Data.Discription.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.View
{
    [Table(Name = "SW_SYS_OPERATIONVIEW",ColPreStr ="SW_SYS_OPVIEW_")]
    public class OperationView
    {
        [Column (ColumnName = "NAME")]
        public string Name { get; set; }
        [Column(ColumnName = "RESULT")]
        public Soway.Model.View.View ResultView { get; set; }
        [Column(ColumnName = "OPREATION")]
        public Soway.Model.Operation Operation { get; set; }
        [Column(ColumnName = "SUCCESMSG")]
        public string SuccessMsg { get; set; }
        [Column(ColumnName = "ERRORMSG")]
        public string ErrorMsg { get; set; }
        [Column(ColumnName = "MSG")]
        public string Msg { get; set; }
        [Column(ColumnName ="SHOW")]
        public bool IsShow { get; set; }
        [Column(ColumnName = "ConfirmMSG")]
        public string ConfirmMsg { get; set; }

        public List<OperationViewItem> Params { get; set; }
    }
}
