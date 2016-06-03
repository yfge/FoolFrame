using org.soldier.platform.svr_platform.comm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Thrift.Protocol;
using Thrift.Transport;

namespace Soway.Service.ThriftClient
{
    public class TokenAoStub : ITokenAoStub
    {
        private TTransport mTransport;

        public string getToken(int timeout, string key)
        {
            string result = String.Empty;
            try
            {
                TokenAo.Client client = getTokenAoStub(timeout);
                result = client.getToken(getPlatformArgs(),   key);
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

        public void updateToken(int timeout, 
            string key, 
            String token, 
            int expireSeconds)
        {
            try
            {
                TokenAo.Client client = getTokenAoStub(timeout);
                client.updateToken(getPlatformArgs(), key, token, expireSeconds);
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

        public void deleteToken(int timeout, string key)
        {
            try
            {
                TokenAo.Client client = getTokenAoStub(timeout);
                client.deleteToken(getPlatformArgs(), key);
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

         public string checkToken(int timeout, string token)
        {
            string result;
            try
            {
                TokenAo.Client client = getTokenAoStub(timeout);
                result = client.checkToken(getPlatformArgs(), token);
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

        private TokenAo.Client getTokenAoStub(int timeout)
        {
            // string ipAddress = ThriftServiceConstant.ServiceInternalIp;
          
        
            string ipAddress = ThriftServiceConstant.ServiceIp;
            int port = ThriftServiceConstant.TokenAoPort;
            TProtocol protocol = getTProtocol(ipAddress, port, timeout);
            return new TokenAo.Client(protocol);
        }

        //public SessionDao.Client getSessionDaoStub()
        //{
        //    string ipAddress = ThriftServiceConstant.ServiceIp;
        //    int port = ThriftServiceConstant.SessionDaoPort;
        //    TProtocol protocol = getTProtocol(ipAddress, port);
        //    return new SessionDao.Client(protocol);
        //}

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