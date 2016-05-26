using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;

namespace Soway.DB.Manage
{

    [Table(Name="DB_App")]
    public class Application :Soway.Data.BusinessObject
    {
        [Column(ColumnName="BO_AppName")]
        public string AppName { get; set; } 
    }
}
