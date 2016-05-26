using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Soway.Model;

namespace Soway.Service
{
    //class DataBase 
    //{

    //    private static DataBase instance;

    //    private object obj = new object(
    //        );
    //    private DataBase()
    //    {

    //    }

    //    public static DataBase GetInstance()
    //    {   
    //        if (instance == null)
    //        {
    //            instance = new DataBase();
    //        }
    //        return instance;
    //    }
    //    public void InitDataBaseInfo(
    //        )
    //    {
 
    //        //Soway.Model.Global.SqlCon = new Soway.Model.SqlCon()
    //        //{
    //        //    InitialCatalog = "ISYS_APP",
    //        //    DataSource="demo.soway.co",
    //        //    UserID="rfid",
    //        //    IntegratedSecurity=false,
    //        //    Password="xwlyrfid",
    //        //    IsLocal = true
    //        //};

    //        System.Data.SqlClient.SqlConnectionStringBuilder builder =
    //    new System.Data.SqlClient.SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Global"].ConnectionString);


    //        //  System.Data.SqlClient.SqlConnectionStringBuilder builder =
    //        //new System.Data.SqlClient.SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Global"].ConnectionString);



    //        global::Soway.Model.Global.SqlCon = new global::Soway.Model.SqlCon()
    //        {
    //            DataSource = builder.DataSource,
    //            InitialCatalog = builder.InitialCatalog,
    //            UserID = builder.UserID,
    //            Password = builder.Password,
    //            IsLocal = false,
    //            IntegratedSecurity = false
    //        };
    //    }
    //}

}