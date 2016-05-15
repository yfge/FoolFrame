using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service
{
    class ILocalSessionDaoStub : ISessionDaoStub
    {
        private class Session
        {
            public int TimeOut { get; set; }
            public DateTime LastUseTime { get; set; }
            public Byte[] Data { get; set; }

            public string Key { get; set; }
        }
        private static System.Collections.Concurrent.ConcurrentDictionary<string, Session> Local = new System.Collections.Concurrent.ConcurrentDictionary<string, Session>();
        public void deleteSession(int timeout, string sessionKey)
        {
            RemoveTimeOut();
            Session session = null;
            while (Local.ContainsKey(sessionKey) && Local.TryRemove(sessionKey, out session) == false)
                ;
        }

        public byte[] getSession(int timeout, string sessionKey)
        {
            RemoveTimeOut();

            Session session = null;
            while (Local.ContainsKey(sessionKey) && Local.TryGetValue(sessionKey, out session) == false) ;

            if (session == null)
                return new byte[] { };

            session.LastUseTime = DateTime.Now;
            return session.Data;
        }

        private void RemoveTimeOut()
        {
            System.Collections.Concurrent.ConcurrentBag<Session> trytoremove = new System.Collections.Concurrent.ConcurrentBag<Session>();
            foreach(var item in Local.Values.Where(p => DateTime.Now.Subtract(p.LastUseTime).TotalSeconds > p.TimeOut))
            {
                trytoremove.Add(item);
            }
            Session remove;
            foreach(var i in trytoremove)
            {
                while (false == Local.TryRemove(i.Key, out remove)) ;
            }
        }
        public void updateSession(int timeout, string sessionKey, byte[] sessionValue, int expireSecond)
        {

            RemoveTimeOut();

            Session session = new Session()
            {
                Key = sessionKey,
                Data = sessionValue,
                TimeOut = timeout,
                LastUseTime = DateTime.Now
            };
            Session remove = null;
            while (Local.ContainsKey(sessionKey) && Local.TryRemove(sessionKey, out remove)) ;
            while (Local.TryAdd(sessionKey, session) == false) ;

        }
        internal ILocalSessionDaoStub() { }
    }
}
