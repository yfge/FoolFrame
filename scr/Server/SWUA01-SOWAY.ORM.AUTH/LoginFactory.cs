using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Soway.Model.Context;

namespace SOWAY.ORM.AUTH
{
    public class LoginFactory
    {
        public User Login(String UserName,String Pwd)
        {

            var db = new Soway.Model.SqlServer.ObjectContext<User>(this.Con,this.ConFac);
            var user = db.GetDetail(UserName);

            if (user.PassWord == ToMD5(Pwd))
            {
                user.PassWord = Pwd;
                return user;
            }
            else
                return null;
        }

        public void RegUser(User user)
        {
            var db = new Soway.Model.SqlServer.ObjectContext<User>(this.Con,this.ConFac);
            db.Create(user);
        }
        public bool ChangePassWord(String userName,string oldPwd,String newPwd)
        {
            var db = new Soway.Model.SqlServer.ObjectContext<User>(this.Con,this.ConFac);
            var user = db.GetDetail(userName);
            if (user.PassWord == ToMD5(oldPwd))
            {

                user.PassWord = newPwd;
                db.Save(user);
                return true;
            }
            else
            {
                return false;
            }

        }
        private Soway.Model.SqlCon Con { get; set; }
        public ICurrentContextFactory ConFac { get; private set; }

        public LoginFactory(Soway.Model.SqlCon con,Soway.Model.Context.ICurrentContextFactory conFac)
        {
            this.Con = con;
            this.ConFac = conFac;

        }
        public bool UpdateUser(User user)
        {
            var db = new Soway.Model.SqlServer.ObjectContext<User>(this.Con,this.ConFac);
            db.Save(user);
            return true;
            

        }
        public static string ToMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }
            return byte2String;
        }

    }
}