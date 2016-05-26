using Soway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Event
{
    static class ModelFac
    {
        private static List<Model.Model> models;

        static ModelFac()
        {
            initModels();
        }

        private static void initModels()
        {
            models = new AssemblyModuleSource(new AssemblyModelFactory(typeof(EventDefination))).GetModels();
        }

        public static List<Model.Model> Models
        {
            get
            {
                if (models == null || models.Count == 0)
                    initModels();
                return models;
            }
        }
    }
}
