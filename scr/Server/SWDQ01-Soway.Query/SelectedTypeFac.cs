using Soway.Data;
using Soway.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Query
{
    public class SelectedTypeFac
    {
        public string ConStr { get; private set; }

        public List<SelectType> GetSelectedType(PropertyType properyType)
        {

            string SQL = string.Format(@"SELECT  [SE_SELECTEDSHOW]
      ,[SE_SELECTEDEXP]
      ,[SE_REQUIREGROUP]
      ,SE_SELECTEDTYPE.[SysID]  as SysID
  FROM  [SE_SELECTEDTYPE]
  JOIN [SE_SELECTEDTYPE_PROPERTYINDEX] ON SE_SELECTEDTYPE.SysID= [SELECTEDTYPE_ID]
  WHERE [PROPERTYTYPE_VALUE]='{0}'", (int)(properyType));
            return new DBContext(this.ConStr).Get<SelectType>(SQL);
        }

        public List<SelectType> GetSelectedType(PropertyType properyType,long id)
        {

            string SQL = string.Format(@"SELECT  [SE_SELECTEDSHOW]
      ,[SE_SELECTEDEXP]
      ,[SE_REQUIREGROUP]
      ,SE_SELECTEDTYPE.[SysID] as SysID
  FROM  [SE_SELECTEDTYPE]
  JOIN [SE_SELECTEDTYPE_PROPERTYINDEX] ON SE_SELECTEDTYPE.SysID= [SELECTEDTYPE_ID]
  WHERE [PROPERTYTYPE_VALUE]='{0}'", (int)(properyType));
            return new DBContext(this.ConStr).Get<SelectType>(SQL);
        }

        public List<SelectType> GetAllSelectedType()
        {
            return new DBContext(this.ConStr).Get<SelectType>();
        }

        public SelectedTypeFac(String conStr)
        {
            this.ConStr = conStr;
        }
    }
}
