using Soway.Model;
using Soway.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Soway.Service
{
	public class Global : System.Web.HttpApplication
	{




        private static List<Soway.Model.Model> modelsCache;
        private static void Init()
        {
            modelsCache = new AssemblyModuleSource(new AssemblyModelFactory(typeof(Global))).GetModels();
        }

        public static List<Model.Model> Models
        {
            get
            {
                if (modelsCache == null || modelsCache.Count == 0)
                    Init();
                return modelsCache;
            }
        }
		protected void Application_Start(object sender, EventArgs e) 		
		{

           
            Soway.Event.EventMakeService service = new Soway.Event.EventMakeService(
            new EvtConfac());
           service.Start();

            
             
		}
		
		protected void Session_Start(object sender, EventArgs e) 
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e) 
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e) 
		{

		}

		protected void Application_Error(object sender, EventArgs e) 
		{

		}

		protected void Session_End(object sender, EventArgs e) 
		{

		}

		protected void Application_End(object sender, EventArgs e) 
		{

		}
	}
}