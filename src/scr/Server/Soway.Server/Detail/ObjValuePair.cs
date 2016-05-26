using Soway.Data;
using Soway.Model.View;

namespace Soway.Service.Detail
{
    public class ObjValuePair
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ObjId { get; set; }
        /// <summary>
        /// 属性ID
        /// </summary>
        public string PrpId { get; set; }

        /// <summary>
        /// 显示的格式值
        /// </summary>
        public string FmtValue { get; set; }
        public string PrpShowName { get; set; }
        public PropertyType PrpType { get; set; }
        public long PrpModelId { get; set; }
        public bool ReadOnly { get; set; }
        public ItemEditType EditType { get; set; }

    }
}