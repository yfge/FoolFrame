using Soway.Model;
using Soway.Model.View;
using Soway.Service.Login.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using Soway.Service.Login;
using Soway.Service.Login.V2;
using Soway.Service.QueryData;
using Soway.Service.bean;
using Soway.Service.User;
using Soway.Service.ItemView;
using Soway.Service.Enum;
using Soway.Service.InputQuery;
using Soway.Service.ObjDetail;
using Soway.Service.New;
using Soway.Service.Report;
using Soway.Service.Message;

namespace Soway.Service
{

    public class DataService : IDataService
    {

        public DataService()
        {
            
            //DataBase.GetInstance().InitDataBaseInfo();
        }

        public ResultView GetListView(ListView.GetViewOption option)
        {
            Handler handler = new HandlerGetView(option);
            handler.Invoke();
            return (ResultView)handler.Result;
        }



        public ResultQuery QueryData(QueryDataOption option)
        {
            Handler handler = new HandlerQueryData(option);
            handler.Invoke();
            return (ResultQuery)handler.Result;
        }

        public ResultDataDetail QueryDataDetail(QueryDataDetailOption option)
        {
            Handler handler = new HandlerQueryDataDetail(option);
            handler.Invoke();
            return (ResultDataDetail)handler.Result;
        }

        public ResultOperation RunOperation(OperationOption option)
        {
            Handler handler = new HandlerRunOperation( option);
            handler.Invoke();
            return (ResultOperation)handler.Result;
        }

        public ResultInitApp InitApp(InitOption option)
        {
            Handler handler = new HandlerInitApp(option.AppId, option.AppKey);
            handler.Invoke();
           
            return (ResultInitApp)handler.Result;
        }

        public ResultLogin Login(Login.LoginOperation option)
        {
          
            Handler handler = new LoginHandler(option);
            handler.Invoke();
            return (ResultLogin)handler.Result;
        }

        public ResultLogin LoginV2(LoginOperation option)
        {

            Handler handler = new Login.V2.HandlerLogin(option);
            handler.Invoke();
            return (ResultLogin)handler.Result;
        }

        public ResultGetSubAuth GetSubMenu(GetSubAuthOption option)
        {
            Handler handler = new Login.V2.HandlerGetSubAuth(option);
            handler.Invoke();
            return (ResultGetSubAuth)handler.Result;
        }

        public MainResult GetMainInfo(string token)
        {
            Handler hander = new  HandlerGetMain(new PostDataOption() { Token = token });
            hander.Invoke();
            return (MainResult)hander.Result;

        }

        public CheckCode GetCheckCode()
        {
            Handler hander = new HandlerCheckCode();
            hander.Invoke();
            return (CheckCode)hander.Result;
        }

        public bool ValidatCheckCode(CheckCode checkCode)
        {
            HandlerCheckCode hander = new HandlerCheckCode();
            return  hander.Check(checkCode);
        }

        public AppResult GetAppInfo(PostDataOption postdata)
        {
            throw new NotImplementedException();
        }

        public UserResult GetUserInfo(PostDataOption postdata)
        {
            throw new NotImplementedException();
        }

        public ReadItemView GetReadItem(ViewOption option)
        {
            Handler hander = new HandlerGetReadItemView(option);
            hander.Invoke();
            return (ReadItemView)hander.Result;

        }

        public GetEnumResult GetEnums(GetEnumOption option)
        {
            Handler handler = new HandlerGetEnum(option);
            handler.Invoke();
            return (GetEnumResult)handler.Result;
        }

        public ResultInputQuery InputQuery(InputQueryOption option)
        {
            Handler hander = new HandlerInputQuery(option);
            hander.Invoke();
            return (ResultInputQuery)hander.Result;
        }

        public Result SaveObj(SaveObjOption option)
        {
            
            Handler handler =new Soway.Service.ObjDetail.HandlerSaveObj(option);
            handler.Invoke();
            return handler.Result;
        }

        public ResultDataDetail InitNewObj(InitNewOption option)
        {
            Handler handler =new Soway.Service.New.HandlerInitNew(option);
            handler.Invoke();
            return (ResultDataDetail)handler.Result;
            
        }

        public Result SaveNewObj(NewObjOption option)
        {
            Handler handler = new Soway.Service.New.HandlerSaveNew(option);
            handler.Invoke();
            return handler.Result;
        }

        public Result Logout(PostDataOption option)
        {
            Handler handler = new Soway.Service.User.LogoutHandler(option);
            handler.Invoke();
            return handler.Result;
        }

        public ResultQueryModel Get(GetReportModelOption option)
        {

            Handler handler = new Soway.Service.Report.HandlerGetReportModel(option);
            handler.Invoke();
            return (ResultQueryModel)handler.Result;
        }

        public ReportResult GetReport(MakeReportOption option)
        {
            Handler handler =new  HandlerMakeReport(option);
            handler.Invoke();
            return (ReportResult)handler.Result;
        }

        public Result SaveReport(SavedReportOption option)
        {
            Handler handler = new HandlerSaveReport(option);
            handler.Invoke();
            return handler.Result;
        }

        public GetMessageResult GetMessage(PostDataOption option)
        {
            Handler handler = new Message.HandlerGetMessage(option);
            handler.Invoke();
            return (GetMessageResult)handler.Result;
        }

        public GetNotifyResult GetNotify(PostDataOption option)
        {
            throw new NotImplementedException();
        }
    }
}
