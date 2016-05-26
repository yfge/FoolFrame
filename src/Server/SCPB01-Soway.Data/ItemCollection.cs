using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{
    public class SubItemList<I> :
        System.ComponentModel.BindingList<I>
        
     
    {



         
        protected override void RemoveItem(int index)
        {

            if (this.RaiseListChangedEvents)
            {
                DeleteList.Add(this[index]);
                this.AddedList.Remove(this[index]);
                base.RemoveItem(index);
            }
        }
        protected override void OnListChanged(System.ComponentModel.ListChangedEventArgs e)
        {
     
           
            
            
            switch (e.ListChangedType)
            {

                case System.ComponentModel.ListChangedType.ItemAdded:
                  
                    AddedList.Add(this[e.NewIndex]);
                    
                    break;
                case System.ComponentModel.ListChangedType.ItemChanged:
                    UpdatedList.Add(this[e.NewIndex]);
                    break;
                case System.ComponentModel.ListChangedType.ItemDeleted:
                 //   DeleteList.Add(this[e.NewIndex]);
                    break;
                default:
                    break;



            }
            base.OnListChanged(e);
        }
         private List<I> added= new List<I>();
        public List<I> AddedList { get { return added; } }
        private List<I> updated = new List<I>();
        public List<I> UpdatedList {get{return updated;}}
        private List<I> deleted = new List<I>();
        public List<I> DeleteList { get{return deleted ;}}


    }



   


 
}
