using Soway.Model;
using Soway.Model.View;
using Soway.Service.bean;
using Soway.Service.Enum;
using Soway.Service.InputQuery;
using Soway.Service.ItemView;
using Soway.Service.Login.V1;
using Soway.Service.Login.V2;
using Soway.Service.New;
using Soway.Service.QueryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Soway.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDataService
    {

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getlistview", ResponseFormat = WebMessageFormat.Json)]
        ResultView GetListView(ListView.GetViewOption option);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "querydata", ResponseFormat = WebMessageFormat.Json)]
        ResultQuery QueryData(QueryDataOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "querydatadetail", ResponseFormat = WebMessageFormat.Json)]
        ResultDataDetail QueryDataDetail(QueryDataDetailOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "runoperation", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultOperation RunOperation(OperationOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "initapp",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]

        ResultInitApp InitApp(InitOption option);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "login",
            RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json)]
        ResultLogin Login(Login.LoginOperation option);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "loginv2",
      RequestFormat = WebMessageFormat.Json,
      ResponseFormat = WebMessageFormat.Json)]
        ResultLogin LoginV2(Login.LoginOperation option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getmain",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        User.MainResult GetMainInfo(string token);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getsubmenu",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        ResultGetSubAuth GetSubMenu(GetSubAuthOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getcheckcode",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        CheckCode GetCheckCode();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "checkcode",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        bool ValidatCheckCode(CheckCode checkCode);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getapp",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        User.AppResult GetAppInfo(PostDataOption postdata);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getuserinfo",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        User.UserResult GetUserInfo(PostDataOption postdata);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getreaditemview",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        ReadItemView GetReadItem(ViewOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getenums",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        GetEnumResult GetEnums(GetEnumOption option);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "inputquery",
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        ResultInputQuery InputQuery(InputQuery.InputQueryOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "saveobj", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Result SaveObj(ObjDetail.SaveObjOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "initnew", RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
        ResultDataDetail InitNewObj(InitNewOption option);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "savenewobj", RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
        Result SaveNewObj(NewObjOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "logout", RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
        Result Logout(PostDataOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getmkqview", RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        Report.ResultQueryModel Get(Report.GetReportModelOption option);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getrpt", RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        Report.ReportResult GetReport(Report.MakeReportOption option);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "saverpt", RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        Result SaveReport(Report.SavedReportOption option);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getmsg", RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        Message.GetMessageResult GetMessage(PostDataOption option);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "getnotify", RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        Message.GetNotifyResult GetNotify(PostDataOption option);

    }

}
