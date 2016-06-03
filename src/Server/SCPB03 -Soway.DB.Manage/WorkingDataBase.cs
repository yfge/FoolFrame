using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Data.SqlClient;
using Soway.DB.Manage; 
namespace Soway.DB.Manage
{
    public class WorkingDataBase
    {
        public string Name
        {
            get;
            set;
        }

        public string SysName
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Name + "_" + this.Year;
        }

        public bool IsActive
        {

            get;
            set;
        }

        public string Code
        {
            get;
            private set;
        }


        public bool IsLocal { get; set; }
        public string ConnectionString
        {
            get
            {
                
                    SqlConnectionStringBuilder SqlBuilder = new SqlConnectionStringBuilder
                    (Global.ConnectionString );


                    SqlConnectionStringBuilder formsql = new SqlConnectionStringBuilder(Global.ConnectionString);
                    

                    SqlBuilder.InitialCatalog = this.SysName;
                    SqlBuilder.ApplicationName = Global.AppName;
                    SqlBuilder.UserID = this.UserName;
                    SqlBuilder.Password = this.PassWord;
                    if (this.IsLocal)
                        SqlBuilder.DataSource = formsql.DataSource;
                else    
                        SqlBuilder.DataSource=this.ServerIp;
                    return SqlBuilder.ToString();
                

            }
        }

        public string Year { get; set; }




        
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string ServerIp { get; set; }
        public WorkingDataBase (){}
     
        internal WorkingDataBase( DataRow i)
        {
            Init(i);
         
        }
        public void Reload()
        {
            

            var sql = @"Select [DBID]
      ,[DBName]
      ,[DBYear]
      ,[DBSysName]
      ,[IsActive]
      ,[DBNo]
      ,[pwd1]
      ,[pwd2]
      ,[pwd3]
      ,[pwd4]
      ,[pwd5]
      ,[UserName]
      ,[CompanyName]
      ,[ServerIp]
  FROM [WorkDataBase] Where [DBNo]='" + this.Code +"'";
            var q = new SqlCon().GetTable(sql);
                if (q.Rows .Count  > 0)
                {
                    Init(q.Rows[0]);
                }
             
        }

        public string CompanyName { get; set; }

        private void Init(DataRow i)
        {
            this.Name = i["DBName"].ToString();
            this.SysName = i["DBSysName"].ToString();
            this.Year = i["DBYear"].ToString();
            this.IsActive =System.Convert.ToBoolean ( i["IsActive"]);
            this.Code = i["DBNo"].ToString();
            this.CompanyName = i["CompanyName"].ToString();
            this.ServerIp = i["ServerIp"].ToString();

            try
            {
                EncryptionClass entry = new EncryptionClass();
                entry.EncrKey = i["pwd1"] as byte[];
                entry.EnIndex = i["pwd2"] as byte[];// ToArray();
                entry.IV = i["pwd3"] as byte[];// ToArray();
                entry.IvIndex = i["pwd4"] as byte[];// ToArray();
                entry.EncryString = i["pwd5"].ToString();
                PassWord = entry.Decrypt();
                this.UserName = i["UserName"].ToString();

                this.IsLocal = (bool)i["IsLocal"];
            }
            catch
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Global.ConnectionString );
                this.UserName = builder.UserID;
                this.PassWord = builder.Password;
                new WorkDataBaseFactory().Save(this);
            }

        }
      
        /// <summary>
        /// 结转
        /// </summary>
        public void Update()
        {
            throw new System.NotImplementedException();
        }
        
    }
}
