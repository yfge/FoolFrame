using org.soldier.platform.svr_platform.comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Thrift.Protocol;
using Thrift.Transport;

namespace Soway.Service.ThriftClient
{

    public class SessionDaoStub : ISessionDaoStub
    {
        private SessionDaoStub() { }
        private TTransport mTransport;

        public byte[] getSession(int timeout, string sessionKey)
        {
            byte[] result = null;
            try
            {
                SessionDao.Client client = getSessionDaoStub(timeout);
                result = client.getSession(getPlatformArgs(), sessionKey);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                mTransport.Close();
            }
            return result;
        }

        public void deleteSession(int timeout, string sessionKey)
        {
            try
            {
                SessionDao.Client client = getSessionDaoStub(timeout);
                client.deleteSession(getPlatformArgs(), sessionKey);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                mTransport.Close();
            }
        }

        public void updateSession(int timeout, string sessionKey, byte[] sessionValue, int expireSecond)
        {
            try
            {
                SessionDao.Client client = getSessionDaoStub(timeout);
                client.updateSession(getPlatformArgs(), sessionKey, sessionValue, expireSecond);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                mTransport.Close();
            }
        }
        private SessionDao.Client getSessionDaoStub(int timeout)
        {
            string ipAddress = ThriftServiceConstant.ServiceIp;
            int port = ThriftServiceConstant.SessionDaoPort;
            TProtocol protocol = getTProtocol(ipAddress, port, timeout);
            return new SessionDao.Client(protocol);
        }

        private PlatformArgs getPlatformArgs()
        {
            PlatformArgs platformArgs = new PlatformArgs();
            platformArgs.SourceDesc = "Soway service";
            return platformArgs;
        }

        private TProtocol getTProtocol(string ipAddress, int port, int timeout)
        {
            TSocket socketTransport = new TSocket(ipAddress, port, timeout);
            mTransport = new TFramedTransport(socketTransport);
            TProtocol protocol = new TCompactProtocol(mTransport);
            mTransport.Open();
            return protocol;
        }
    }
}