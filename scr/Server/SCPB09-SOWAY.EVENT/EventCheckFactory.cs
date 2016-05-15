using System;
using Soway.Model.App;
using System.Collections.Generic;

namespace Soway.Event
{
    internal class EventCheckFactory
    {
        private Soway.Model.SqlCon db;

        public EventCheckFactory(Soway.Model.SqlCon db)
        {
            this.db = db;
        }

        internal List<dynamic> GetObjs(dynamic def)
        {
            if (def.DefModel != null)
            {
                var model = new Soway.Model.ModelHelper(null).GetFromProxy(def.DefModel) as Model.Model;
                var command = SqlHelper.GetQueryCommand(model, def);
                return new Soway.Model.SqlServer.dbContext(this.db, null).GetBySqlCommand(model, command);
            }else
            {
                return new List<dynamic>();
            }
        }
    }
}