using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data;
using Soway.Model.View;

namespace Soway.Service
{
   public class ViewItem
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public bool IsReadOnly { get; set; }
        public int ShowIndex { get; set; }
        public int Width { get; set; }
        public string PropertyName { get; set; }
        public long PropertyId { get; set; }

        public int  ListViewType { get;  set; }
        public long ListViewId { get;  set; }

        public long EditViewId { get; set; }
        public long EditExp { get; set; }
        public string ViewFile { get;  set; }
        public PropertyType PropertyType { get;   set; }
        public long PropertyModel { get;   set; }
        public ItemEditType EditType { get;   set; }
    }
}
