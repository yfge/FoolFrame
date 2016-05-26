using Soway.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class HandlerGetView : Handler
    {
        public ResultView Data;
        private long mViewId;
        public HandlerGetView(ViewOption option)
        {
            PostData = option;
            init(option.ViewId);
        }

        private void init(long viewId)
        {
            IsNeedAuthenticate = true;
            mViewId = viewId;
            Data = new ResultView();
            Result = Data;
        }

        protected override void ImplementBusinessLogic()
        {
            AutoViewFactory factory = new AutoViewFactory();
            View view = factory.GetView(mViewId);

            MapViewToResult(view);
        }

        private void MapViewToResult(View view)
        {
            ViewData viewData = new ViewData();
            viewData.ID = view.id;
            List<ViewItem> viewItems = new List<ViewItem>();
           
            foreach (global::Soway.Model.ViewItem item in view.Items)
            {
                ViewItem viewItem = new ViewItem();
                viewItem.Name = item.Name;
                viewItem.ShowIndex = item.ShowIndex;
                viewItem.Format = item.Format;
                viewItem.IsReadOnly = item.ReadOnly;
                viewItem.PropertyName = item.Property.Name;
                viewItems.Add(viewItem);
            }
            viewData.Items = viewItems;
            viewData.Name = view.Name;
            viewData.Operations = new List<ViewOperation>();
            foreach (var opreaion in view.Operations)
            {
                viewData.Operations.Add(new ViewOperation()
                {
                    Name = opreaion.Name,
                    ID = 0
                    //  ID = opreaion.Operation.
                });
            }
            Data.data = viewData;
            
        }

    }
}