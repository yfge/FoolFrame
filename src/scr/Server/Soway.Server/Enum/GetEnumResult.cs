using System.Collections.Generic;

namespace Soway.Service.Enum
{
    public class GetEnumResult:TokenResult
    {
        public List<EnumValues> EnumValues { get; set; }
    }
}