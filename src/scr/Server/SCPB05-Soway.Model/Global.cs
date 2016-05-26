using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public   class   Global
    {

        internal static Soway.Model.AssemblyModelFactory fac;
        private static void InistStaticModels()
        {
            fac = new Soway.Model.AssemblyModelFactory(typeof(Model).Assembly);
            ModeMode = fac.GetModel(typeof(Model));
            ViewMode = fac.GetModel(typeof(Soway.Model.View.View));
            ModuleMode = fac.GetModel(typeof(Module));
            RelationModel = fac.GetModel(typeof(Relation));
        }

     
        public static Model ModeMode {
            get;
            set; }

        public static Model RelationModel { get; set; }
        public static Model ViewMode { get; set; }

        public static Model ModuleMode { get; set; }

       static  Global()
        {
            InistStaticModels();

        }

      // public static  Context.ICurrentContextFactory CurrentContextFactory { get; set; }
   　
    }
}
