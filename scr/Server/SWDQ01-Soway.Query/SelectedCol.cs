using System;
using System.Collections.Generic;
 
using System.Text;
using Soway.Query.Entity;

namespace Soway.Query
{
    /// <summary>
    /// 表示选择的要输出的或分组的列
    /// </summary>
    public class SelectedCol :System.ComponentModel.INotifyPropertyChanged
    {

  

      
        private int _SelectedIndex;
        /// <summary>
        /// 选择/分组顺序
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                var old = _SelectedIndex;
                _SelectedIndex = value;
                if (old != _SelectedIndex)
                {
                    Notify("SelectedIndex");
                }
            }
        }


 
        /// <summary>
        /// 选择的列
        /// </summary>
        public IQueryColumn DataCol
        {
            get;
            set;
        }


        private SelectType selectType;
        /// <summary>
        /// 选择的类型
        /// </summary>
        public SelectType SelectType
        {
            get
            {
                return selectType;
            }
            set
            {

                var old = selectType;
                selectType = value;
                if (selectType != old)
                {
                    Notify("SelectType");
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public OrderType OrderType { get; set; }


        //public SelectedCol()
        //}



        private string selectedName;
        /// <summary>
        /// 输出的名称
        /// </summary>
        /// <remarks>用于构造于sql中 as后的列名</remarks>
        public String SelectedName { get {
            return selectedName;

        }
            set
            {
                var old = selectedName;
                selectedName = value;
                if (selectedName != old)
                    Notify("SelectedName");

            }
        }


        /// <summary>
        /// 选择的表
        /// </summary>
        public SelectedTable SelectedTable { get; set; }


        public List<ColStateValue> Values { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;


        private void Notify(string Name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(Name));
            }
        }
    }
}
