using Soway.Model.Context;
using Soway.Model.View;
using Soway.Service.QueryData;
using Soway.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public class HandlerQueryData : Handler
    {
        public ResultQuery Data;
        private QueryDataOption Option;

        public HandlerQueryData(QueryDataOption option)
        {
        //    init(viewId, filter);
        //}

            Data = new ResultQuery();
            IsNeedAuthenticate = true;
            Result = Data;
            Option = option;
            this.PostData = option;
        }



        protected override void ImplementBusinessLogic()
        {
        
            AutoViewFactory factory = new AutoViewFactory(this.Info.AppSqlCon,this);
            View view = factory.GetView(Option.ViewId);
            var sql = GetViewSql(view);
            ListViewQueryContext context = new ListViewQueryContext(sql,this);
            var orderby = view.Items.OrderBy(p => p.ShowIndex).First();
            QueryResult queryResult = context.Query(view, Option.PageIndex, Option.PageSize,
                Option.QueryFilter,orderby,Model.OrderByType.DESC);
            Data.TotalItem = queryResult.TotalItemsCount;
            Data.TotalPage = queryResult.TotalPagesCount;
            Data.PageIndex = queryResult.CurrentPageIndex;
            List<QueryKeyValueResult> result = new List<QueryKeyValueResult>();
            List<ObjectKeyValuePair> valueList = new List<ObjectKeyValuePair>();
            Data.FreshTime = this.GetDateTime();
            Data.Cols = view.Items.Where(p=>p.EditType != ItemEditType.Format).Select(P => P.Name).ToList();

            Data.AutoFreshTime = view.AutoFreshInterval;
            foreach (ViewResultItem item in queryResult.CurrentResult)
            {
                QueryKeyValueResult data = new QueryKeyValueResult();
                data.RowIndex = item.RowIndex;
                data.Id = item.ObjectProxy.ID;
                data.Items = new DataFormator().IObjectProxyToValue(item.ObjectProxy,
                    view.Items).ToList();

                var fmt = view.Items.FirstOrDefault(p => p.EditType == ItemEditType.Format);
                if (fmt != null)
                    data.RowFmt = (item.ObjectProxy[fmt.Property.Name] ?? "").ToString();
                result.Add(data);
            }

            Data.Data = result;


        }
    }
}