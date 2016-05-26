using Soway.Query.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Query
{
    public class ModelQueryCol : IQueryColumn
    {
        public string PropertyName { get; set; }
       
        public string ModelId { get; set; }


        public List<Soway.Model.EnumValues> States { get; set; }
    }
}
