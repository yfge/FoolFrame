using Soway.Data;
using Soway.Data.Discription.ORM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Soway.Model
{
    public class AssemblyModelFactory :IModelFactory
    {
        private System.Reflection.Assembly Assembly { get; set; }
        public List<Model> GetModels(Module Module)
        {

            try
            {
                this.Assembly = System.Reflection.Assembly.LoadFile(Module.AssemblyFile);
            }catch{
                this.Assembly = System.Reflection.Assembly.LoadFile(new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + Module.Assembly + ".dll");
            }
            List<Model> result = new List<Model>();
            try
            {
                foreach (Type businuessType in this.Assembly.GetTypes())
                {
                    Model model = GetModel(businuessType);
                    if (model != null)
                        result.Add(model);

                }
                foreach(var model in result)
                {
                    LoadRelation(model);
                }
            }
            catch { }
            return result;
        }
        public void GetModelProperties(Model model)
        {



            if(model.Properties ==null)
            model.Properties = new List<Property>();
            Type type = System.Reflection.Assembly.LoadFile(model.Module.AssemblyFile).GetType(model.ClassName);
            Soway.Data.Discription.ORM.ORMHelper helper = new Data.Discription.ORM.ORMHelper();

           // List<Property> items = new List<Property>();
            foreach (System.Reflection.PropertyInfo propertyInfo in type.GetProperties())//.OrderBy(p=>(typeof(ICollection).IsAssignableFrom(p.PropertyType))))
            {
               
             
                Property property = new Property();
                property.PropertyName = propertyInfo.Name;
                if (model.Properties.Count(p => p.PropertyName == propertyInfo.Name) > 0)
                    property = model.Properties.First(p => p.Name == propertyInfo.Name);
                else
                    model.Properties.Add(property);
                     
                
                property.PropertyName = propertyInfo.Name;

                property.Name = propertyInfo.Name;
                property.CanGet = propertyInfo.CanRead;
                property.CanSet = propertyInfo.CanWrite;
                var dis = propertyInfo.GetCustomAttributes(typeof(Soway.Data.Discription.Display.ShowDescriptionAttribute), true);
                if(dis.Length >0)
                {
                    property.Name = (dis[0] as Soway.Data.Discription.Display.ShowDescriptionAttribute).DisplayName;
                }

                if (
            model.Properties.Count(p => p.Name == property.Name) > 0)
                {
                    property.Name = propertyInfo.Name;
                }
                
            
                property.PropertyType =Soway.Data.PropertyTypeAdaper. GetPropertyType(propertyInfo.PropertyType);
             
                property.IsArray = typeof(ICollection).IsAssignableFrom(propertyInfo.PropertyType);
 
            
                var cols = helper.GetColNameAttributes(propertyInfo);
                if (property.PropertyType == PropertyType.BusinessObject)
                {
                    property.Model = GetModel(propertyInfo.PropertyType);
                    if (cols.Count == 1)
                    {
                        property.DBName = helper.GetColName(type, propertyInfo.Name);
                        property.IsCheck = cols[0].IsKey;
                        property.IXGroup = cols[0].KeyGroupName;
                        property.AutoGenerationType = cols[0].IsAutoGenerate;
                        if (cols[0].EncrpytType == Data.Discription.ORM.EncryptType.RadomDECS)
                            property.PropertyType = PropertyType.RadomDECS;
                        else if (cols[0].EncrpytType == Data.Discription.ORM.EncryptType.MD5)
                            property.PropertyType = PropertyType.MD5;
                        if (cols[0].IsIdentify)
                        {
                            property.PropertyType = PropertyType.IdentifyId;
                            property.IsCheck = true;
                        }
                        property.IsMultiMap = false;
                        property.Format = cols[0].FormatStr;
                    }
                    else
                    {
                        property.Format = cols[0].FormatStr;
                        property.IsMultiMap = true;
                        foreach (var col in cols)
                            property.DBMaps.Add(new MultiDBMap()
                            {
                                PropertyName = col.PropertyName,
                                DBColName = helper.GetColName(col, propertyInfo)
                            });
                    }

                }
                else
                {
                    if(cols[0].NoMap ==false )
                        property.DBName = helper.GetColName(type,propertyInfo.Name);
                    property.IsCheck = cols[0].IsKey;
                    property.IXGroup = cols[0].KeyGroupName;
                    property.AutoGenerationType = cols[0].IsAutoGenerate;
                    if (cols[0].IsIdentify)
                    {
                        if( propertyInfo.PropertyType != typeof(Guid))
                        property.PropertyType = PropertyType.IdentifyId;
                        property.IsCheck = true;
                    } if (cols[0].EncrpytType == Data.Discription.ORM.EncryptType.RadomDECS)
                        property.PropertyType = PropertyType.RadomDECS;
                    else if (cols[0].EncrpytType == Data.Discription.ORM.EncryptType.MD5)
                        property.PropertyType = PropertyType.MD5;
                    property.Format = cols[0].FormatStr;
                    property.IsMultiMap = false;
                    if (property.PropertyType == PropertyType.Enum)
                    {
                        property.Model = GetModel(propertyInfo.PropertyType);
                    }else 
                        property.Model = null;
                }



            }
            //   model.Properties = items;

          

//      return items;

        }

        Dictionary<string, Model> models = new Dictionary<string, Model>();
        public   Model GetModel(Type businuessType)
        {
            //如果是集合,则返回集合的的项类型
            if (typeof(ICollection).IsAssignableFrom(businuessType))
                if (businuessType.GetElementType() == null)
                {
                    if (businuessType.GetGenericArguments().Count() >= 1)
                        businuessType = businuessType.GetGenericArguments()[0];
                    else
                        return null;
                }
                else
                    businuessType = businuessType.GetElementType();

            //可以存储的(1,为类,而且有table定义;2.为枚举)
            if (((businuessType.IsClass ) && 
                businuessType.GetCustomAttributes(typeof(Soway.Data.Discription.ORM.TableAttribute), true).Length > 0) || businuessType.IsEnum)
            {
                if (models.ContainsKey(businuessType.ToString()) == false)
                {
                    //System.Diagnostics.Trace.WriteLine(String.Format("Create Model of {0},Count:{1}",businuessType.Name,models.Keys.Count));

                    Model model = new Model();
                    models.Add(businuessType.ToString(), model);
                    Soway.Data.Discription.ORM.ORMHelper helper = new Data.Discription.ORM.ORMHelper();
                    model.Name = businuessType.Name;
                    model.ClassName = businuessType.FullName;

                    model.Module = GetModule(businuessType.Assembly);

                    if (businuessType.IsEnum)
                        model.ModelType = ModelType.Enum;
                    else if (businuessType.IsAbstract)// ||businuessType.IsInterface)
                        model.ModelType = ModelType.AbstractClass;
                    else
                    {
                        model.ModelType = ModelType.Class;
                        model.Operations = new List<Operation>();
                        model.Operations.Add(new Operation()
                        {
                            Name = "删除",
                            BaseOperationType = BaseOperationType.Delete,
                            ArgModel = model
                        });
                    }

                    if (model.ModelType != ModelType.Enum)
                    {
                        model.BaseModel = GetModel(businuessType.BaseType);
                        var table = helper.GetTableAttribute(businuessType);
                        model.DataTableName = helper.GetTable(businuessType).Replace("[", "").Replace("]", "");
                        model.IsView = table.IsView; 
                        LoadModel(model);
                     
                    }
                    else
                    { 
                        foreach (var i in Enum.GetValues(businuessType))
                        {
                          model.EnumValues.Add(new EnumValues() { String = i.ToString(), Value = (int)i });
                        }
                        model.ShowProperty = model.Properties.FirstOrDefault(p => p.PropertyName == "String");
                        
                    }


                    
                }
                return models[businuessType.ToString()];
            }
            else
                return null;
        }

        private void LoadModel( Model model )
        {

            var helper = new Data.Discription.ORM.ORMHelper();

            if (model.Properties != null && model.Properties.Count > 0)
                return;
            GetModelProperties(model);
            model.AutoSysId = !(model.Properties.Count(p => p.IsCheck
                && String.IsNullOrEmpty(p.IXGroup)) == 1);
            if (model.AutoSysId == false)
            {
                model.IdProperty = model.Properties.First(p => p.IsCheck && String.IsNullOrEmpty(p.IXGroup)); ;

                var Primary = model.IdProperty;

                if ((Primary.AutoGenerationType == Data.Discription.ORM.GenerationType.OnInSert
                    && (Primary.PropertyType == PropertyType.Int || Primary.PropertyType == PropertyType.Long)))
                {
                    Primary.PropertyType = PropertyType.IdentifyId;
                }else if (Primary.AutoGenerationType == GenerationType.OnInSert
                    &&String.IsNullOrEmpty(Primary.Format)==false
                    &&Primary.PropertyType == PropertyType.String)
                {
                    Primary.PropertyType = PropertyType.SerialNo;
                }

                Primary.AllowDBNull = false;
            }
            
            model.ShowProperty = model.Properties.FirstOrDefault(p => p.Name.ToLower().IndexOf("name") >= 0 || p.PropertyName.ToLower().IndexOf("name") >= 0);

            if (model.ShowProperty == null)
                model.ShowProperty = model.IdProperty;
            if (model.ShowProperty == null)
                model.ShowProperty = model.Properties.FirstOrDefault();
            LoadRelation(model);

        }

        private string GetRelationTable(Model parent, Model property)
        {
            List<string> result = new List<string>();
            result.Add(parent.DataTableName.Replace("[", "").Replace("]", "").Trim());
            result.Add(property.DataTableName.Replace("[", "").Replace("]", "").Trim());

            result.Sort();

            return result[0] + "_" + result[1];
        }
        private void LoadRelation(Model model)
        {
            Type type = System.Reflection.Assembly.Load(model.Module.Assembly).GetType(model.ClassName);
            foreach (var property in model.Properties.Where(p => p.IsArray)) {
                if (property.IsArray)
                {
                    try
                    {
                        Relation relation = new Relation();
                        relation.Property = property;
                        relation.RelationTable = "";
                        if (model.Relations.Count(p => p.Property == relation.Property) == 0)
                            model.Relations.Add(relation);
                        else
                            relation = model.Relations.First(p => p.Property == property);

                        if (property.Model != null 
                            && (property.Model.Properties == null || 
                                property.Model.Properties.Count == 0))
                            LoadModel(property.Model);
                        var propertyInfo = type.GetProperty(property.PropertyName);
                        var attri = propertyInfo.GetCustomAttributes(typeof(Soway.Data.Discription.ORM.MultiTypeAttribute), true);
                        var refAttr = propertyInfo.GetCustomAttributes(typeof(Soway.Data.Discription.ORM.ReferToProperyAttrbute), true);
                        if (attri.Length > 0)
                        {
                            //有定义
                            var multi = attri.First() as Soway.Data.Discription.ORM.MultiTypeAttribute;
                            relation.RelationType = RelationType.Many2Many;
                            relation.RelationTable = GetRelationTable(model, property.Model);
                            relation.PropertyColumn = property.Model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_ID";
                            relation.Property = property;
                            relation.TargetColumn = model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_ID";
                        } else if (refAttr.Length > 0&&property.Model !=null)
                        {

                            var refDef = refAttr[0] as Soway.Data.Discription.ORM.ReferToProperyAttrbute;
                            var target = property.Model.Properties.FirstOrDefault(p => p.Name == refDef.PropertyName || p.PropertyName == refDef.PropertyName);

                            //属性模型不为空
                            relation.RelationType = RelationType.One2Many;
                            relation.RelationTable = property.Model.DataTableName;
                            relation.PropertyColumn = property.Model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_ID";
                            relation.Property = property;
                            relation.TargetColumn = target.DBName;
                            relation.TargetProperty = target;
                        }
                        else if (property.Model != null &&
                            property.Model.Properties.Count(p => p.Model == model && p.IsArray) == 1
                            && property.Model != model)
                        {

                            //属性模型不为空
                            relation.RelationType = RelationType.Many2Many;
                            relation.RelationTable = GetRelationTable(model, property.Model);

                            relation.PropertyColumn = property.Model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_ID";

                            relation.Property = property;
                            relation.TargetColumn = model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_ID";
                            relation.TargetProperty = property.Model.Properties.First(p => p.Model == model && p.IsArray);
                        }                        else
                        if (property.Model != model)
                            {
                                relation.RelationType = RelationType.One2Many;
                                relation.RelationTable = property.Model.DataTableName.Replace("[", "").Replace("]", "").Trim();

                            }
                            else
                            {
                                relation.RelationType = RelationType.Recurve;
                                relation.RelationTable = model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_" + property.Name;
                                relation.PropertyColumn = model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_" + property.Name.ToUpper() + "_ITEM";

                            }


                            if (model.AutoSysId || (model.IdProperty != null && model.IdProperty.DBName.ToUpper().Trim() == "SYSID"))
                            {
                                relation.TargetProperty = null;
                                relation.TargetColumn = model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_" + property.Name + "_SYSID";

                            }
                            else
                            {
                                //这里的逻辑是有问题的,回来得改
                                if (type.BaseType.IsGenericType &&
                                    type.BaseType.GetGenericTypeDefinition() == typeof(Soway.Data.ObjectWithSubItem<>))
                                {

                                    relation.TargetProperty = property.Model.Properties.FirstOrDefault(p => p.PropertyName == "Parent");
                                    relation.TargetColumn = property.Model.Properties.FirstOrDefault(p => p.PropertyName == "Parent").DBName;
                                }
                                else if (String.IsNullOrEmpty(relation.TargetColumn))
                                {
                                    relation.TargetColumn = model.DataTableName.Replace("[", "").Replace("]", "").Trim() + "_" + property.Name + model.IdProperty.DBName;
                                }
                            }



                        }
                    catch (Exception ex)
                    {
                      
                        property.DBName = "";
                    }

                }
            }
        }
      
        private Dictionary<Assembly, Module> moduleCache = new Dictionary<Assembly, Module>();
        private Module GetModule(System.Reflection.Assembly Ass)
        {
            if (moduleCache.ContainsKey(Ass) )
            {

                return moduleCache[Ass];
            }
            else
            {
                Module Module = new Module();
                Module.Assembly = Ass.GetName().Name;
             
                Module.GerationDLL = true;
                Module.Name = Ass.GetName().Name;
                Module.Remartk = "";
                Module.Verstion = Ass.GetName().Version.ToString();
                Module.AssemblyFile = Ass.Location;
                moduleCache.Add(Ass,Module);
                return Module;
            }
        }
        public List<Module> GetModules()
        {
            return moduleCache.Values.ToList();
        } 
        public List<Module> GetRefrenceModules(Module Module)
        {

            Assembly ass = null;

            if (moduleCache.ContainsValue(Module))
                ass = moduleCache.FirstOrDefault(p => p.Value == Module).Key;
            else
            {
                try
                {
                    var assName = Module.Assembly;

                    ass = System.Reflection.Assembly.LoadFile(Module.AssemblyFile);
                }
                catch
                {
                    ass = System.Reflection.Assembly.LoadFile(Module.Assembly + ".dll");
                }
            }

            List<Module> result = new List<Module>();

         
            foreach (var RefrenceAss in ass.GetReferencedAssemblies())
            {

                var rass = System.Reflection.Assembly.Load(RefrenceAss);
                if(rass.GetExportedTypes().Count(p=>p.GetCustomAttributes(typeof(Soway.Data.Discription.ORM.TableAttribute),true).Count()>0)>0)
                result.AddRange(GetRefrenceModules(GetModule(rass)).ToArray());
                    
            }
            Module.Depdency = new List<Soway.Model.Module>();
            Module.Depdency.AddRange(result.Distinct().ToArray());
            result.Add(GetModule(ass));
            return result; 
        }

        public AssemblyModelFactory(System.Reflection.Assembly assembly)
        {
           
            this.Assembly = assembly;
            GetRefrenceModules(GetModule(assembly));
        
        }


        public AssemblyModelFactory(String fileName)
        {

            var assemblyName = fileName;
            if (System.IO.File.Exists(assemblyName) == false)
                assemblyName = new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + "\\" + fileName + ".dll";

            this.Assembly = System.Reflection.Assembly.LoadFile(assemblyName);
            GetRefrenceModules(GetModule(this.Assembly));
        }

        public AssemblyModelFactory(Type type)
        {
            this.Assembly = type.Assembly;

            GetRefrenceModules(GetModule(this.Assembly));
        }



        public void InstallModel(Module Module)
        {
            throw new NotImplementedException();
        }
    }
}
