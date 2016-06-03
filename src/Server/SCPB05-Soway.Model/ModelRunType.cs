using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public class ModelRunType
    {

        public PropertyOperation[] GetProperties()
        {
            return this.propertyOperations.ToArray();
        }

        public PropertyOperation GetProperty(String Name)
        {
            return this.propertyOperations.FirstOrDefault(p => p.Property.Name == Name);
        }

        public ModelRunType(Model model)
        {
            propertyOperations = new List<PropertyOperation>();
            this.Model = model;
            if (model.Module.GerationDLL)
            {
                foreach (var property in model.Properties)
                {
                    propertyOperations.Add(new ReflectionPropertyOperation(property,Model));
                }

            }
            else
            {
                foreach (var proerty in model.Properties)
                {
                    propertyOperations.Add(new DynamicPropertyOperation(proerty));
                }
            }
        }
        private List<PropertyOperation> propertyOperations = new List<PropertyOperation>();
        public Model Model { get; set; }

        public ModelRunType(object ob)
        {

            if (ob is ObjectProxy)
            {
            }
            else
            {
            }
        }
    }
}
