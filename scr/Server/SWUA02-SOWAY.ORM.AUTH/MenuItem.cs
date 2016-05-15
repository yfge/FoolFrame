using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;

namespace SOWAY.ORM.AUTH
{
    [Table(Name = "SW_APP_AUTH_MENU", ColPreStr = "AUTH_MENU_")]
    public class MenuItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        /// 
        [Column(ColumnName ="ID",IsKey =true,IsAutoGenerate = GenerationType.OnInSert,IsIdentify =true)]
        public long ID
        {
            get; set;
        }

        [Column(ColumnName ="TEXT")]

        /// <summary>
        /// 文字
        /// </summary>
        public string Text
        {
            get; set;
        }
        /// <summary>
        /// 快捷键
        /// </summary>
        /// 
        [Column(ColumnName ="SHORTCUTKEY")]
        public string ShortcutKey { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        /// 
        [Column(ColumnName ="IMAGE")]
        public string Image
        {
            get; set;
        }

        /// <summary>
        /// 默认可见
        /// </summary>
        /// 
        [Column(ColumnName = "VISIABLE")]
        public bool DefaultVisiable
        {
            get; set;
        }

        /// <summary>
        /// 默认可用
        /// </summary>
        /// 
        [Column(ColumnName ="ENABLE")]
        public bool DefautEnable
        {
            get; set;
        }
        [Column(ColumnName ="VIEWID")]
        public int ViewID
        {
            get;set;
        }
        [Column(ColumnName ="TEMPLATEFILE")]
        public string TemplateFile
        {
            get; set;
        }

        [Column(ColumnName = "INDEX")]
        public int Index
        {
            get; set;
        }


        public List<MenuItem> SubItems { get; set; }


        public List<Role> Roles { get; set; }
    }
}