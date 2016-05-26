using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service
{
    class SessionDaoStubFac
    {
        public ISessionDaoStub Get()
        {
            return new ILocalSessionDaoStub();
        }
    }
}
