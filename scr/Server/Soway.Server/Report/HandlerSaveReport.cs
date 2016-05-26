using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    class HandlerSaveReport : Handler
    {
        public HandlerSaveReport(SavedReportOption opt)
        {
            this.Opt = opt;
            IsNeedAuthenticate = true;
            this.PostData = opt;
            this.Result = new Result();

        }

        public SavedReportOption Opt { get; private set; }

        protected override void ImplementBusinessLogic()
        {
            
        }
    }
}
