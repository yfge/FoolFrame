using Soway.Model;
using Soway.Service.Detail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Soway.Service
{
    public class DataFormator
    {
        public List<ObjValuePair> IObjectProxyToValue(IObjectProxy objectProxy, List<
            global::Soway.Model.ViewItem> viewitems)
        {
            List<ObjValuePair> valueList = new List<ObjValuePair>();

            foreach (global::Soway.Model.ViewItem viewitem in viewitems)
            {
                valueList.Add(GetAddItem(objectProxy, viewitem));
                //if (property.IsArray)
                //{
                //    dynamic iobList = objectProxy[property];
                //    foreach (IObjectProxy p in iobList)
                //    {
                //        ObjectKeyValuePair pair = toValuePair(p);
                //        valueList.Add(pair);
                //    }
                //}
                //else
                //{
                //    object unknownOb = objectProxy[property];
                //    String str = (unknownOb ?? "").ToString();
                //    ObjectKeyValuePair pair = new ObjectKeyValuePair();
                //    pair.ObjectId = str;
                 
                //    if(property.PropertyType == Data.PropertyType.Enum)
                //    {
                //        int enumvalue = 0;
                //        //(int)unknownOb;
                //        if ( int.TryParse(str,out enumvalue)&&property.Model != null)
                //        {
                //            var enumdefine = property.Model.EnumValues.FirstOrDefault(p => p.Value == enumvalue);

                //            if (enumdefine != null)
                //                str = enumdefine.String;
                //        }


                //    }
                //    pair.Value = str;
                //    valueList.Add(pair);
                //}
            }
            return valueList;
        }

        private static ObjectKeyValuePair toValuePair(IObjectProxy iob)
        {
            ObjectKeyValuePair pair = new ObjectKeyValuePair();
            pair.ObjectId = iob.ID.ToString();
            pair.Value = iob.ToString();
            return pair;
        }

        public static DataDetail IObjectProxyToDetail(IObjectProxy objectProxy, Soway.Model.View.View view)
        {
            DataDetail result = new DataDetail();

            if(objectProxy.ID!=null)
            result.ObjId = objectProxy.ID.ToString();
            result.Name = view.Name;
            result.Model = view.Model.Name;
          
            result.SimpleData = new List<ObjValuePair>();
            result.Items = new List<PropertyDataItems>();
            if (objectProxy.Owner != null)
                result.ParentId = (objectProxy.Owner.ID ?? "").ToString();
            foreach (var item in view.Items.Where(p => p.Property.IsArray == false))
            {
               
               var addItem= GetAddItem(objectProxy, item);

                result.SimpleData.Add(addItem);
            }
            foreach (var item in view.Items.Where(p => p.Property.IsArray == true))
            {
                var PropertyItems = new PropertyDataItems();

                PropertyItems.Properties = new List<ReadItemViewItem>();
                PropertyItems.Items = new List<DataItem>();
                PropertyItems.Name = item.ListView.Name;
                PropertyItems.ItemName = item.Name;
                PropertyItems.PrpId = item.Property.Name;

               
                if (item.EditView != null)
                    PropertyItems.DetailViewId = item.EditView.ID;
                if(item.ListView!=null)
                PropertyItems.ListViewId = item.ListView.ID;
                if (item.SelectedView != null)
                {
                    PropertyItems.SelectedView = item.SelectedView.ID;
                    PropertyItems.SelectFromExists = true;
                }
                foreach (var viewItem in item.ListView.Items)
                {
                    PropertyItems.Properties.Add(
                        new ReadItemViewItem()
                        {
                            ID = viewItem.Name,
                            PrpModelId = (viewItem.Property.Model == null ? 0 : viewItem.Property.Model.ID),
                            Name = viewItem.Name,
                            PrpType = viewItem.Property.PropertyType,
                            PrpId = viewItem.Property.Name,
                            PrpShowName = viewItem.Name,
                            ReadOnly = viewItem.ReadOnly,
                            EditType = viewItem.EditType


                        });
                }
                var items = objectProxy[item.Property] as Model.ModelBindingList;
                foreach(var pitem in items)
                {
                    var detailItem = new DataItem()
                    {
                        DataID = (pitem.ID ?? "").ToString(),
                        Values = new List<ObjValuePair>()
                    };
                    PropertyItems.Items.Add(detailItem
                   );
                    foreach(var propertyItem in item.ListView.Items)
                    {
                        detailItem.Values.Add(
                            GetAddItem(pitem,propertyItem)
                          );
                    }
                }
             
                result.Items.Add(PropertyItems);
            }
            return result;
        }

        private static ObjValuePair GetAddItem(IObjectProxy objectProxy, Model.ViewItem item)
        {
            var addItem = new ObjValuePair()
            {
                PrpId = item.Property.Name,
                PrpShowName = item.Name,
                ObjId = (objectProxy[item.Property.Name] ?? "").ToString(),
                FmtValue = (objectProxy[item.Property.Name] ?? "").ToString(),
                PrpType = item.Property.PropertyType,
                ReadOnly = item.ReadOnly,
                 EditType = item.EditType,
                PrpModelId = item.Property.Model == null ? 0 : item.Property.Model.ID,
            };

            if (item.Property.PropertyType == Data.PropertyType.Date)
            {
                var date = (DateTime)objectProxy[item.Property.Name];
                if (date != null)
                    addItem.FmtValue = date.ToString("yyyy-MM-dd");
            }

            if (item.Property.PropertyType == Data.PropertyType.Time)
            {
                var date = (DateTime)objectProxy[item.Property.Name];
                if (date != null)
                    addItem.FmtValue = date.ToString("HH:mm:ss");
            }

            if (item.Property.PropertyType == Data.PropertyType.BusinessObject)
            {
                addItem.FmtValue = (objectProxy[item.Property.Name] ?? "").ToString();
                if (objectProxy[item.Property.Name] != null)
                    addItem.ObjId = ((objectProxy[item.Property.Name] as IObjectProxy).ID??"").ToString();
                else
                    addItem.ObjId = "";
            }
            else if (item.Property.PropertyType == Data.PropertyType.Enum)
            {
                var valueItem = item.Property.Model.EnumValues.FirstOrDefault(p => p.Value == System.Convert.ToInt32(objectProxy[item.Property.Name]));
                if (valueItem != null)
                {
                    addItem.FmtValue = valueItem.String;
                    addItem.ObjId = valueItem.Value.ToString();

                }
                else
                {
                    addItem.FmtValue = (objectProxy[item.Property.Name] ?? "").ToString();
                    addItem.ObjId = (objectProxy[item.Property.Name] ?? "").ToString();
                }
            }
            return addItem;
        }


        public static void  ObjUpdateToProxy(ObjDetail.Obj obj,IObjectProxy proxy)
        {
            
            
            foreach(var i in obj.Propertyies)
            {
                try {
                    proxy[i.Key] = i.Value;
                }catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(String.Format("Set value of {0} error,value is :{1},message:{2}", i.Key, i.Value,ex.ToString()));
                }
            }
            foreach(var items in obj.Itemproperties)
            {
                if (items.Items != null && items.Items.Count > 0)
                {
                    var itemsproxies = proxy[items.Key] as Soway.Model.ModelBindingList;
                    if (items.Items != null)
                        foreach (var item in items.Items)
                        {
                            var objItem = itemsproxies.FirstOrDefault(p => (p.ID ?? "").ToString() == item.ItemId);
                            if (objItem != null)
                            {
                                foreach (var key in item.Propertyies)
                                {
                                    try
                                    {

                                        objItem[key.Key] = key.Value;
                                    }
                                    catch (Exception ex)
                                    {
                                        System.Diagnostics.Trace.WriteLine(String.Format("Set value of {0} error,value is :{1},message:{2}", key.Key, key.Value, ex.ToString()));
                                    }
                                }
                            }

                        }
                }
                if (items.AddedItems != null && items.AddedItems.Count > 0)
                {
                    var itemsproxies = proxy[items.Key] as Soway.Model.ModelBindingList;
                    foreach (var item in items.AddedItems)
                    {
                        var addedItem = itemsproxies.AddNew();
                        foreach (var key in item.Propertyies)
                        {
                            
                            addedItem[key.Key] = key.Value;
                         
                        }
                        if (item.IsExist)
                            addedItem.ID = item.ItemId;

                    }
                }
                if (items.DelteItems != null && items.DelteItems.Count > 0)
                {
                    var itemsproxies = proxy[items.Key] as Soway.Model.ModelBindingList;
                    foreach (var item in items.DelteItems)
                    {
                        var deleteItme = itemsproxies.FirstOrDefault(p => (p.ID ?? "").ToString() == item.ItemId);
                        if (deleteItme != null)
                            itemsproxies.Remove(deleteItme);
                    }

                }
             
            }
        }
    }
}