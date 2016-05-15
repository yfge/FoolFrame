using Soway.Model.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.InputQuery
{
    class HandlerInputQuery : Handler
    {
        private InputQueryOption Option { get; set; }   
        private ResultInputQuery QueryResult { get; set; }

        public HandlerInputQuery(InputQueryOption option)
        {
            this.Option = option;
            this.QueryResult = new ResultInputQuery();
            this.Result = QueryResult;
            this.PostData = this.Option;
            IsNeedAuthenticate = true;
        }
        protected override void ImplementBusinessLogic()
        {

            AutoViewFactory factory = new AutoViewFactory(this.Info.AppSqlCon, this);
            View view = factory.GetView(Option.ViewName);

             
         
            var sql = GetViewSql(view);
          

            var item = view.Items.FirstOrDefault(p => p.Name == this.Option.ViewItemId);
            if (item.SelectedView != null)
                sql = GetViewSql(item.SelectedView);
            var context = new Soway.Model.Context.InputContext(sql);

            List<Soway.Model.Context.InputQueryResult> list = null;
            Soway.Model.ModelBindingList source = null;
            var sourceExp = "";
            sourceExp = item.ItemSourceExp;
            if (String.IsNullOrEmpty(sourceExp))
                sourceExp = item.Property.Source;

            if (String.IsNullOrEmpty(sourceExp) == false )
            {
                if (Option.IsAdded && String.IsNullOrEmpty(Option.OwnerId) == false && view.Model.Owner != null)
                {
                    var parentObj = new Soway.Model.SqlServer.dbContext(sql, this).GetDetail(view.Model.Owner, Option.OwnerId);
                    var addedItem = new Soway.Model.ObjectProxy(item.Property.Model, this);
                    addedItem.Owner = parentObj;
                    source = addedItem[sourceExp] as Soway.Model.ModelBindingList;


                }
                else if(String.IsNullOrEmpty(this.Option.ObjID)==false)
                {
                    var obj = new Soway.Model.SqlServer.dbContext(sql, this).GetDetail(view.Model,
                      this.Option.ObjID);
                    
                        source = new Soway.Model.SqlServer.dbContext(sql, this).GetDetail(view.Model,
                          this.Option.ObjID)[item.Property.Source] as Soway.Model.ModelBindingList;
                }
                
            }
              list = context.Query(item.Property.Model, Option.Text,source, null, 5);
            this.QueryResult.Items = new List<QueryItem>();
            foreach(var dataitem in list)
            {
                this.QueryResult.Items.Add(new QueryItem()
                {
                    Id = dataitem.id.ToString(),
                    Text = dataitem.Text
                });
            }
        }
    }
}
