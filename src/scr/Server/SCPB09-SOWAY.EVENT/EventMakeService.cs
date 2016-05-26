using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Soway.Model.Context;

namespace Soway.Event
{
    public class EventMakeService
    {
        private bool isRun;
        private Thread thread;
        private ICurrentContextFactory conFac;
        private int sleepMiliseconds;

        public  void Work()
        {
            //1 得到所有事件的定义


            var appModel = ModelFac.Models.First(p => p.ClassName == "Soway.Model.App.Application"); ;
            isRun = true;
            while (isRun)
            {
                try
                {
                    var apps = new Soway.Model.SqlServer.dbContext(this.conFac.GetCurrentContext().SysCon, this.conFac).GetBySqlCommand(appModel, "Select * from SW_APPLICATION");
                    var appfac = new Soway.Model.App.AppFac(this.conFac.GetCurrentContext().SysCon, this.conFac);
                    foreach (dynamic app in apps)
                    {


                        // passed 
                        Soway.Model.SqlCon con = new Soway.Model.ModelHelper(this.conFac).GetFromProxy(app.SysCon) as Model.SqlCon;
                        var fac = new EventDefFactory(con);
                        List<Model.SqlCon> dbs = new List<Model.SqlCon>();
                        var evtFac = new EventFactory(conFac, con);
                        foreach (dynamic db in app.DataBase)
                        {
                            Soway.Model.SqlCon dbSqlCon = new Soway.Model.ModelHelper(this.conFac).GetFromProxy(db.Conection) as Soway.Model.SqlCon;
                            dbs.Add(dbSqlCon);
                        }
                        // to test
                        //2 根据事件去检查
                        foreach (dynamic defs in fac.GetDefines())
                        {
                            foreach (var db in dbs)
                                evtFac.CheckAndGenerate(defs, db, con);
                        }

                    }
            }catch (Exception ex)
            {

            }

            System.Threading.Thread.Sleep(this.sleepMiliseconds);

            }
          
            //3生成时间 
        }
        public void Start()
        {
            this.thread =
            new System.Threading.Thread(
                new System.Threading.ThreadStart(Work));
            this.thread.Start();
        }
        public void Stop()
        {
            isRun = false;
            this.thread.Join();

        }

       
        public  EventMakeService(Soway.Model.Context.ICurrentContextFactory fac)
        {
            this.sleepMiliseconds = 60*1000;
            this.conFac = fac;

        }

    }
}
