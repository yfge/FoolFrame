using Soway.Data.Discription.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    [Table(Name = "SW_SYS_OPERATION_PARAM")]
    public class OperationParam
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        /// 

            [Column(ColumnName = "SW_SYS_OPERATION_PARAM_NAME")]
        public String ParamName { get; set; }

        /// <summary>
        /// 选择的参数的视图
        /// </summary>
        /// 
        [Column(ColumnName = "SW_SYS_OPERATION_PARAM_VIEW")]
        public View.View ParamSelectView { get; set; }


        /// <summary>
        /// 选择的筛选条件
        /// </summary>
        /// 
        [Column(ColumnName = "SW_SYS_OPERATION_PARAM_FILTER")]
        public string ParamFilter { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        /// 
        [Column(ColumnName = "SW_SYS_OPERATION_PARAM_VALUE")]
        public string ParamValue { get; set; }
    }
}
