using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using Soway.Data.Discription.Display;
namespace Soway.DB.Manage
{
    
    [Table(Name="DS_DataSourceSet")]
    public class DataBaseSource
    {
        [Column(ColumnName="DS_Key",IsKey=true)]
        public string Key
        {
            get;  

            set;
        }


        [Column(ColumnName="DS_DBNo")]
        public string DbNo
        {
            get;
            set;
        }
    }
}
