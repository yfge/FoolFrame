using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.DB.Manage
{
    public abstract class BOFac
    {
        protected WorkingDataBase db;
        public BOFac(String code)
        {
            this.db =new WorkDataBaseFactory().ALLList().FirstOrDefault(p => p.Code == code);
        }
    }
}
