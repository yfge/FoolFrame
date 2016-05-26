using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report.Views
{
    public class CellFactory
    {
        public void OrdCells(List<Cell> items){

            items.Sort((ob1,ob2)=>{
                if (ob1.Column == ob2.Column)
                    return ob1.Row.CompareTo(ob2.Row);
                else
                    return ob1.Column.CompareTo(ob2.Column);

            });
        }
    }
}
