using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Context
{
    public class InputQueryResult
    {   /// <summary>
        /// 提示的文字
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        /// 选择项的ID
        /// </summary>
        public object id { get; internal set; }


        public override string ToString()
        {


            return this.Text;
        }
    }
}
