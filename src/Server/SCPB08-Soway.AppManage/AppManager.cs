using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;
using Soway.DB.Manage;

namespace Soway.AppManage
{
    public class AppManager
    {

        private Soway.Model.SqlCon Con { get; set; }
        public ICurrentContextFactory ConFac { get; private set; }

        public AppManager (Soway.Model.SqlCon con,Soway.Model.Context.ICurrentContextFactory conFac)
        {
            this.Con = con;
            this.ConFac = conFac;
        }

        public Soway.Model.App.Application CreatApp(Soway.Model.App.Application App)
        {
            var dbContext = new Soway.Model.SqlServer.ObjectContext<Soway.Model.App.Application>
                (this.Con,this.ConFac);
            //1 创建应用
            var newApp = new Soway.Model.ModelHelper(this.ConFac).GetFromProxy(dbContext.Create(App)) as Soway.Model.App.Application;

            //2 创建应用系统库

            new Soway.Model.Manage.SqlServerModuleInstaller(this.ConFac).InstallModules
                (new Soway.Model.AssemblyModuleSource(
                    new Model.AssemblyModelFactory(typeof(Soway.Model.App
                    .Application))),App.SysCon,App.SysCon);


            new Soway.Model.Manage.SqlServerModuleInstaller(this.ConFac).InstallModules
      (new Soway.Model.AssemblyModuleSource(
          new Model.AssemblyModelFactory(typeof(SOWAY.ORM.AUTH.AuthorizedUser))), App.SysCon, App.SysCon);
            //3 连接应用用户

            SOWAY.ORM.AUTH.AuthorizedUser user = new SOWAY.ORM.AUTH.AuthorizedUser();
            user.User = App.Creator;
            new Soway.Model.SqlServer.ObjectContext<SOWAY.ORM.AUTH.AuthorizedUser>(App.SysCon,this.ConFac).Create(user);
            var itemsStrs = new String[]
            {
                "Model列表","Module列表","SqlCon列表","View列表","MenuItem列表"
            };


            Dictionary<String, String> subItem = new Dictionary<string, string>();
            subItem.Add("Module列表", "业务包管理");
            subItem.Add("Model列表", "模型管理");
            subItem.Add("SqlCon列表", "连接管理");
            subItem.Add("View列表", "界面管理");
            subItem.Add("MenuItem列表", "菜单项管理");

            Dictionary<String, String> AuthItem = new Dictionary<string, string>();
            AuthItem.Add("AuthorizedUser列表", "授权用户管理");
            AuthItem.Add("Department列表", "部门管理");
            AuthItem.Add("Role列表", "角色管理");
         
            foreach (var db in App.DataBase)
            {   //4 创建应用数据库
                new Soway.Model.Manage.SqlServerModuleInstaller(this.ConFac).InstallModules
             (new Soway.Model.AssemblyModuleSource(
                 new Model.AssemblyModelFactory(typeof(SOWAY.ORM.AUTH.User
                 ))), App.SysCon,db.Conection);
                //5 创建菜单
                SOWAY.ORM.AUTH.MenuItem item = new SOWAY.ORM.AUTH.MenuItem()
                {
                    Text = "系统管理", SubItems = new List<SOWAY.ORM.AUTH.MenuItem>()
                };
                foreach(var str in subItem.Keys)
                {
                    var view = new Soway.Model.View.AutoViewFactory(App.SysCon,this.ConFac).GetView(str);
                    view.ConnectionType = Model.ConnectionType.AppSys;

                    item.SubItems.Add(new SOWAY.ORM.AUTH.MenuItem()
                    {
                        Text = subItem[str],
                        ViewID = (int)view.ID
                    });

                    new Soway.Model.View.AutoViewFactory(App.SysCon,this.ConFac).SaveView(view);

                };
                SOWAY.ORM.AUTH.MenuItem AuthMenu = new SOWAY.ORM.AUTH.MenuItem()
                {
                    Text = "人员及权限",
                    SubItems = new List<SOWAY.ORM.AUTH.MenuItem>()
                };
                foreach (var str in AuthItem.Keys)
                {
                    var view = new Soway.Model.View.AutoViewFactory(App.SysCon,this.ConFac).GetView(str);
                    view.ConnectionType = Model.ConnectionType.AppSys;

                    AuthMenu.SubItems.Add(new SOWAY.ORM.AUTH.MenuItem()
                    {
                        Text = AuthItem[str],
                        ViewID = (int)view.ID
                    });

                    new Soway.Model.View.AutoViewFactory(App.SysCon,this.ConFac).SaveView(view);

                };
                new
          Soway.Model.SqlServer.ObjectContext<SOWAY.ORM.AUTH.MenuItem>(App.SysCon,this.ConFac).Create(item);
                new
Soway.Model.SqlServer.ObjectContext<SOWAY.ORM.AUTH.MenuItem>(App.SysCon,this.ConFac).Create(AuthMenu);
                //6 创建角色
                SOWAY.ORM.AUTH.Role role = new SOWAY.ORM.AUTH.Role()
                {
                    RoleName = "应用管理员"
                };
                role.Items = new List<SOWAY.ORM.AUTH.MenuItem>();
                role.Items.Add(item);
                foreach(var sub in item.SubItems)
                {
                    role.Items.Add(sub);
                }
                role.Items.Add(AuthMenu);
                role.Items.AddRange(AuthMenu.SubItems.ToArray());
                role.AuthUsers = new List<SOWAY.ORM.AUTH.AuthorizedUser>();
                role.AuthUsers.Add(user);

                new
                    Soway.Model.SqlServer.ObjectContext<SOWAY.ORM.AUTH.Role>(App.SysCon,this.ConFac).Create(role);
            
                //7 给用户加入角色
            }
            return newApp;
        }


      
    }
}
