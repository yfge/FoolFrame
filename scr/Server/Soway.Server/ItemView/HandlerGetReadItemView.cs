using Soway.Service.ModelAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ItemView
{
    class HandlerGetReadItemView : Handler
    {
        ViewOption Option;
        ReadItemView Data;
         
        protected override void ImplementBusinessLogic()
        {
            
            var data = new ViewAdapter().GetReadItemView(new Soway.Model.View.AutoViewFactory(
                Info.AppSqlCon,this).GetView(Option.ViewId));

            data.Token = this.Option.Token;
            this.Result = data;

        }

        public HandlerGetReadItemView(ViewOption option)
        {
            IsNeedAuthenticate = true;
            this.Option = option;
            this.PostData = option;
            Data = new ReadItemView();
            Data.Token = this.Option.Token;
            this.Result = Data;

        }
    }
}
