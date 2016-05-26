using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Discription.ORM
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumAttribute :Attribute
    {
        public String NoteStr { get; private  set; }

        public EnumAttribute(String note)
        {
            this.NoteStr = note;
        }
    }
}
