using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;

namespace Soway.Model
{

    [System.Serializable]
    [Table(Name="SW_SYS_CON",ColPreStr="SW_SYS_CON_")]
 
    [DataContract]
    public class SqlCon
    {
        [DataMember]
        [Column(ColumnName = "DATASOURCE", IsKey = true, KeyGroupName = "DB")]
        public string DataSource
        {
            get;
            set;
        }

        [DataMember]
        [Column(ColumnName = "INITALCATALOG", IsKey = true, KeyGroupName = "DB")]
        public string  InitialCatalog
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName = "USERNAME", IsKey = true, KeyGroupName = "DB")]
        public string UserID
        {
            get;
            set;
        }

        [DataMember]
        [Column(ColumnName = "PASSWORD",
            EncrpytType=Soway.Data.Discription.ORM.EncryptType.RadomDECS)]
        public string Password
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName = "INTEGRATEDSECURITY")]
        public bool IntegratedSecurity
        {
            get;
            set;
        }

        [DataMember]
        [Column(ColumnName = "ISLOACL")]
        public bool IsLocal
        {

            get;
            set;
        }

        public SqlCon() 
        {
        }

        public override string ToString()
        {

            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
           
            builder.InitialCatalog = this.InitialCatalog;
            builder.DataSource = this.DataSource;
            //if (this.IsLocal)
            //    builder.DataSource = Global.SqlCon.DataSource;
            builder.IntegratedSecurity = this.IntegratedSecurity;
            if (this.IntegratedSecurity == false)
            {
                builder.Password = this.Password;
                builder.UserID = this.UserID;
            }
            return builder.ToString();
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
