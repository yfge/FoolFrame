using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Enum
{
    class HandlerGetEnum : Handler
    {
        private GetEnumOption Option { get; set; }
        private GetEnumResult Data { get; set;}
        public HandlerGetEnum(GetEnumOption option)
        {
            this.Option = option;
            this.Data = new GetEnumResult();
            this.PostData = this.Option;
            this.Result = this.Data;
            IsNeedAuthenticate = true;
        }
        protected override void ImplementBusinessLogic()
        {
            this.Data.EnumValues = new List<EnumValues>();
            var cxt = new Soway.Model.SqlServer.DynamicContext(this.Info.AppSqlCon.ToString(), this);
            dynamic model = cxt.GetById(typeof(Model.Model), this.Option.ModelId);
            foreach (dynamic item in model.EnumValues)
            {
                this.Data.EnumValues.Add(new EnumValues()
                {
                    Name = item.String,
                    Value = item.Value
                });

            }
            
        }
    }
}
