using Soway.Data.Discription.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOWAY.ORM.AUTH
{
    [Table(Name = "SW_APP_AUTH_COMPANY", ColPreStr = "APP_COR_")]
    public class Company
    {
        [Column(ColumnName = "ID", IsKey = true, IsIdentify = true, IsAutoGenerate = GenerationType.OnInSert)]
        public long Id { get; set; }
        [Column(ColumnName = "NAME")]
        public string  Name { get; set; }

        public List<Department> Deps { get; set; }
    }
}