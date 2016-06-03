using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Report
{
    public class Report
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 参数
        /// </summary>
        public List<Param> Params
        {
            get;
            set;
        }

        /// <summary>
        /// 维一标识
        /// </summary>
        public Guid ID
        {
            get;
            set;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string No
        {
            get;
            set;
        }

        /// <summary>
        /// 结果
        /// </summary>
        public List<ReportResultTable> Result
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 报表源
        /// </summary>
        public IReportSource Source
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public String CreatePerson
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 更改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// 更改人
        /// </summary>
        public string MoidiyPerson
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
