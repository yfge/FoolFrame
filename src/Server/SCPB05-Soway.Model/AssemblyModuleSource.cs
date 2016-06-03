using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public class AssemblyModuleSource : IModuleSource
    {


        private Soway.Data.Graphic.Graphic<Model> graphic = new Data.Graphic.Graphic<Model>();


        private List<Module> thisModules = new List<Module>();
        public List<Module> GetModules() 
        {

            if (this.thisModules.Count == 0)
            {
                var list = fac.GetModules();

            


                Soway.Data.Graphic.Graphic<Module> moduleRelation = new Data.Graphic.Graphic<Module>();
                foreach (var item in list)
                {
                     
                    foreach (var property in item.Depdency)
                        moduleRelation.AddEdge(item, property);
                }


              
                while (moduleRelation.GetTopNode() != null)
                {
                    /////// System.Diagnostics.Trace.WriteLine(moduleRelation.GetTopNode().Name);
                    thisModules.Add(moduleRelation.GetTopNode());
                    moduleRelation.Remove(moduleRelation.GetTopNode());
                }
          
            }
            //foreach (var i in this.thisModules)
                /////// System.Diagnostics.Trace.WriteLine(i.Name);
            return this.thisModules;
            
        }

        

        public List<Model> GetModels(Module module)
        {

            var list = fac.GetModels(module);

            Soway.Data.Graphic.Graphic<Model> moduleRelation = new Data.Graphic.Graphic<Model>();
            foreach(var item in list){


                moduleRelation.Add(item);
                if (item.BaseModel != null)
                    moduleRelation.AddEdge(item.BaseModel, item);
                foreach (var property in item.Properties.Where(p => p.Model != null && p.IsArray ==true))
                {
                 
                    moduleRelation.AddEdge(property.Model, item);
                }
            }

            var result = new List<Model>();


            while (moduleRelation.Nodes.Count >0)
            {
                /////// System.Diagnostics.Trace.WriteLine("Model:" + moduleRelation.GetTopNode().Name);
                var ob = moduleRelation.GetTopNode();
                if (ob == null)
                {
                    ob = moduleRelation.Nodes[0].Data;
                }
                result.Add(ob);
                moduleRelation.Remove(ob);
            }



             
            
            return result;
             
        }

   

        private AssemblyModelFactory fac;

        public AssemblyModuleSource(String FileName)
        {
               
            fac = new AssemblyModelFactory(FileName);
        }
      
        public  AssemblyModuleSource(AssemblyModelFactory fac)
        {
            this.fac = fac;
        }

        private bool IsLoadMode = false;
        List<Model> result = new List<Model>();
        public List<Model> GetModels()
        {

            if (IsLoadMode == false)
            {
                foreach (var module in GetModules())
                {
                    result.AddRange(GetModels(module).ToArray());

                }
                IsLoadMode = true;
            }
            return result;
        }


        public List<Relation> GetRelations()
        {

            List<Relation> result = new List<Relation>();
            var models = this.GetModels();
            foreach (var mode in models)
            {
                result.AddRange(mode.Relations.ToArray());
            }
            return result;
        }
    }
}
