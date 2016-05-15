namespace Soway.Service
{
    public interface ISessionDaoStub
    {
        void deleteSession(int timeout, string sessionKey);
        byte[] getSession(int timeout, string sessionKey);
        void updateSession(int timeout, string sessionKey, byte[] sessionValue, int expireSecond);
    }
}