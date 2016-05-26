using System;
using System.Collections.Generic;
using Soway.Model;
using Soway.Model.Context;
using System.Linq;

namespace Soway.Event
{
    internal class EventDefFactory
    {
        private Model.Model model;
        private Model.SqlCon SqlCon;

        

        public EventDefFactory(Model.SqlCon sqlCon)
        {
            this.SqlCon = sqlCon;
            this.model =ModelFac.Models.First(p=>p.ClassName=="Soway.Event.EventDefination");
        }

        internal IEnumerable<dynamic> GetDefines()
        {
            return new Soway.Model.SqlServer.dbContext(this.SqlCon, null).GetBySqlCommand(
                this.model, "SELECT * FROM SW_EVT_DEF WHERE EVTDEF_STATE ='0'");
        }
    }
}