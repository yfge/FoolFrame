using Soway.Service.Detail;
using Soway.Service.ItemView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.ModelAdapter
{
    public class ViewAdapter
    {
        public ReadItemView GetReadItemView(Soway.Model.View.View view)
        {
            var data = new ReadItemView();
            data.ViewName = view.Name;
            data.ViewId = view.ID;
            data.Items = new List<ReadItemViewItem>();
            data.DetailViews = new List<ReadItemViewDetailProperty>();
            foreach(var item in view.Items)
            {
             
                if(item.Property.IsArray==false )
                data.Items.Add(
                    new ReadItemViewItem()
                    {
                        Name = item.Name,
                        Index = item.ShowIndex,
                        PrpType = item.Property.PropertyType,
                        PrpId = item.Property.Name ,
                        EditType =item.EditType,
                        ReadOnly=item.ReadOnly
                        
                    });
                else if (item.EditView !=null){

                    var detail = new ReadItemViewDetailProperty();
                    detail.Name = item.Name;
                    detail.PrpId = item.Property.PropertyName;
                    detail.Items = new List<ReadItemViewItem>();
                    foreach(var pItem in item.EditView.Items)
                    {
                        detail.Items.Add(new ReadItemViewItem()
                        {
                            Name = pItem.Name,
                            Index = pItem.ShowIndex,
                            PrpType = pItem.Property.PropertyType,
                            PrpId = pItem.Property.Name
                        });
                    }
                    data.DetailViews.Add(detail);
                }
            }
            return data;
           
          
        }
    }
}
