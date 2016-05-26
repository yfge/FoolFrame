using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service
{
    class LocaTokenAoStub : ITokenAoStub
    {
        public string checkToken(int timeout, string token)
        {
            throw new NotImplementedException();
        }

        public void deleteToken(int timeout, string key)
        {
            throw new NotImplementedException();
        }

        public string getToken(int timeout, string key)
        {

            return Guid.NewGuid() + "&&" + key;
        }

        public void updateToken(int timeout, string key, string token, int expireSeconds)
        {
            throw new NotImplementedException();
        }
    }
}
