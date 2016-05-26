using Soway.Model.SqlServer;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model
{
    public class ModelMethodContext
    {
        public SqlCon Con { get; private set; }
        public ICurrentContextFactory ContextFac { get; private set; }

        /// <summary>
        /// 对一个operation执行一个操作
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="operation"></param>
        public void ExcuteOperation(IObjectProxy ob, IOperation operation)
        {


            List<object> param = new List<object>();
            List<object> contravlue = new List<object>();

            
            
            foreach (var command in operation.GetCommands().OrderBy(p=>p.Index))
            {
                switch (command.CommandsType)
                {
                    case CommandsType.SetAccess:
                        //设置一个属性是否可用,这段是没问题的。。
                        ob.NotifyPropertyCanSet(command.Property, System.Convert.ToBoolean(command.Exp)); 
                        break;
                    case CommandsType.SetValue:
                        //给一个属性赋值,这段也OK，要好发写一下GetValue
                        ob[command.Property] = GetValue(command.Property,ob, command.Exp);
                        break;
                    case CommandsType.ExuteProprtyModelMethod:
                        //执行属性的模型操作
                        ExuctePropertyModelMethod(ob, command);
                        break;
                    case CommandsType.ExuteListMethod:
                        ///执行List的操作 也没问题。
                        ExcuteListMethod(ob, command);
                        break;
                    case CommandsType.ExuteOutModelMethod :
                        ///执行外部模型的操作的
                        var desmodel =this.ExcuteOperation(
                            command.ArgModel,
                            command.ArgModel.Operations.FirstOrDefault(p => p.Name.Trim().ToUpper() == command.Exp.Trim().ToUpper()),
                            ob,
                            String.IsNullOrEmpty(command.ArgSourceIdExp)?"":GetValue(command.Property,ob, command.ArgSourceIdExp));
                        if (desmodel !=null && command.Property != null)
                            ob[command.Property] = GetValue(command.Property,desmodel, command.ArgPropertyExp);
                        break;
                    case CommandsType.Filter:
                        CheckCommand(command, ob);
                        break;
                    case CommandsType.SetParamValue:
                        var paramOb = GetValue(command.Property, ob, command.Exp);
                        var valueOb = paramOb;
                        if(paramOb is IObjectProxy)
                        {
                            valueOb = new ModelHelper(this.ContextFac).GetFromProxy(valueOb as IObjectProxy);
                             

                        }
                        param.Add(valueOb);
                        break;

                    case CommandsType.SetConStrValue:
                          paramOb = GetValue(command.Property, ob, command.Exp);
                          valueOb = paramOb;
                        if (paramOb is IObjectProxy)
                        {
                            valueOb = new ModelHelper(this.ContextFac).GetFromProxy(valueOb as IObjectProxy);


                        }

                        contravlue.Add(valueOb);
                        break;
                    default:
                        break;
                }
            }
            var db = new SqlServer.dbContext((ob.Model.SqlCon == null ? ob.Model.Module.SqlCon : ob.Model.SqlCon),this.ContextFac);
            //// System.Diagnostics.Trace.WriteLine("Operatoin Type :" + operation.BaseOperationType);
            switch (operation.BaseOperationType)
            {
                case BaseOperationType.Create :
                    db.Create(ob);
                    break;
                case BaseOperationType.Delete:
                    db.Delete(ob);
                    break;
                case BaseOperationType.Update:
                    db.Save(ob);
                    break;
                case BaseOperationType.Assebmly :
                    {

                        
                        var ass =System.Reflection.Assembly.Load(operation.InvokeDLL);
                        var invokeOb = ass.CreateInstance(operation.InvokeClass,true,System.Reflection.BindingFlags.CreateInstance,null,contravlue.ToArray(),null,null);
                        var type = ass.GetType(operation.InvokeClass);
                        var invokemethod = type.GetMethod(operation.InvokeMethod);
                        if (String.IsNullOrEmpty(ob.Model.ClassName))
                        {
                            //invokemethod.Invoke(invokeOb, new object[] { ob });
                            param.Insert(0,ob);
                        }
                        else
                        {
                            var busiOb = new ModelHelper(this.ContextFac).GetFromProxy(ob);
                            param.Insert(0,busiOb);
                         
                        }
                    

                        invokemethod.Invoke(invokeOb, param.ToArray());
                    }
                    break;
                default :
                    break;
                    

            }
        }
        private void CheckCommand(ICommand command, IObjectProxy ob)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(SqlHelper.GetSqlCon(this.Con,ob.Model).ToString()))
            {
                con.Open();
                var sqlcommand = SqlHelper.GenerateGetItemsCommand(ob.Model);
                sqlcommand.Connection = con;
                var keycol = SqlHelper.GetKeyCol(ob.Model);
                sqlcommand.CommandText += " WHERE [" + keycol + "]=@" + keycol;
                sqlcommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + keycol, ob.ID));
                sqlcommand.CommandText += string.Format(" AND {0}", command.Exp);
                var table = SqlDataLoader.GetSqlData(sqlcommand);
                if (table.Rows.Count == 0)
                    throw new Exception(command.ArgPropertyExp);

            }

        
             
        }

        private   void ExcuteListMethod(IObjectProxy ob, ICommand command)
        {
            DynamicObject proxyArray = ob[command.Property] as DynamicObject;
            object ob2 = new object();
            proxyArray.TryInvokeMember(new ObjectInvokeMemberBinder(command.Exp), new object[] { }
                , out ob2);
        }
        /// <summary>
        /// 这个函数是OK的
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="command"></param>
        private   void ExuctePropertyModelMethod(IObjectProxy ob, ICommand command)
        {
            if (command.Property.IsArray == false)
            {
                DynamicObject proxy = ob[command.Property] as DynamicObject;
                object ob1 = new object();
                proxy.TryInvokeMember(new ObjectInvokeMemberBinder(command.Exp), new object[] { 
 }
                    , out ob1);
            }
            else
            {
                ModelBindingList list = ob[command.Property] as ModelBindingList;
                object tempOb;
                foreach (DynamicObject item in list)
                {
                    item.TryInvokeMember(new ObjectInvokeMemberBinder(command.Exp), new object[] { }
                    , out tempOb);
                }
            }
        }

 
        /// <summary>
        /// 执行外部模型的操作
        /// </summary>
        /// <param name="model">外部的模型</param>
        /// <param name="operation">外部的模型的操作</param>
        /// <param name="arg">参数 arg的Model为源模型</param>
        /// <param name="id">外部模型的ID</param>
        /// <returns>生成的ObjectProxy</returns>
            public IObjectProxy ExcuteOperation(Model model, IOperation operation, IObjectProxy arg, object id)
            {

            //如果为空，返回默认值 
            //明天个性
            //Set Value 如果为BusinisObject 返回默认值 ~~~ 笨的!!!!

            #region 初始化要操作的值,即要返回的值resultProxy
            var db = new SqlServer.dbContext((model.SqlCon == null ? model.Module.SqlCon : model.SqlCon),this.ContextFac);
             IObjectProxy resultproxy = null;
             if (operation == null)
                 return db.GetDetail(model, id);
            List<object> invokeparams = new List<object>();
            switch (operation.BaseOperationType)
            {
                case  BaseOperationType.Create:
                    resultproxy = new SqlDataProxy(model,this.ContextFac,LoadType.Null,this.Con);
                    break;
                case BaseOperationType.Delete:
                case BaseOperationType.Update:
                    resultproxy = db.GetDetail(model, id);
                    break;
                default :
                    return null;
            }
            #endregion

            foreach (var command in operation.GetCommands().OrderBy(p=>p.Index))
            {

                switch (command.CommandsType)
                {
                    case CommandsType.SetAccess:
                        resultproxy.NotifyPropertyCanSet(command.Property, System.Convert.ToBoolean(command.Exp));
                        break;
                    case CommandsType.SetValue:
                        resultproxy[command.Property] = GetValue(command.Property, arg, command.Exp);
                        break;
                    case CommandsType.ExuteProprtyModelMethod:
                        DynamicObject proxy = resultproxy[command.Property] as DynamicObject;
                        object ob1 = new object();
                        proxy.TryInvokeMember(new ObjectInvokeMemberBinder(command.Exp), new object[] { }
                            , out ob1);
                        break;
                    case CommandsType.ExuteOutModelMethod:
                         var desmodel =this.ExcuteOperation(
                            command.ArgModel,
                            command.ArgModel.Operations.FirstOrDefault(p => p.Name.Trim().ToUpper() == command.Exp.Trim().ToUpper()),
                            resultproxy,
                            GetValue(command.Property,resultproxy, command.ArgSourceIdExp));
                        if(desmodel != null &&command.Property != null)
                            resultproxy[command.Property] = GetValue(command. Property ,desmodel, command.ArgPropertyExp);
                        break;
                    case CommandsType.Filter:
                        CheckCommand(command, resultproxy);
                        break;
                    default:
                        break;



                }

            }
            var db2 = new SqlServer.dbContext((model.SqlCon == null ? model.Module.SqlCon : model.SqlCon),this.ContextFac);
                  switch (operation.BaseOperationType)
            {
                case BaseOperationType.Create:
                    db2.Create(resultproxy);
                    break;
                case BaseOperationType.Delete:
                    db2.Delete(resultproxy);
                    break;
                case BaseOperationType.Update:
                    db2.Save(resultproxy);
                    break;
                default:
                    break;


            }
                  return resultproxy;
        }
        /// <summary>
        /// 执行一个操作
        /// </summary>
        /// <param name="operation"></param>

        public object GetValue(Property property,IObjectProxy ob,String exp)
        {

            return new Expressions.GetValueExpression(this.ContextFac).GetValue(ob, property, exp);
        }

        public ModelMethodContext(SqlCon con ,Soway.Model.Context.ICurrentContextFactory conFac)
        {
            this.Con = con;
            this.ContextFac = conFac;
        }

    }
}
