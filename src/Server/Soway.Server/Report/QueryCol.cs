using System.Collections.Generic;
using Soway.Data;
using Soway.Model;

namespace Soway.Service.Report
{
    public class QueryCol
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public List<QueryType> QueryTypes { get; set; }
        public List<CompareOpItem> CompareTypes { get; set; }
        public PropertyType PrpType { get;  set; }
        public string ModelId { get;  set; }
        public List<Model.EnumValues> States { get;  set; }
    }
}