using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public interface IModelSqlFactory
    {
        IModelFactory ModelFactory { get; }
        System.Data.Common.DbCommand GerateCreateSql(Model Model);
  //      System.Data.Common.DbCommand GerateNewSql(ObjectProxy proxy);

         
    }
}
