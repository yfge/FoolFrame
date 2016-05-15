using Soway.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class EvtConfac :Soway.Model.Context.ICurrentContextFactory
    {
        public CurrentContext GetCurrentContext()
        {

            CurrentContext con = new CurrentContext();
            con.SysCon = new bean.ConHelper().GetSysCon();
         

            return con;
        }

        public object GetValue(string key)
        {

            return "";
        }

        public DateTime GetDateTime()
        {

            return DateTime.Now;
        }
    }
}