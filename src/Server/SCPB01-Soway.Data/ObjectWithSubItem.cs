using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Diagnostics.Contracts;

namespace Soway.Data
{
    


    public abstract   class ObjectWithSubItem <
        Item> //:BusinessObject
        where Item : IItemInterface<ObjectWithSubItem<Item>>,　
        new()
    {

       


        [Soway.Data.Discription.ORM.Column(NoMap=true)]
        public SubItemList<Item> Items { get; set; }


        public ObjectWithSubItem()
        {
          this.Items = new SubItemList<Item>();
     
            
            
            this.Items.AddingNew += Items_AddingNew;

            this.Items.ListChanged += Items_ListChanged;
             
        }

        void Items_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
            {
                  var temp =(this.Items[e.NewIndex]);
            
                  temp.SetParent(this);
            
            }
        }

        void Items_AddingNew(object sender, System.ComponentModel.AddingNewEventArgs e)
           {

               if (e.NewObject == null)
               {
                   var item = new Item();
                   item.SetParent(this);
                   e.NewObject = item;
               }

        }

      

       
 
    }




     


}
