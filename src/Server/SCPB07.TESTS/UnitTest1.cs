using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SCPB07.TESTS
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetGetApp()
        {
            global::Soway.Model.App.AppFac fac = new global::Soway.Model.App.AppFac(
                new Soway.Model.SqlCon()
                {
                    DataSource = "localhost",
                    InitialCatalog = "NY_2016_SW_NEW_SYS",
                    Password = "xwlyrfid",
                    UserID = "rfid"
                },null);
            var app = fac.GetApp("E4BEE30B-F38B-41D3-8B83-4C08E5E25FDE", "159753");
        }
    }
}
