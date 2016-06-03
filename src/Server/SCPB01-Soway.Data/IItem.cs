using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{

    public abstract class IItem<T> : IItemInterface<T>,IBusinessObject
    {

        //[Discription.ORM.Column(ColumnName = "BO_Id", IsKey = true, IsAutoGenerate = Soway.Data.Discription.ORM.GenerationType.OnInSert)]
        //public long Id { get; set; }
        


        public abstract T Parent { get; set; }


        public　 void SetParent(object ob)
        {
            Parent = (T) ob;
        }
    }
}
