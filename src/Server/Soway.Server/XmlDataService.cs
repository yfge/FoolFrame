//using Soway.Model;
//using Soway.Model.View;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.ServiceModel;
//using System.Text;

//namespace Soway.Service
//{
//    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SowayXmlDataService" in code, svc and config file together.
//    // NOTE: In order to launch WCF Test Client for testing this service, please select SowayXmlDataService.svc or SowayXmlDataService.svc.cs at the Solution Explorer and start debugging.
//    public class XmlDataService : IXmlDataService
//    {
//        public XmlDataService()
//        {
//            DataBase.GetInstance().InitDataBaseInfo();
//        }

//        public ResultView GetView(ViewOption option)
//        {
//            Handler handler = new HandlerGetView(option);
//            handler.Invoke();
//            return (ResultView)handler.Result;
//        }

//        public ResultQuery QueryData(long viewId, string filter)
//        {
//            Handler handler = new HandlerQueryData(viewId, filter);
//            handler.Invoke();
//            return (ResultQuery)handler.Result;
//        }

//        public ResultDataDetail QueryDataDetail(long viewId, string objId)
//        {
//            Handler handler = new HandlerQueryDataDetail(viewId, objId);
//            handler.Invoke();
//            return (ResultDataDetail)handler.Result;
//        }

//        public ResultOperation RunOperation(OperationOption option)
//        {
//            Handler handler = new HandlerRunOperation(option);
//            handler.Invoke();
//            return (ResultOperation)handler.Result;
//        }




//        public ResultInitApp InitApp(string AppId, string AppKey)
//        {

//            Handler hander = new HandlerInitApp(AppId, AppKey);
//            hander.Invoke();
//            return (ResultInitApp)hander.Result;
//        }
//    }
//}
