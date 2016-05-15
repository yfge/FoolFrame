using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Manage
{
    public interface IModuleInstaller
    {

        void CreateSysDataBase(SqlCon sysCon);
        void InstallModules(IModuleSource Source,SqlCon modelSqlCon,SqlCon dataSqlCon );


        bool CheckModelIsInstalled(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon);


        /// <summary>
        /// 安装一个模块
        /// </summary>
        /// <param name="model"></param>
        void InstallModel(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon);


        /// <summary>
        /// 更新一个模块 
        /// </summary>
        /// <param name="model"></param>
        void UpdateModel(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon);


        /// <summary>
        /// 删除一个模块
        /// </summary>
        /// <param name="model"></param>
        void DeleteMode(Model model, SqlCon modelSqlCon, SqlCon dataSqlCon);
    }
}
