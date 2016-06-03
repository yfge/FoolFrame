using Soway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model.Context;

namespace Soway.Model.Expressions
{

    /// <summary>
    /// 表达式
    /// </summary>
    /// <remarks>
    /// 说明 
    /// exp 的约定 -- 以第一个字符开始
    /// . --- 是一个属性
    /// $ ----是一个常量
    /// @ --- 当前的状态值  ，其中@userid ,@username,@clientaddress,@datetime,@date,@time 
    /// 
    /// 支持运算     +，-，*，/,(,)  
    /// </remarks>
    public  class GetValueExpression
    {
        public ICurrentContextFactory ContextFac { get; private set; }



        public GetValueExpression(Context.ICurrentContextFactory confac)
        {
            this.ContextFac = confac;
        }
        public  object GetPropertyValue(IObjectProxy ob, Property propery)
        {
            return ob[propery];
        }

        public  object GetValue(IObjectProxy ob, Property property, string exp)
        {

            if (Soway.Data.Math.MathExpression.IsMathexprese(exp))
            {
                var mathexp = new Soway.Data.Math.MathExpression();
                var math =  mathexp.CalculateParenthesesExpression(
                    exp, new Data.Math.MathExpression.GetOperator((string simpleexp) =>
                    {
                        return GetValueSingle(ob, property, simpleexp).ToString();
                    }));
                return GetStaticVlue(property,math
                );
            }else{
                return GetValueSingle(ob,property,exp);
            }


        }
        private Object GetValueSingle(IObjectProxy ob, Property property, string exp)
        {



            switch (exp[0])
            {

                case '#':
                    return GetValueSingle(ob.Owner, property, exp.Substring(1));

                case '.':


                    if (string.IsNullOrEmpty(exp.Substring(1)))
                        return null;
                    return ob[exp.Substring(1)];
            
                case '$':
                    return GetStaticVlue(property, exp.Substring(1));
                case '@':
                
                    return GetContexValue(exp.Substring(1));
                  //  return ob;

            

                default:
                    return "";


            }
            }
        private object GetStaticVlue(Property type, string exp)
        {
            switch (type.PropertyType)
            {
                case PropertyType.Boolean:
                    return System.Convert.ToBoolean(exp);
                case PropertyType.Byte:
                    return System.Convert.ToByte(exp);
                case PropertyType.Char:
                    return System.Convert.ToChar(exp);
                case PropertyType.Date:
                case PropertyType.Time:
                    return exp;
                case PropertyType.DateTime:
                    return System.Convert.ToDateTime(exp);
                case PropertyType.Int:
                case PropertyType.UInt:
                case PropertyType.Long:
                case PropertyType.ULong:
                    return System.Convert.ToInt32(exp);
                case PropertyType.Decimal:
                    return System.Convert.ToDecimal(exp);
                case PropertyType.Double:
                case PropertyType.Float:

                    return System.Convert.ToDouble(exp);
                case PropertyType.String:
                case PropertyType.Radom:
                case PropertyType.SerialNo:
                    return exp;
                case PropertyType.BusinessObject:

                    return new SqlServer.dbContext(type.Model.SqlCon ?? type.Model.Module.SqlCon,this.ContextFac).GetDetail(
                        type.Model, exp);
                default:
                    return exp;
            }
        }
       // 其中@userid ,@username,@clientaddress,@datetime,@date,@time ,@appcon,@currentcon,@syscon
        public object  GetContexValue(string exp){
            var tempexp = exp.Trim().ToLower();
            var context = this.ContextFac.GetCurrentContext();
            var date = this.ContextFac.GetDateTime();
            //// System.Diagnostics.Trace.WriteLine("GetValue:" + exp);
            switch (tempexp)
            { 
                case "userid":
                    return context.UserId;
                case "username":
                    return context.UserName;
                case "clientaddress":
                    return context.Address;
                case "datetime":
                    return date;
                case "syscon":
                    return context.SysCon;
                case "modelcon":
                    return context.ModelCon;
                case "appcon":
                    return context.AppCon;
                case "datacon":
                    return context.CurrentCon;
                case "date":
                    return date.Date;
                case "time":
                    return date;
                case "context":
                    return this.ContextFac;
                
                default :
                    return this.ContextFac.GetValue(tempexp);
                    
                    

            }

        }
    }
}
