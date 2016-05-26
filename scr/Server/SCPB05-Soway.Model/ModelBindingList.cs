using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model
{
    public class ModelBindingList:System.ComponentModel.BindingList<IObjectProxy>
    {
        public ModelBindingList(SqlCon con)
        {
            this.Con = con;
        }
        private Property Property;
        public ModelBindingList(Property property, IObjectProxy owner,Context.ICurrentContextFactory conFac)
        {

            this.Property = property;
            this.Owner = owner;
            this.ConFac = conFac;
        }

        private IObjectProxy Owner { get; set; }
        public SqlCon Con { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }

        protected override void OnAddingNew(System.ComponentModel.AddingNewEventArgs e)
        {

            e.NewObject = new SqlDataProxy(this.Property.Model,this.ConFac, LoadType.Complete,this.Con) { Owner = this.Owner };
          
            foreach (var trigger in this.Property.Triggers.Where(p => p.PropertyTriggerType == PropertyTriggerType.ItemsAdd))
                new ModelMethodContext(this.Con,this.ConFac).ExcuteOperation(e.NewObject as IObjectProxy, trigger as IOperation);
            base.OnAddingNew(e);
        }


        protected override void InsertItem(int index, IObjectProxy item)
        {
            base.InsertItem(index, item);
            item.Owner = this.Owner;
        }
         
        protected override void RemoveItem(int index)
        {
            ToRemove.Add(this[index]);
            foreach (var trigger in this.Property.Triggers.Where(p => p.PropertyTriggerType == PropertyTriggerType.ItemsDelete))
                new ModelMethodContext(this.Con,this.ConFac).ExcuteOperation(this[index], trigger as IOperation);
            base.RemoveItem(index);
        }

        internal List<IObjectProxy> ToRemove = new List<IObjectProxy>();
    }
}
