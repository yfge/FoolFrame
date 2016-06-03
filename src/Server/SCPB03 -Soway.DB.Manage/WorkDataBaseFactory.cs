using System;
using System.Collections.Generic;
 
using System.Text;
using Soway.DB.Manage;
 
using System.Data;

namespace Soway.DB.Manage
{
    public class WorkDataBaseFactory
    {

        
        private string SqlCon { get; set; }

        public WorkDataBaseFactory(String sqlCon)
        {
            SqlCon = SqlCon;

        }

        public WorkDataBaseFactory()
        {
            this.SqlCon = (Global.FacType.GetConstructor(new Type[]{}).Invoke(new object[]{}) as DbConStrFac).GetConStr();
        }
       
        
　


        public   WorkingDataBase[] ALL(String AppName)
        {
            var sql = @"SELECT [DBID]
                      ,[DBName]
                      ,[DBYear]
                      ,[DBSysName]
                      ,[IsActive]
                      ,[WorkDataBase].[DBNo]
                      ,[pwd1]
                      ,[pwd2]
                      ,[pwd3]
                      ,[pwd4]
                      ,[pwd5]
                      ,[UserName]
                      ,[CompanyName]
                      ,[ServerIp]
                      ,[IsLocal]
                  FROM  [WorkDataBase] join [DB_AppDB] on [WorkDataBase].[DBNo]=[DB_AppDB].[DBNo]
                    join [DB_App] on ([DB_App].[BO_Id]=[DB_AppDB].[App_Id] and [BO_AppName]='" +AppName + @"') ORDER BY [WorkDataBase].DBNo";
            var r = new SqlCon(this.SqlCon).GetTable(sql);
            List<WorkingDataBase> result = new List<WorkingDataBase>();
            try
            {
                foreach (DataRow i in r.Rows)
                {
                    result.Add(new WorkingDataBase(i));
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public 　 WorkingDataBase[] ALLList()
        {
            if (allList != null)
                return allList.ToArray();
            else
            {
                allList = new List<WorkingDataBase>();
                var sql = @"SELECT [DBID]
                      ,[DBName]
                      ,[DBYear]
                      ,[DBSysName]
                      ,[IsActive]
                      ,[WorkDataBase].[DBNo]
                      ,[pwd1]
                      ,[pwd2]
                      ,[pwd3]
                      ,[pwd4]
                      ,[pwd5]
                      ,[UserName]
                      ,[CompanyName]
                      ,[ServerIp]
                      ,[IsLocal]
                  FROM  [WorkDataBase] ORDER BY DBNo";
                var r = new SqlCon().GetTable(sql);
         //       List<WorkingDataBase> result = new List<WorkingDataBase>();
                try
                {
                    foreach (DataRow i in r.Rows)
                    {
                        allList.Add(new WorkingDataBase(i));
                    }
                    return allList.ToArray();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        private static List<WorkingDataBase> allList; 
        private String ByteToString(byte[] data)
        {
            string a = "0x";
            foreach (var i in data)
            {
                a += i.ToString("X2");
            }
            return a;
        }
        public void Create(WorkingDataBase db)
        {

               
            //using (DBDataDataContext contect = DBDataDataContext.Instance)
            //{

                String selectSql = "Select [DBID] from [WorkDataBase] Where [DBName]='" + db.Name + "' And [DBYear]='" + db.Year + "'";
                if (new SqlCon().GetTable(selectSql).Rows.Count > 0)
                {
                    throw new Exception("不能创建名称，年度相同的帐套");
                }
                else
                {

                    var NoSql = "Select Max(DBNo) from  [WorkDataBase]";

                    var noTable = new SqlCon().GetTable(NoSql);
                    var topno = noTable.Rows[0][0];
                    String dbNo;
                    if (topno == null
                        || (topno is DBNull))
                    {
                        dbNo = "01";
                    }
                    else
                    {
                        dbNo = (System.Convert.ToInt32(topno.ToString()) + 1).ToString().PadLeft(2, '0');
                    }


                    EncryptionClass entry = new EncryptionClass(db.PassWord);
                    //user.pwd1 = entry.EncrKey;
                    //user.pwd2 = entry.EnIndex;
                    //user.pwd3 = entry.IV;
                    //user.pwd4 = entry.IvIndex;
                    //user.pwd5 = entry.EncryString;

                    String InsertSql = @"INSERT INTO  [WorkDataBase]
           ([DBName]
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
           ,[ServerIp])
     VALUES
           ('" + db.Name + @"'
           ,'" + db.Year + @"'
           ,'" + db.SysName + @"'
           ,'" + System.Convert.ToInt32(db.IsActive).ToString() + @"'
           ,'" + dbNo + @"'
           ," + ByteToString( entry.EncrKey) + @"
           ," + ByteToString(entry.EnIndex) + @"
           ," + ByteToString(entry.IV) + @"
           ," + ByteToString(entry.IvIndex) + @"
           ,'" + entry.EncryString + @"'
           ,'" + db.UserName + @"'
             ,'" + db.CompanyName + @"'
           ,'" + db.ServerIp  + @"')";
                    new SqlCon().ExcuteSql(InsertSql);
                }
//                WorkDataBase user = new WorkDataBase();
//                var q = from db1 in contect.WorkDataBases
//                        where db1.DBName == db.Name
//                        && db.Year == db1.DBYear
//                        select db1 ;
//                if (q.Count() > 0)
//                {
//                    throw new Exception("不能创建名称，年度相同的帐套");
//                }
//                  var q2=contect.WorkDataBases ;
//                    if (q2.Count() > 0)
//                    {
//                        user.DBNo  = (System.Convert.ToInt32(q2.Max(p => p.DBNo ))+1).ToString().PadLeft(2, '0');
//                    }
//                    else
//                        user.DBNo  =  "01";
           
              
//                user.UserName = db.UserName;
//                user.DBYear = db.Year;
//                user.DBSysName = db.SysName;
//                user.IsActive = db.IsActive;
//                user.DBName = db.Name;
                 
//                EncryptionClass entry = new EncryptionClass(db.PassWord);
//                user.pwd1 = entry.EncrKey;
//                user.pwd2 = entry.EnIndex;
//                user.pwd3 = entry.IV;
//                user.pwd4 = entry.IvIndex;
//                user.pwd5 = entry.EncryString;
                
//                contect.WorkDataBases.InsertOnSubmit(user);
//                contect.SubmitChanges();
//            }
        }
        public void Save(WorkingDataBase db)
        {   String selectSql = "Select [DBID] from [WorkDataBase] Where [DBName]='" + db.Name + "' And [DBYear]='" + db.Year + "' And [DBNo]<>'"+db.Code +"' ";
                if (new SqlCon().GetTable(selectSql).Rows.Count > 0)
                {
                   throw new Exception("无法保存登录设置，保存会与现有登录冲突");
                }

                 EncryptionClass entry = new EncryptionClass(db.PassWord);
            //    user.pwd1 = entry.EncrKey;
            //    user.pwd2 = entry.EnIndex;
            //    user.pwd3 = entry.IV;
            //    user.pwd4 = entry.IvIndex;
            //    user.pwd5 = entry.EncryString;

                 String updateSql = @"UPDATE [WorkDataBase]
   SET [DBName] = '" + db.Name + @"'
      ,[DBYear] ='" + db.Year + @"'
      ,[DBSysName] ='" + db.SysName + @"'
      ,[IsActive] = '" + System.Convert.ToInt32(db.IsActive) + @"'
      
      ,[pwd1] = " + ByteToString(entry.EncrKey) + @"
      ,[pwd2] = " + ByteToString(entry.EnIndex) + @"
      ,[pwd3] = " + ByteToString(entry.IV) + @"
      ,[pwd4] =" + ByteToString(entry.IvIndex) + @"
      ,[pwd5] = '" + entry.EncryString + @"'
      ,[UserName] = '" + db.UserName  + @"'
      ,[CompanyName] = '" + db.CompanyName + @"'
      ,[ServerIp]='"+db.ServerIp + @"'
      ,[IsLocal]='"+System.Convert.ToInt32(db.IsLocal)+@"'
        WHERE [DBNo]='" + db.Code + "'";
                 new SqlCon().ExcuteSql(updateSql);
            
        }
        public void Delete(WorkingDataBase db)
        {
            String deleteSql = "DELETE  [WorkDataBase] WHERE [DBNo]='" + db.Code + "'";
            new SqlCon().ExcuteSql(deleteSql);

            //using (Soway.DB.Manage.DBDataDataContext connect = Soway.DB.Manage.DBDataDataContext.Instance)
            //{
            //    var q = from dbs in connect.WorkDataBases
            //            where dbs.DBNo == db.Code 
            //            select dbs;
            //    if (q.Count() > 0)
            //    {
            //        connect.WorkDataBases.DeleteOnSubmit(q.First());
            //        connect.SubmitChanges();
            //    }
            //}
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        public void CreateDataBase(WorkingDataBase DataBase)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 将一个大宗物料的数据库转化成自动过磅数据库
        /// </summary>
        /// <param name="DataBase">将一个大宗物料的数据库转化成自动过磅数据库</param>
        public void ConvertToAutoDataBase(WorkingDataBase DataBase)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 年终结转
        /// </summary>
        /// <param name="WorkDataBase">要结转的数据库</param>
        /// <param name="DisWorkDataBase">B</param>
        /// <param name="IsBFDB">大宗物料库</param>
        public void CarryForward(WorkingDataBase WorkDataBase, WorkingDataBase DisWorkDataBase, bool IsBFDB)
        {
            throw new System.NotImplementedException();
        }

    }
}
