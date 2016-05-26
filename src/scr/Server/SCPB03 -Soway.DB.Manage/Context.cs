namespace Soway.DB.Manage
{
    public partial class DBDataDataContext
    {


        public static DBDataDataContext Instance
        {
            get
            {
                return null;
                //var r = new DBDataDataContext(SFTech.Forms.App.ConnectionString);
                //r.CommandTimeout = 300; 
                //return r;
            }
        }

    }

}



