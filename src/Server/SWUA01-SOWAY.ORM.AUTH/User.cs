using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using Soway.Model.View;

namespace SOWAY.ORM.AUTH
{
    [Table(ColPreStr ="USER_",Name ="SW_AUTH_USER")]
    public class User
    {
        [Column(ColumnName ="UID",IsIdentify =true,IsAutoGenerate = GenerationType.OnInSert,IsKey =true)]
        public long UserID { get; set; }

        [Column(ColumnName ="UUID",IsKey =true,KeyGroupName ="UUID")]
        public Guid UserGuid {
            get; set; }
        [Column(ColumnName ="LOGINNAME",IsKey =true,KeyGroupName ="LOGINNAME")]
        public string LoginName { get; set; }

        [Column(ColumnName ="PHONE",IsKey =true,KeyGroupName ="PHONE")]
        public string Phone
        {
            get; set;
        }
        [Column(ColumnName ="MAIL",IsKey =true,KeyGroupName = "MAIL")]
        public string Email
        {
            get; set;
        }
        [Column(ColumnName ="FIRSTNAME")]
        public string FirstName
        {
            get; set;
        }
        [Column(ColumnName ="LASTNAME")]
        public string LastName
        {
            get; set;

        }
        [Column(ColumnName ="SHOWNAME")]
        public string ShowName
        {
            get; set;
        }
        [Column(ColumnName ="TITLE")]
        public string Title
        {
            get; set;

        }
        [Column(ColumnName ="AVTAR")]
        public string Avtar
        {
            get; set;
        }
        [Column(ColumnName ="PWD",EncrpytType = EncryptType.MD5)]
        public string PassWord
        {
            get; set;
        }
        [Column(ColumnName ="REGTIME",IsAutoGenerate = GenerationType.OnInsertAndUpate)]
        public DateTime CreatTime
        {
            get; set;
        }
        [Column(ColumnName ="LASTLOGINTIME")]
        public DateTime LastLoginTime
        {
            get; set;

        }

        [Column(ColumnName ="LASTMODIFYTIME")]
        public DateTime LastModifyTime
        {
            get;set;
        }
        [Column(ColumnName ="SEX")]
        public Sex Sex
        {
            get; set;
        }

        [Column(ColumnName="DEFAULTVIEW")]
        public View DefaultView { get; set; }
    }
       
}