using System;
using System.Collections.Generic;
using System.Text;

namespace Soway.Data.Discription.Display
{

    /// <summary>
    /// 显示一个属性是否需要展示
    /// </summary>
    public class ShowDescriptionAttribute : Attribute 
    {
        /// <summary>
        /// 显示的名称
        /// </summary>
        public String DisplayName { get; set; }

        /// <summary>
        /// 是否显示 
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 显示的顺序
        /// </summary>
        public int  DisplayIndex { get; set; }


        /// <summary>
        /// 是否可以编辑
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// 是否生成下拉列表 目前未用
        /// </summary>
        public bool GernerationDropdownlist { get; set; }

        /// <summary>
        /// 是否以列表显示
        /// 目前未使用
        /// </summary>
        public bool ShowInList { get; set; }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ShowDescriptionAttribute() { this.ShowInList = true; }
    }
}
