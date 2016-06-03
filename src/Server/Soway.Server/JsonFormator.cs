using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Soway.Service
{
    public class JsonFormator
    {
        public static string Serialize(Result obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            string json = JsonConvert.SerializeObject(obj,setting);
            return json;
        }
    }
}