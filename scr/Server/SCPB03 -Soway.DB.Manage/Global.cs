using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.DB.Manage
{
    public class Global
    {

        private static  string conStr;
        public static String ConnectionString
        {
            get
            {
                if (conStr == null)
                    return (Global.FacType.GetConstructor(new Type[] { }).Invoke(new object[] { }) as DbConStrFac).GetConStr();
                return conStr;
            }

            set
            {
                conStr = value;
          
            }
        }
        public static String AppName { get; set; }
        public static String insId
        {
            get;
            set;
        }

        public static Type FacType { get; set; }
    }
}
