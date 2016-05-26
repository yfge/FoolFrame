using Soway.Data.Discription.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Soway.Model.App
{
    [Table(Name = "SW_APPLICATION", ColPreStr = "SW_APP_")]
    public class Application
    {
        [Column(ColumnName = "APPLICATIONID", IsKey = true)]
        public Guid APPID { get; set; }
        [Column(ColumnName = "KEY", EncrpytType = EncryptType.RadomDECS)]
        public string AppKey { get; set; }
        [Column(ColumnName = "TYPE")]
        public AppType AppType { get; set; }
        [Column(ColumnName = "AVATAR")]
        public string Avatar { get; set; }
        [Column(ColumnName = "COMPANY")]
        public string Company { get; set; }
        [Column(ColumnName = "CREATEIME")]
        public DateTime CreateTime { get; set; }
        [Column(ColumnName = "CREATOR")]
        public SOWAY.ORM.AUTH.User Creator { get; set; }

        [Column(ColumnName = "INITPIC")]
        public string InitImage { get; set; }
        [Column(ColumnName = "NAME")]
        public string Name { get; set; }
        [Column(ColumnName = "NOTE")]
        public string Note { get; set; }
        [Column(ColumnName = "OWNER")]
        public SOWAY.ORM.AUTH.User Owner { get; set; }
        [Column(ColumnName = "RELEASETIME")]
        public DateTime ReleaseTime { get; set; }
        [Column(ColumnName = "UPDATETIME")]
        public DateTime UpdateTime { get; set; }
        [Column(ColumnName = "URL")]
        public string Url { get; set; }
        [Column(ColumnName = "VERSION")]
        public string Version { get; set; }
        public List<StoreDataBase> DataBase { get; set; }
        [Column(ColumnName = "CON")]
        public SqlCon SysCon { get; set; }

        [Column(ColumnName = "VIEW")]
        public long DefaultView { get; set; }



 
    }
}
