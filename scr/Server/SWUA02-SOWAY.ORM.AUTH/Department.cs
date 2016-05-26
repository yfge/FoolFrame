using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using Soway.Model.View;

namespace SOWAY.ORM.AUTH
{
    [Table(Name = "SW_APP_AUTH_DEPARTMENT", ColPreStr = "APP_DEP_")]
    public class Department
    {
        [Column(ColumnName ="ID",IsKey =true,IsIdentify =true,IsAutoGenerate = GenerationType.OnInSert)]
        public long DepId { get; set; }
        [Column(ColumnName ="NAME")]
        public string DepartmentName { get; set; }
        public List<Role> DefaultRoles { get; set; }

        public List<Department> SubDepartments { get; set; }

        [ReferToProperyAttrbute("Department")]
        public List<AuthorizedUser> Users { get; set; }



        [Column(ColumnName="DEFAULTVIEW")]
        public long DefaultView { get; set; }
    }
}