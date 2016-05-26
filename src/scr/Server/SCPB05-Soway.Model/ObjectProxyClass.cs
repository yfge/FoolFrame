using Soway.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model
{
    public abstract class ObjectProxyClass: System.Dynamic.DynamicObject,IObjectProxy
    {
        public SqlCon Con { get; internal set; }
        public event System.EventHandler<ObjectPropertyCanSetChanged> OjbectPropertyCanSetChangedEventHandler;
        internal Dictionary<Property, PropertyData> KeyPairs;
        private IObjectProxy owner;
        private bool loadowner = false;
        public IObjectProxy Owner
        {
            get
            {
                if (owner == null ||loadowner==false)
                {
                    loadowner = true;
                    if (owner == null)
                    {
                        owner = new SqlServer.dbContext(Con, ConFac).GetParent(this);
                    }

                } 
                
                return owner;
            }
            set
            {
                loadowner = true;
                owner = value;
            }
        }

        private object id;
        public object ID
        {
            get
            {
                if (Model.IdProperty != null)
                {
                    return this[Model.IdProperty];
                }
                else
                {
                    return id;
                }
            }
            set
            {
                if (Model.IdProperty != null)
                {
                    　
                    this[Model.IdProperty] = value;
                }
                else
                {

                    if ((oldId != id && id != null && oldId != null)
                        ||(oldId !=null &&
                        oldId ==  Soway.Data.PropertyTypeAdaper.GetDefaultValue(
                            Soway.Data.PropertyTypeAdaper.GetPropertyType(oldId.GetType()))))
                    {
                        oldId = id;
                    }
                    id = value;


                    if (this.Model.AutoSysId == true || (this.Model.IdProperty != null && this.Model.IdProperty.PropertyType == PropertyType.SerialNo))
                        oldId = id;
                    
                }
            }
        }

        private object oldId;

        public object OldId
        {
            get
            {

                if (Model.IdProperty != null)
                    return GetOld(Model.IdProperty);
                if ((oldId == null||string.IsNullOrEmpty((oldId ??"").ToString())==true)
                    && id !=null)
                    oldId = id;
                return oldId;
            }
 
        }
 
        public Model Model
        {
            get;
            set;
        }



        public abstract  object  this[string index] { get; set; }


        

       
     
        public abstract   object this[Property index]{get;set;}


        private LoadType loadtype = LoadType.Null;
        public LoadType IsLoad
        {
            get
            {
                return loadtype;
            }
            set
            {
                loadtype = value;

            }
        }
        public ObjectProxyClass(Model Model,Context.ICurrentContextFactory conFac, LoadType isLoad = LoadType.Null  )
        {
            this.KeyPairs = new Dictionary<Property, PropertyData>();
            this.Model = Model;
            this.IsLoad = isLoad;


            this.ConFac = conFac;
            foreach (var p in this.Model.Properties)
            {
                if (p.IsArray)
                {
                    if (p.PropertyType != PropertyType.BusinessObject)
                        KeyPairs.Add(p, new PropertyData(p)
                        {
                            IsLoad = IsLoad,
                            Data = new BindingList<Object>()
                        });
                    else
                        KeyPairs.Add(p,
                            new PropertyData(p)
                            {
                                IsLoad = IsLoad,
                                Data = new ModelBindingList(p, this,this.ConFac)
                                
                            });
                }
                else
                {
                    KeyPairs.Add(p,
                        new PropertyData(p)
                        {
                            IsLoad = IsLoad,
                            Data = GetDefaultValue(p),
                              
                        });
                }
            }

            if (Model.IdProperty != null)
                this.ID = this[Model.IdProperty
];

        }


       
        protected void SetDefaultValue(Property property,string value)
        {
            switch (property.PropertyType)
            {
                case PropertyType.Boolean:
                    this[property] = Boolean.Parse(value.ToString().Trim());
                    return;
                case PropertyType.BusinessObject:
                    if(string.IsNullOrEmpty(value)==false)
                    this[property] = new SqlDataProxy(property.Model,this.ConFac, LoadType.Null, this.Con) { ID = value };
                    else
                    {
                        this[property] = null;
                    }
                    return;
                case PropertyType.Byte:
                    this[property] = byte.Parse(value);
                    return;
                case PropertyType.Char:
                    this[property] = char.Parse(value);
                    return;
                case PropertyType.Date:
                case PropertyType.DateTime:

                    this[property] = DateTime.Parse(value);
                    return;
                case PropertyType.Decimal:
                    this[property] = decimal.Parse(value);
                    return;
                case PropertyType.Double:
                    this[property] = double.Parse(value);
                    return;
                case PropertyType.Enum:
                    this[property] = int.Parse(value);
                    return;
                case PropertyType.Float:
                    this[property] = float.Parse(value);
                    return;
                case PropertyType.Guid:
                    this[property] = Guid.Parse(value);
                    return;
                case PropertyType.IdentifyId:
                    long ID = 0;
                    long.TryParse(value, out ID);
                    this[property] = ID;
                    return;
                case PropertyType.Int:
                    this[property] = int.Parse(value);
                    return;
                case PropertyType.Long:
                    this[property] = long.Parse(value);
                    return;
                case PropertyType.MD5:
                    this[property] = value;
                    return;
                case PropertyType.Radom:
                    this[property] = value;
                    return;
                case PropertyType.RadomDECS:
                    this[property] = value;
                    return;
                case PropertyType.SerialNo:
                    this[property] = value;
                    return;
                case PropertyType.String:
                    this[property] = value;
                    return;
                case PropertyType.Time:
                    this[property] = DateTime.Parse(value);
                    return;
                case PropertyType.UInt:
                    this[property] = uint.Parse(value);
                    return;
                case PropertyType.ULong:
                    this[property] = ulong.Parse(value);
                    return;
                default:
                    return;
            }

        }

        private object GetDefaultValue(Property property)
        {
            switch (property.PropertyType)
            {
                case PropertyType.Boolean:
                    return false;
   
                case PropertyType.Byte:

                    decimal a = 0;
                    return a;
                case PropertyType.Char:
                    char c = '\0';
                    return c;
                case PropertyType.Date:
                  
                case PropertyType.DateTime:
                    return DateTime.MinValue;
                case PropertyType.Decimal:
                    decimal d = 0;
                    return d;
                case PropertyType.Double:
                    double e = 0;
                    return e;
                case PropertyType.Enum:

                    int enumI = property.Model.EnumValues.First().Value;
                    return enumI;
                case PropertyType.Float:
                    float f = 0;
                    return f;
                case PropertyType.Int:
                    int intresult = 0;
                    return intresult;
                case PropertyType.Long:
                case PropertyType.IdentifyId:
                    long longresult = 0;
                    return longresult;
                case PropertyType.String:
                case PropertyType.SerialNo:
                    return "";
                case PropertyType.Time:
                    return DateTime.MinValue;
                case PropertyType.UInt:
                    uint uintResult = 0;
                    return uintResult;
                case PropertyType.ULong:
                    ulong ulongResult = 0;
                    return ulongResult;
                default:
                    return null;
            }


        }
        public override string ToString()
        {

            if (this.Model.ShowProperty != null)
                return (this[this.Model.ShowProperty] ?? "").ToString();

            if (this.Model.EnumValues.Count > 0)
            {
                var item = this.Model.EnumValues.FirstOrDefault(p => p.Value == (int)this.ID);
                if (item != null)
                    return item.String;
            }
            return (this.ID ?? "").ToString();

        }




        public event System.EventHandler<ObjectValueChangedEventArgs> PropertyChangedEventHandler;



        


        protected void Notify(Property property)
        {
            if (this.PropertyChangedEventHandler != null)
                this.PropertyChangedEventHandler(this, new ObjectValueChangedEventArgs() { ChangedProperty = property });
        }
        public void NotifyPropertyCanSet(Property proerty, bool canSet)
        {
            if (this.OjbectPropertyCanSetChangedEventHandler != null)
                this.OjbectPropertyCanSetChangedEventHandler(this,
                    new ObjectPropertyCanSetChanged() { proerty = proerty, CanSet = canSet });
        }
        
        

        #region 重写

        public override IEnumerable<string> GetDynamicMemberNames()
        {

            List<String> names = new List<string>();
            names.AddRange(this.KeyPairs.Keys.Select(p => p.Name).ToArray());
            names.AddRange(this.Model.Operations.Select(p => p.Name).ToArray());
            return names;
        }
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {

            var property = this.Model.Properties.FirstOrDefault(p => p.Name == binder.Name || p.PropertyName == binder.Name);
            if (property != null)
            {
                result = this[property];
                return true;
            }
            else
            {
                return base.TryGetMember(binder, out result);
            }
        }
        public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        {
            var property = this.Model.Properties.FirstOrDefault(p => p.Name == binder.Name || p.PropertyName == binder.Name);
            if (property != null)
            {
                this[property] = value;
                return true;
            }
            else
            {
                return base.TrySetMember(binder, value);
            }
        }
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {

            var operation = this.Model.Operations.FirstOrDefault(p => p.Name.Trim().ToUpper() == binder.Name.ToUpper().Trim());
            if (operation != null)
            {

                var method = new ModelMethodContext(this.Con,this.ConFac);
                if (operation.ArgModel != null)
                {

                    result = method.ExcuteOperation(operation.ArgModel, operation,this,method.GetValue(null, this,operation.ArgFilter));
                    return true;
                }
                else
                {
                    method.ExcuteOperation(this,operation);
                    result = null;
                    return true;

                }
                
     
            }
            return base.TryInvokeMember(binder, args, out result);
        }
        #endregion


        public SaveType IsSave
        {
            get;
            set;
        }
        public ICurrentContextFactory ConFac { get; private set; }

        public object GetOld(string exp)
        {


            var property = this.Model.Properties.FirstOrDefault(p => p.PropertyName == exp || p.Name == exp);
            return GetOld(property);
        }

        public object GetOld(Property exp)
        {
            
            return this.KeyPairs[exp].Old;
        }


        public object GetOrgi(Property exp)
        {
            return this.KeyPairs[exp].Origin;

        }

        public void  
            UpdateToNew(Property exp)
        {
            this.KeyPairs[exp].UpdateToNew();
            if (Model.IdProperty != null && Model.AutoSysId == false &&
                Model.IdProperty ==exp)
            {
                ID = this[exp];
                ID = this[exp];
               
            }

        }

        public LoadType GetLoadType(Property exp)
        {

            var property = this.Model.Properties.FirstOrDefault(p => p.Name == exp.Name);
            if (property != null)
                return this.KeyPairs[property].IsLoad;
            return LoadType.NoObj;
        }
    }
}
