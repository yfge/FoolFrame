using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model
{
    public class ModelHelper
    {

        public ModelHelper(Context.ICurrentContextFactory conFac)
        {
            this.ConFac = conFac;
        }

        public dynamic GetFromProxy(IObjectProxy proxy)
        {
            if (proxy.Model.ModelType != ModelType.Enum)
            {
                Dictionary<Model, Dictionary<string, object>> ProxyDic = new Dictionary<Model, Dictionary<string, object>>();
                var ob = getFromProxy(proxy, ProxyDic);
                return ob;
            
            }
            else
            {
                return proxy.ID;
            }
        }


        private dynamic getFromProxy(IObjectProxy proxy, Dictionary<Model, Dictionary<string, object>> ProxyDic)
        {

            dynamic ob;
          

            if (ProxyDic.ContainsKey(proxy.Model) == false)
            {

                ProxyDic.Add(proxy.Model, new Dictionary<string, object>());
            
            }
            var modeItems = ProxyDic[proxy.Model];
            if (modeItems.ContainsKey(proxy.ID.ToString()) == false)
            {
                 
                modeItems.Add(proxy.ID.ToString(), System.Reflection.Assembly.Load(proxy.Model.Module.Assembly).CreateInstance(proxy.Model.ClassName));
            }
            else
            {
                return modeItems[proxy.ID.ToString()];
            }
            ob = modeItems[proxy.ID.ToString()];
            foreach (var proerty in proxy.Model.Properties.Where(p=>String.IsNullOrEmpty(p.PropertyName)==false))
            {

                var op =
                new ReflectionPropertyOperation(proerty, proxy.Model);

                if (proerty.IsArray == false)
                {
                    if (proerty.PropertyType != PropertyType.BusinessObject)
                        op.Set(ob,proxy[proerty]);
                    else
                    {

                        IObjectProxy  proertyProxy = proxy[proerty] as IObjectProxy;
                        if (proertyProxy != null)
                        {
                            op.Set(ob, getFromProxy(proertyProxy, ProxyDic));
                           
                        }
                    }
                }
                else
                {
                    

                    dynamic items = op.Get(ob);
                    if (items == null)
                    {
                        //如果集合类型为空，则创建 
                        //return;
                       // items  = op.
                        //items = 
                    }
                    if (items != null)
                    {


                        dynamic modelItem = proxy[proerty];



                        foreach (var i in modelItem)
                        {
                         
                            if (proerty.PropertyType != PropertyType.BusinessObject)
                                items.Add(i);
                            else
                            {
                               items.Add(getFromProxy(i as IObjectProxy,ProxyDic));
                         
                            }
                        }
                    }
                }
            }
            return ob;

        }

        internal void SetProxy<T>(ref ObjectProxy proxy, T obj)
        {
        
        }

        private      Dictionary<object, IObjectProxy> ProxyDic = new Dictionary<object, IObjectProxy>();
        private ICurrentContextFactory ConFac;

        /// <summary>
        /// 将object值赋给proxy
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="ob"></param>
        public void SetProxy(ref IObjectProxy  proxy, object ob)
        {
            if (proxy.Model.ModelType != ModelType.Enum)
            {
               
                setProxy( ref proxy, ob, ProxyDic);
            }
            else
            {

                proxy.ID = ob;
            }

        }

        private void setProxy(ref IObjectProxy proxy, object ob, Dictionary<object, IObjectProxy> ProxyDic)
        {

            if(ProxyDic .ContainsKey(ob))
            {
                proxy = ProxyDic[ob];
            }
            else
            {
                ProxyDic.Add(ob, proxy);
               
            }
     
            foreach (var proerty in proxy.Model.Properties)
            {

                var op =
                new ReflectionPropertyOperation(proerty, proxy.Model);
                if (proerty.IsArray == false)
                {
                    if (proerty.PropertyType != PropertyType.BusinessObject)
                        proxy[proerty] = op.Get(ob);
                    else
                    {
                       
                        var propertyValue = op.Get(ob);
                        if (propertyValue != null)
                        {
                        
                            if (ProxyDic.ContainsKey(propertyValue))
                            {
                                proxy[proerty] = ProxyDic[propertyValue];
                            }
                            else
                            {

                                if (proerty.Model != null)
                                {
                                    
                                    IObjectProxy pob = new ObjectProxy(proerty.Model,this.ConFac);

                                    ProxyDic.Add(propertyValue, pob);
                                    this.setProxy(ref pob, propertyValue, ProxyDic);
                                    proxy[proerty] = pob;
                                }
                            }
                        }
                    }
                }
                else
                {   
                    dynamic items = op.Get(ob);
                    if (items != null)
                    {
                        dynamic addItem = proxy[proerty];
                        addItem.Clear();
                        foreach (var i in items)
                        {
                           ;
                            if (proerty.PropertyType != PropertyType.BusinessObject)
                                addItem.Add(i);
                            else
                            {
                                if (ProxyDic.ContainsKey(i))
                                    addItem.Add(ProxyDic[i]);
                                else
                                {
                                    IObjectProxy itemProxy = new ObjectProxy(proerty.Model,this.ConFac);
                                    this.setProxy(ref itemProxy, i,ProxyDic);
                                    addItem.Add(itemProxy);
                                }
                            }
                        }
                    }
                }
            }
        }
        

      


    }
}
