using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public interface IModuleSource
    {
        /// <summary>
        /// 得到模块
        /// </summary>
        /// <returns></returns>
        List<Module> GetModules();


        /// <summary>
        /// 得到模型
        /// </summary>
        /// <param name="Modules"></param>
        /// <returns></returns>
        List<Model> GetModels(Module Module);


        List<Model> GetModels();



        List<Relation> GetRelations();
    }
}
