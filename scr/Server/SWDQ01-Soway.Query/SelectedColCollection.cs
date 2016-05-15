using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Query
{
    /// <summary>
    /// 选择输出列的集合
    /// </summary>
    public class SelectedColCollection : IList<SelectedCol >,ICollection<SelectedCol>
    {

     
        private List<SelectedCol> _items;
 
        internal SelectedColCollection( )
        {
            _items = new List<SelectedCol>();
     
        }
        #region IList<SelectedCol> 成员

        public int IndexOf(SelectedCol item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, SelectedCol item)
        {
            _items.Insert (index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public SelectedCol this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<SelectedCol> 成员
        /// <summary>
        /// 增加一个输出列
        /// </summary>
        /// <param name="item">要增加的列</param>
    
        public void Add(SelectedCol item)
        {

            if (this.Count(p => p.SelectedName == item.SelectedName) > 0)
                throw new Exception("已经有相同的列名称存在");
            _items.Add(item);
            for (int i = 0; i < _items.Count; i++)
                _items[i].SelectedIndex = i;

        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(SelectedCol item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(SelectedCol[] array, int arrayIndex)
        {
            _items.CopyTo (array, arrayIndex);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(SelectedCol item)
        {
            return _items.Remove(item);
        }

        #endregion

        #region IEnumerable<SelectedCol> 成员

        public IEnumerator<SelectedCol> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

       
    }
}
