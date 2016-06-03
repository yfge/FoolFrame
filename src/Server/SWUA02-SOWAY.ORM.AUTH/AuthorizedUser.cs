using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
namespace SOWAY.ORM.AUTH
{
    [Table(Name ="SW_APP_AUTH_USER",ColPreStr ="APP_AUTH_")]
    public class AuthorizedUser
    {
        /// <summary>
        /// 用户
        /// </summary>
        /// 
        [Column(ColumnName ="USERID",PropertyName = "UserID")]
        [Column(ColumnName = "USERLOGINNAME", PropertyName = "LoginName")]

        public SOWAY.ORM.AUTH.User User
        {
            get;set;
        }
        /// <summary>
        /// 角色
        /// </summary>
        public List<Role> Roles
        {
            get; set;
        }

        /// <summary>
        /// 部门
        /// </summary>
        /// 
        [Column(ColumnName = "DEP")]
        public Department Department
        {
            get;set;
        }
        [Column(ColumnName="ID",IsKey =true,IsIdentify =true,IsAutoGenerate = GenerationType.OnInSert)]

        //授权ID 
        public long AuthorizedId
        {
            get; set;
        }
    }
}