using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
   
    public class 
        ObjectValueChangedEventArgs :System.EventArgs
    {
        public Property ChangedProperty { get; internal set; }
    }
}
