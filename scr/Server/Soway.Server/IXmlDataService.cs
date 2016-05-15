using Soway.Model;
using Soway.Model.Context;
using Soway.Model.View;
using Soway.Service.QueryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Soway.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISowayXmlDataService" in both code and config file together.
    [ServiceContract]
    public interface IXmlDataService
    {
        [OperationContract]
        ResultView GetListView(ListView.GetViewOption option);

        [OperationContract]
        ResultQuery QueryData(QueryDataOption option);

        [OperationContract]
        ResultDataDetail QueryDataDetail(long viewId,string objId);

        [OperationContract]
        ResultOperation RunOperation(OperationOption option);

        [OperationContract]
        ResultInitApp InitApp(String AppId, String AppKey);


       

    }
}
