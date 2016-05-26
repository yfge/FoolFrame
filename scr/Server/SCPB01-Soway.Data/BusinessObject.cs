using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{
    public abstract class BusinessObject : Soway.Data.IBusinessObject
    { 
        [Discription.ORM.Column(ColumnName="BO_Id",IsKey=true,IsAutoGenerate=Soway.Data.Discription.ORM.GenerationType.OnInSert)]
        [Discription.Display.ShowDescription(DisplayName="ID",Display=true,Editable=false)]
        public  long Id { get; set; }
        
         
     
    }
}
