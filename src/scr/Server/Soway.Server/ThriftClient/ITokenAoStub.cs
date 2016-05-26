namespace Soway.Service
{
    public interface ITokenAoStub
    {
        string checkToken(int timeout, string token);
        void deleteToken(int timeout, string key);
        string getToken(int timeout, string key);
        void updateToken(int timeout, string key, string token, int expireSeconds);
    }
}