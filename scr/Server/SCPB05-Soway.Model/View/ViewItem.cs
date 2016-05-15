using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
using System.Runtime.Serialization;
using Soway.Model.View;

namespace Soway.Model
{
   
    [DataContract]
    [Table(Name = "SW_SYS_VIEW_ITEM")]
    public class ViewItem
    {

        /// <summary>
        /// 列名称(Label)
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_NAME")]
        public string Name
        {
            get;
            set;
        }


        /// <summary>
        /// 列名称(Label)
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_NOTE")]
        public string Note
        {
            get;
            set;
        }


        /// <summary>
        /// 格式
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_FORMAT")]
        public string Format
        {
            get;
            set;
        }
        /// <summary>
        /// 属性
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_PROPERTY")]
        public Property Property
        {
            get;
            set;
        }

        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_PROPERTY_SHOW")]
        public Property ShowProperty
        {
            get;
            set;
        }

        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_PROPERTY_VALUE")]
        public Property ValueProperty
        {
            get;
            set;
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_READONLY")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// 显示序列
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_INDEX")]
        public int ShowIndex { get; set; }


        /// <summary>
        /// 列表视图
        /// 当属性为集合时,显示集合用的视图
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_SUBVIEW")]
        public View.View ListView
        {
            get;
            set;
        }


        /// <summary>
        /// 如果属性为集合类型，那么这个表示属性在增加，修改时弹出的详细的窗体的View
        /// 如果属性为单一的选择类型，那么这表示属性在选择时用的
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_EDITVIEW")]
        public View.View EditView 
        {
            get;
            set;
        }

        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_SELECTVIEW")]
        public View.View SelectedView
        {
            get;
            set;
        }
        /// <summary>
        /// 宽度
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_WIDTH")]
        public int Width
        {
            get;
            set;

        }
        /// <summary>
        /// 是否显示
        /// </summary>
        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_ISSHOW")]
        public bool IsShow
        {
            get;
            set;
        }
        [DataMember]
        [Column(ColumnName ="VIEW_ITEM_FILE")]
        public ViewTemplateFile ItemFile { get; set; }

        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_EDITTYPE")]
        public ItemEditType EditType
        {
            get;
            set;

        }

        [DataMember]
        [Column(ColumnName = "VIEW_ITEM_SOURCEEXP")]
        public string  ItemSourceExp
        {
            get;
            set;

        }

    }
}
