using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    [Serializable]
    [DataContract]
    public class AppInfo
    {
        [DataMember]
        public string AppName { get; set; }
        [DataMember]
        public string AppVer { get; set; }
        [DataMember]
        public string AppNote { get; set; }
        [DataMember]
        public string AppPowerBy { get; set; }
        [DataMember]
        public string AppPowerUrl { get; set; }
        [DataMember]
        public string AppLogoUrl { get; set; }

        [DataMember]
        public long DefaultViewId { get; set; }

        public string AppId { get; set; }
        
    }
}
 