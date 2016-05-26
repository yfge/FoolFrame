using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public interface IModelFactory
    {


        /// <summary>
        /// 得到一个模块里的所有模型定义
        /// </summary>
        /// <param name="Modules"></param>
        /// <returns></returns>
        List<Model> GetModels(Module Module);

       // List<Property> GetModelProperties(ArgModel model);
        /// <summary>
        /// 得到所有模块
        /// </summary>
        /// <returns></returns>
        List<Module> GetModules();

        /// <summary>
        /// 得到一个模块的依赖项
        /// </summary>
        /// <param name="Modules"></param>
        /// <returns></returns>
        List<Module> GetRefrenceModules(Module Module);


        /// <summary>
        /// 安装一个模块
        /// </summary>
        /// <param name="Modules"></param>
        void InstallModel(Module Module);


        Model GetModel(Type type);

         
         
 
    }
}
