using System;
using System.Collections.Generic;
 
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Soway.DB.Manage
{
    class EncryptionClass
    {
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
           // try
           // {
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


        //private String Encrypt(String strText, String strEncrKey)
        //{
        //    Byte[] byKey = { };
        //    Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        //    try
        //    {
        //        byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
        //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //        Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
        //        MemoryStream ms = new MemoryStream();
        //        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
        //        cs.Write(inputByteArray, 0, inputByteArray.Length);
        //        cs.FlushFinalBlock();
        //        return Convert.ToBase64String(ms.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        ////'解密函数   
        //private String Decrypt(String strText, String sDecrKey)
        //{
        //    Byte[] byKey = { };
        //    Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        //    Byte[] inputByteArray = new byte[strText.Length];
        //    try
        //    {
        //        byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
        //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //        inputByteArray = Convert.FromBase64String(strText);
        //        MemoryStream ms = new MemoryStream();
        //        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
        //        cs.Write(inputByteArray, 0, inputByteArray.Length);
        //        cs.FlushFinalBlock();
        //        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        //        return encoding.GetString(ms.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}
    }
  
}
