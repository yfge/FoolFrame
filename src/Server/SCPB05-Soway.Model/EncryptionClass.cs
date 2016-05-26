using System;
using System.Collections.Generic;
 
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data;

namespace Soway.Model
{
    class EncryptionClass
    {
        public static string IVStr = "_IV";
        public static string ENStr = "_EN";
        public static string IVIndexStr = "_IVINDEX";
        public static string ENIndexStr = "_ENINDEX";
        public static string PwdStr = "_TEXT";
        public static string[] StrCols
        {
            get
            {
                return new string[]{
                    IVStr,ENStr,IVIndexStr,ENIndexStr,PwdStr
                };
            }
        }
        public static string[] GetPropertyCols(Property property)
        {
            List<string> index = new List<string>();
            foreach (var str in StrCols)
            {
                index.Add(property.DBName + str);
            }
            return index.ToArray();
        }


        public static Dictionary<string, object> GetEncryptionValus(Property property, object ob)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();


            var encypt = new EncryptionClass((ob??"").ToString());
            result.Add(property.DBName + IVStr, encypt.IV);
            result.Add(property.DBName + IVIndexStr, encypt.IvIndex);
            result.Add(property.DBName + ENStr, encypt.EncrKey);
            result.Add(property.DBName + ENIndexStr, encypt.EnIndex);
            result.Add(property.DBName + PwdStr, encypt.EncryString);
            return result;
      
       
        }

        public static String GetDecrptyString(Property property, DataRow Row,string PreName=null)
        {
            var encypt = new EncryptionClass();
            encypt.IV = Row[PreName+property.DBName + IVStr] as byte[];
            encypt.IvIndex = Row[PreName+property.DBName + IVIndexStr] as byte[];
            encypt.EncrKey = Row[PreName+property.DBName + ENStr] as byte[];
            encypt.EnIndex = Row[PreName+property.DBName + ENIndexStr] as byte[];
            encypt.EncryString = Row[PreName+property.DBName + PwdStr].ToString();
            return encypt.Decrypt();

        }
        private static String ByteToString(byte[] data)
        {
            string a = "0x";
            foreach (var i in data)
            {
                a += i.ToString("X2");
            }
            return a;
        }
        public byte[] IV { get; set; }
        public byte[] EncrKey { get; set; }
        public byte[] IvIndex { get; set; }
        public byte[] EnIndex { get; set; }
        public string EncryString { get; set; }

        public EncryptionClass(String strText)
        {
            byte[] ivtemp = new byte[8];
            byte[] entemp = new byte[8];

            System.Random random = new Random(System.DateTime.Now.Millisecond);
            random.NextBytes(ivtemp);
            random.NextBytes(entemp);
            IvIndex = GetIndex(random);
            EnIndex = GetIndex(random);
            IV = new byte[8];
            EncrKey = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                IV[i] = ivtemp[IvIndex[i]];
                EncrKey[i] = entemp[EnIndex[i]];

            }
              
            var byKey = entemp;// System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText??"");
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, ivtemp ), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            EncryString = Convert.ToBase64String(ms.ToArray());
        }
        public EncryptionClass() { }
        public String Decrypt()
        {

            byte[] ivtemp = new byte[8];
            byte[] entemp = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                 ivtemp[IvIndex[i]]=IV[i];
                entemp[EnIndex[i]] = EncrKey[i];

            }

               Byte[] inputByteArray = new byte[this.EncryString.Length];
        
                var byKey =  entemp;
                //byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(this.EncryString );
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, ivtemp ), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());

        }
        private byte[] GetIndex(Random r)
        {

            List<byte> index = new List<byte>();
            for (byte i = 0; i < 8; i++)
                index.Add(i);
            List<byte> result = new List<byte>();
            for (byte i = 7; i >= 0 && i != 255; i--)
            {
                var j = r.Next(i);
                result.Add(index[j]);
                index.Remove(index[j]);

            }
            return result.ToArray();
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
