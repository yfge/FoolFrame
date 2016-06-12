using Soway.Service.bean;
using Soway.Service.Login.V1;
using Soway.Service.ThriftClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{


    public class HandlerCheckCode : Handler
    {
        public CheckCode Data;
        public HandlerCheckCode()
        {
            IsNeedAuthenticate = false;
            init();

        }

        private void init()
        {
            Data = new CheckCode();
            Result = Data;
        }


        protected override void ImplementBusinessLogic()
        {

            ChkCodeImg chkImg = GetCheckCode();
            this.Data.ChkCodeImg = ImageHelper.GetImageStr(chkImg.Image);
            this.Data.Key = chkImg.Key;
            this.Data.Code = chkImg.CheckCode;
        }

        public bool Check(CheckCode code)
        {
            if (code.Key == null || code.Code == null)
            {
                Errors = new ErrorInfo(ErrorDescription.CHECK_CODE_ERROR, ErrorDescription.CHECK_CODE_ERROR_MSG,true);
                return false;
            }
            try
            {
                ISessionDaoStub stub = new SessionDaoStubFac().Get(); 
                byte[] content = stub.getSession(3000, code.Key);
                string codeStr = System.Text.UTF8Encoding.UTF8.GetString(content);
                if (codeStr.Trim().ToLower()
                    .Equals(code.Code.Trim().ToLower()))
                {
                    return true;
                }
            }
            catch (Exception e)
            {

                //SowayLog.Log.Error("set session fail", e);

                Errors = new ErrorInfo(ErrorDescription.CODE_SYSTEM_ERROR, ErrorDescription.MESSAGE_SYSTEM_ERROR,true);
            }
            return false;
        }
    }
}