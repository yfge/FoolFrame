using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;
namespace Soway.Model.App
{


    [Table(Name = "SW_STOREDB",ColPreStr = "SW_STORE_")]
   [Serializable]
    public class StoreDataBase
    {
        [Column(ColumnName = "STOREID",IsKey=true)]
        public Guid StoreBaseId { get; set; }

        [Column(ColumnName="NAME")]

        public string Name { get; set; }

        [Column(ColumnName="CON")]

        public SqlCon Conection { get; set; }
        [Column(ColumnName="Note")]
        public string Note { get; set; }

        public List<Application> Apps { get; set; }
    }
}
