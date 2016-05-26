using Soway.Data;
using Soway.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Soway.Query.BoolExp
{
   
    public class CompareOpFac
    {
        private  string ConStr { get;     set; }

        public List<CompareOp> GetSelectedType( PropertyType properyType)
        {
            string SQL =string.Format(@"SELECT  [SE_COMPARESHOW]
      ,[SE_COMPAREEXP]
     ,SE_COMPARETYPE.[SysID] as SysID
  FROM  [SE_COMPARETYPE]
  JOIN [SE_COMPARETYPE_PROPERTYINDEX] ON SE_COMPARETYPE.SysID= [COMPARETYPE_ID]
  WHERE [PROPERTYTYPE_VALUE]='{0}'", (int) (properyType));
            return new DBContext(ConStr).Get<CompareOp>(SQL);
        }
        public List<CompareOp> GetSelectedType(PropertyType properyType,long prpId)
        {
            string SQL = string.Format(@"SELECT  [SE_COMPARESHOW]
      ,[SE_COMPAREEXP]
     ,[SE_COMPARETYPE].[SysID]  as SysID
  FROM  [SE_COMPARETYPE]
  JOIN [SE_COMPARETYPE_PROPERTYINDEX] ON SE_COMPARETYPE.SysID= [COMPARETYPE_ID]
  WHERE [PROPERTYTYPE_VALUE]='{0}'", (int)(properyType));
            return new DBContext(ConStr).Get<CompareOp>(SQL);
        }
        public CompareOpFac(String conStr)
        {
            this.ConStr = conStr;
        }
    }
    
}
