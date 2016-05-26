using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;

namespace SOWAY.ORM.AUTH
{

    [Table(Name = "SW_APP_AUTH_ROLE",ColPreStr ="AUTH_ROLE_")]
    public class Role
    {
        [Column(ColumnName ="ID",IsKey =true,IsIdentify =true,IsAutoGenerate = GenerationType.OnInSert)]
        public long RoleId { get; set; }

        [Column(ColumnName ="NAME")]
        public String RoleName { get; set; }
        public List<AuthorizedUser> AuthUsers { get; set; }
        public List<Department> AuthDeps { get; set; }
        public List<MenuItem> Items { get; set; }
    }
}