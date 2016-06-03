using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    class HandlerGetReportModel : Handler
    {


        public HandlerGetReportModel(GetReportModelOption opt)
        {
            this.PostData = opt;
            this.Opt = opt;
            this.ReturnData = new ResultQueryModel();
        
            this.Result = this.ReturnData;
            this.IsNeedAuthenticate = true;

        }

        public GetReportModelOption Opt { get; private set; }
        public ResultQueryModel ReturnData { get; private set; }

        protected override void ImplementBusinessLogic()
        {
            var view = new Soway.Model.View.AutoViewFactory(this.Info.AppSqlCon, this).GetView(this.Opt.ViewId);
            var queryModel = new Soway.Model.Query.QueryFactory(this.Info.AppSqlCon, this).GetQueryModel(view);
            this.ReturnData.Cols = new List<QueryCol>();
            foreach (var col in queryModel.Columns)
            {
                var item = new QueryCol()
                {
                    Name = col.ShowName,
                    ID = col.ID,
                    PrpType = col.DataType,
                    ModelId = col.ModelId,
                    States = col.States
                    

                };
                item.CompareTypes = new List<CompareOpItem>();
                foreach (var compare in QueryCache.GetCompareType(this.Info.AppSqlCon, col.DataType))
                {
                    item.CompareTypes.Add(new CompareOpItem()
                    {
                        ID = compare.ID.ToString(),
                        Name = compare.ShowName
                    });
                }

                item.QueryTypes = new List<QueryType>();
                
                foreach (var select in QueryCache.GetSelectedType(this.Info.AppSqlCon, col.DataType)){

                    item.QueryTypes.Add(new QueryType()
                    {
                        ID = select.ID.ToString(),
                        Name = select.Show,

                    });
                }
                this.ReturnData.Cols.Add(item);
            }

           
        }
    }
}
