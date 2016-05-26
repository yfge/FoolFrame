using Soway.Data;
using Soway.Model.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public class SqlDataProxy : ObjectProxyClass
    {
       

        public override object this[String exp]
        {
            get
            {

                return IObjectFactory.GetPropertyValue(this, exp);
                
           
            }
            set
            {
                IObjectFactory.SetPropertyValue(this, exp, value);
           
            }
        }

 

        public override object this[Property index]
        {
            get
            {
              
                var indexproperty = this.KeyPairs.Keys.FirstOrDefault(p => p.Name == index.Name);
                if (index.IsArray)
                {
                    if (this.KeyPairs[indexproperty].IsLoad ==  LoadType.Null)
                    {
                     
                        this.KeyPairs[indexproperty].IsLoad =  LoadType.Complete;
                        var items = (this.KeyPairs[indexproperty].Data as dynamic);

                        if (this.ID != null)
                            foreach (var item in new Soway.Model.SqlServer.dbContext(SqlHelper.GetSqlCon(index,this.Con,this.Model),this.ConFac).LoadArrayProperty(this, indexproperty))
                            {
                                item.IsLoad =  LoadType.Complete;
                                items.Add(item);
                                       if (index.PropertyType != PropertyType.BusinessObject)
                                    item.IsLoad = LoadType.Complete;
                      
                            }

                        this.KeyPairs[indexproperty].IsLoad = LoadType.Complete;

                    }
                }
                else
               
                    if (index.PropertyType == PropertyType.BusinessObject)
                    {

                

                    if (this.KeyPairs[indexproperty].IsLoad !=   LoadType.Complete
                          )//this.KeyPairs[index.Name].Data == null)
                        {
                
                        var proxy = this.KeyPairs[indexproperty].Data as IObjectProxy;
                            this.KeyPairs[indexproperty].IsLoad =  LoadType.Complete;
                            if (proxy != null &&
                                (proxy.IsLoad ==  LoadType.Null
                                && string.IsNullOrEmpty((proxy.ID ?? "").ToString()) == false))
                            {
               
                            this.KeyPairs[indexproperty].Data =  new Soway.Model.SqlServer.dbContext(
                                    SqlHelper.GetSqlCon(index,this.Con,proxy.Model),this.ConFac).GetDetail(proxy.Model, proxy.ID);


                            }
                        }
                    }
                    else
                    {
                        if (this.KeyPairs[indexproperty].IsLoad == LoadType.Null

                         && indexproperty != this.Model.IdProperty
                            )
                    {
                        
                        if(this.ID!= null) 
                        new Soway.Model.SqlServer.dbContext(
                                    SqlHelper.GetSqlCon(index,this.Con,this.Model),this.ConFac 
                                    ).LoadDataDetail(this.Model,
                                    this.ID, true, this);

                        }

                    }
               

                return this.KeyPairs[indexproperty].Data;


            }
            set
            {
                var indexproperty = this.KeyPairs.Keys.FirstOrDefault(p => p.Name == index.Name);
                if (value is string && (
                    index.PropertyType != PropertyType.String
                    && index.PropertyType != PropertyType.SerialNo
                    && index.PropertyType != PropertyType.RadomDECS
                    && index.PropertyType != PropertyType.Radom
                    && index.PropertyType != PropertyType.Guid
                    && index.PropertyType != PropertyType.MD5))
                {
                    try
                    {
                        // new Soway.Data.Discription.ORM.ORMHelper().IsBusinessType
                        this.SetDefaultValue(index, value as string);
                        value = this[index];
                    }
                    catch (Exception EX)
                    {

                        System.Diagnostics.Trace.WriteLine(string.Format(@"property:{0},value:{1},message:{2},type:{3}", index.Name, value, EX.ToString(), index.PropertyType));
                    }
                }
                if (indexproperty == null)
                {
                    if (this.Owner != null)
                    {
                       
                        this.Owner[index.Name] = value;
                        return;
                    }
                }

                if (index.PropertyType != PropertyType.BusinessObject)
                    this.KeyPairs[indexproperty].IsLoad = LoadType.Complete;
                else
                    this.KeyPairs[indexproperty].IsLoad = LoadType.Partial;

                var old = this.KeyPairs[indexproperty].Data;
                this.KeyPairs[indexproperty].Data = value;
                if (value == null //||string.IsNullOrEmpty((value??"").ToString())
                    )
                    this.KeyPairs[index].IsLoad = LoadType.NoObj;
              


               

            
                if (index.PropertyType == PropertyType.BusinessObject && value != null){// != null)
                        this.KeyPairs[indexproperty].IsLoad = (this.KeyPairs[indexproperty].Data as IObjectProxy).IsLoad;
                    (this.KeyPairs[indexproperty].Data as IObjectProxy).Owner = this;
                     
                }


                 
                if (this.KeyPairs.Values.Count(p => p.IsLoad == LoadType.Complete
                    || p.IsLoad == LoadType.NoObj) == this.KeyPairs.Values.Count)
                {
                    this.IsLoad = LoadType.Complete;
                }
                else 
                    this.IsLoad = LoadType.Partial;
              


                if ( old != value)
                {
                    Notify( indexproperty);
               
                    var triggers = index.Triggers.Where(p => p.PropertyTriggerType == PropertyTriggerType.Set);
                    if (this.IsLoad == LoadType.Complete)
                    {
                        if (triggers.Count() > 0)
                        {
                          //  //// System.Diagnostics.Trace.WriteLine(String.Format("{0} {1} IS Loaded:{2}", this.Model.Name, this.ID, this.IsLoad));

                            var method = new ModelMethodContext(this.Con,this.ConFac);
                            foreach (var trigger in triggers)
                            {
                                method.ExcuteOperation(this, trigger);
                            }
                        }
                    }
                }
            }
        }


      

        public SqlDataProxy(Model Model, Context.ICurrentContextFactory conFac, LoadType isLoad =  LoadType.Null,SqlCon con=null ):base(Model,conFac,isLoad)
        {

            this.Con = con;
        }

     
    }
}
