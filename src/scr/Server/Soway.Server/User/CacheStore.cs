using Soway.Service.ThriftClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.User
{
    class CacheStore
    {
        private static int timeout = 30 * 60 * 1000;
        public void Store(string Token, CacheInfo info)
        {

            MemoryStream ms = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, info);
            var array = ms.ToArray();



            ISessionDaoStub sessionStub = new SessionDaoStubFac().Get();

            sessionStub.updateSession(timeout, Token, array, timeout);

        }
        public CacheInfo Get(String Token)
        {
            ISessionDaoStub sesstionStub = new SessionDaoStubFac().Get();
            var array = sesstionStub.getSession(timeout, Token);
            if (array != null)
            {
                MemoryStream ms = new MemoryStream(array) { Position = 0 };
                IFormatter formatter = new BinaryFormatter();

                return (CacheInfo)formatter.Deserialize(ms);

            }
            else
            {
                return null;
            }
        }


        public void Remove(string Token)
        {
            ISessionDaoStub sessionStub = new SessionDaoStubFac().Get();
            sessionStub.deleteSession(timeout, Token);
        }
    }
}
