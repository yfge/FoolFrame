using Soway.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model.View
{
    public class AutoViewFactory : IEqualityComparer
    {


        private static Dictionary<string, View> views = new Dictionary<string , View>();
        private static  Dictionary<string, View> detailViews = new Dictionary<string , View>();

        public SqlCon Con { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }

        public View CreateDefaultListView(Model model)
        {
            if (model == null)
                return null;
            if (views.ContainsKey(model.ToString()) == false)
            {

                View view = new View();
                views.Add(model.ToString(), view);
                view.Name = model.Name + "列表";
                view.Items = new List<ViewItem>();
                view.Model = model;
                view.Filter = "";
                view.ViewType = ViewType.列表;
                view.DefaultDetailView = CreateDefaultItemView(model);
                view.Operations = new List<ViewOperation>();
                if (model.ModelType == ModelType.Class)
                {
                    view.Operations.Add(new ViewOperation()
                    {
                        Location = 0,
                        Name = "新建",
                        RequireSelect = false,
                        ResultView = CreateDefaultItemView(model)
                    });

                    view.Operations.Add(new ViewOperation()
                    {
                        Location = 0,
                        Name = "编辑",
                        RequireSelect = true,
                        ResultView = CreateDefaultItemView(model)
                    });
                    if(model.Operations.Count(p=>p.BaseOperationType == BaseOperationType.Delete) > 0)
                    {
                        view.Operations.Add(
                            new ViewOperation(
                                )
                            {
                                Location = 2,
                                Name = "删除",
                                RequireSelect = true,
                                Operation = new OperationView()
                                {
                                    Operation = model.Operations.FirstOrDefault(p => p.BaseOperationType == BaseOperationType.Delete),
                                    IsShow = true,
                                    ConfirmMsg = "确定要删除？该操作不可撤消",
                                    SuccessMsg = "操作成功"
                                }

                          });
                    }
                }

            }
            if(views[model.ToString()].Items .Count ==0)
                foreach (var i in model.Properties.Where(p => p.IsArray == false))
                {
                    views[model.ToString()].Items.Add(new ViewItem()
                    {
                        Name = i.Name,
                        Property = i,
                        Format = (i.Model == null ? "" : ""),
                        ReadOnly = true,
                        ListView = null
                    });
                }

            return views[model.ToString()];
        }

        public View CreateDefaultItemView(Model model)
        {

            if (detailViews.ContainsKey(model.ToString()) == false)
            {
                View view = new View();
                detailViews.Add(model.ToString(), view);
                view.Name = model.Name + "详细";
                view.Items = new List<ViewItem>();
                view.Model = model;
                view.ViewType = ViewType.详细;

            }
            if (detailViews[model.ToString()].Items .Count== 0)
            {
                foreach (var i in model.Properties.Where(p => p.IsArray == false))
                {
                    detailViews[model.ToString()].Items.Add(new ViewItem()
                    {
                        Name = i.Name,
                        Property = i,
                        Format = (i.Model == null ? "" : ""),
                        ReadOnly = false,
                        ListView = null,
                        SelectedView = i.PropertyType == PropertyType.BusinessObject ? CreateDefaultListView(i.Model) : null
                   
                    });
                }
                foreach (var i in model.Properties.Where(p => p.IsArray))
                {
                    detailViews[model.ToString()].Items.Add(new ViewItem()
                    {
                        Name = i.Name,
                        Property = i,
                        ReadOnly = false,
                        ListView = CreateDefaultListView(i.Model),
                        SelectedView = (i.Model != null && i.Model.Properties.Count(p => p.IsArray) > 0) ?
                         (i.PropertyType == PropertyType.BusinessObject ? CreateDefaultItemView(i.Model) : null)
                        : null
                    });
                }
            }
            return detailViews[model.ToString()];
         
        }


        public View GetView(object id)
        {
            var proxy = new Soway.Model.SqlServer.dbContext(this.Con,this.ConFac).GetDetail(Global.ViewMode, id);
            var view = new ModelHelper(this.ConFac).GetFromProxy(proxy) as View; ;
            view.Items = view.Items.OrderBy(p => p.ShowIndex).ToList();
            return view;
        }
        public void SaveView(View obj)
        {
          

            IObjectProxy proxy = new ObjectProxy(Global.ViewMode,this.ConFac);
            new ModelHelper(this.ConFac).SetProxy(ref proxy, obj);
            new Soway.Model.SqlServer.dbContext(this.Con,this.ConFac).Save(proxy);

        }
        public AutoViewFactory(SqlCon con,Context.ICurrentContextFactory conFac)
        {
            this.Con = con;
            this.ConFac = ConFac;
        }
       



        public bool Equals(object x, object y)
        {
            return x.GetHashCode().Equals(y.GetHashCode());
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
