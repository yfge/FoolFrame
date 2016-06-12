using Soway.Model.View;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class HandlerGetView : Handler
    {
        public ResultView Data;
        private ListView.GetViewOption Option;
        public HandlerGetView(ListView.GetViewOption option)
        {
            init(option);
        }

        private void init(ListView.GetViewOption option)
        {
            IsNeedAuthenticate = true;
            Option = option;
            this.PostData = option;
            Data = new ResultView();
            Result = Data;
        }

        protected override void ImplementBusinessLogic()
        {
            
            Data.Token = this.PostData.Token;
            AutoViewFactory factory = new AutoViewFactory(Info.AppSqlCon,this);
            var context = new Soway.Model.SqlServer.DynamicContext(
                Info.AppSqlCon.ToString(), this);
            var view = context.GetById(typeof(Soway.Model.View.View), Option.ViewId);
           // View view = factory.GetView(Option.ViewId);
            MapViewToResult(view);
        }

        private void MapViewToResult(dynamic view)
        {
            ViewData viewData = new ViewData();
            viewData.ID = view.ID;
            List<ViewItem> viewItems = new List<ViewItem>();
           
            foreach (dynamic item in view.Items)
            {
                ViewItem viewItem = new ViewItem();
                viewItem.Name = item.Name;
                viewItem.ShowIndex = item.ShowIndex;
                viewItem.Format = item.Format;
                viewItem.IsReadOnly = item.ReadOnly;
                
                viewItem.PropertyName = item.Property ==null ?"": item.Property.Name;
                viewItem.ListViewId = (item.ListView == null ? 0 : item.ListView.ID);
                viewItem.ListViewType = (item.ListView == null ? 0 :(int) item.ListView.ViewType);
                viewItem.Width = item.Width;
                viewItem.ShowIndex = item.ShowIndex;
                viewItem.PropertyType =( Soway.Data.PropertyType) (item.Property != null ? item.Property.PropertyType : Soway.Data.PropertyType.String);
                viewItem.EditType =  (ItemEditType) item.EditType;
               
                if(item.Property!= null && item.Property.Model != null)
                {
                    viewItem.PropertyModel = item.Property.Model.ID;
                }
             
                if (item.ItemFile != null  )
                {
                    viewItem.ViewFile = item.ItemFile.FileName;

                }
                viewItems.Add(viewItem);
            }
            viewData.Items = viewItems;
            viewData.Name = view.Name;
            viewData.Operations = new List<ViewOperation>();
            viewData.AutoFreshTime = view.AutoFreshInterval;
            if(view.DefaultDetailView != null)
            viewData.DetailViewId = view.DefaultDetailView.ID;
            if (view.ViewFile != null)
                viewData.TempFile = view.ViewFile.FileName;
            else
                viewData.TempFile = "";
            viewData.ShowType =( Model.ViewType) view.ViewType;
            foreach (dynamic opreaion in view.Operations)
            {
                viewData.Operations.Add(new ViewOperation()
                {
                    Name = opreaion.Name,
                    ID =opreaion.Operation==null?0:opreaion.Operation.Operation.ID
                    ,
                    ViewID = opreaion.ResultView==null ? 0 : opreaion.ResultView.ID,
                    RequireSelect = opreaion.RequireSelect

                    //  ID = opreaion.Operation.
                });
            }
            Data.data = viewData;


            
            
        }

    }
}