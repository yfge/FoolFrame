using System;
using System.Collections.Generic;
using System.Text;
using Soway.Query.BoolExp;
using Soway.Query.Entity;


namespace Soway.Query.BoolExp
{

    /// <summary>
    /// 布尔表达式的抽象工厂，用于成成表达式
    /// </summary>
    public class BoolExpresstionFacotry
    {
        public QueryInstance Ins { get; private set; }

        /// <summary>
        /// 返回 col op value所形成的结果
        /// </summary>
        /// <param name="Col">比较的列名</param>
        /// <param name="Op">比较的操作符</param>
        /// <param name="Value">比较的实际值</param>
        /// <param name="ShowValue">比较的表达值</param>
        /// <returns>组成而成的布尔表达式</returns>
        public IBoolExpressionNode CreateBoolExpression(CompareCol
            
            Col, CompareOp Op, String Value, String ShowValue,string paramName)
        {
            return    new SimpleBoolExpression(Col, Op, Value, ShowValue,this.Ins, paramName)  ;
        }


        /// <summary>
        /// 在Exp1的子序列上加入 op exp2的操作
        /// </summary>
        /// <example>
        /// 即，原为 exp1 
        /// 此操作相当于 exp1=exp1 op exp2
        /// </example>
        /// <param name="Exp1"></param>
        /// <param name="Op"></param>
        /// <param name="Exp2"></param>
        public void  AddBoolExpression(BoolExpression Exp1, BoolOp Op, BoolExpression Exp2)
        {


             
            var Exp = Exp1.Exp;
            var ComplexExp = Exp as ComplexBoolExpression;

             if (ComplexExp == null)
             {
                 ComplexExp = new ComplexBoolExpression(this.Ins);
                 ComplexExp.First = new BoolExpression() { Exp = Exp1.Exp };
             }

             ComplexExp.Sequeces.Add(new AddBoolExpression(Op, Exp2));
             Exp1.Exp = ComplexExp;
            
            
        }

        /// <summary>
        /// 完成类似于exp1 = exp1 op col comop vlaue操作
        /// </summary>
        /// <param name="Exp1"></param>
        /// <param name="Op"></param>
        /// <param name="Col"></param>
        /// <param name="ComOp"></param>
        /// <param name="Value"></param>
        /// <param name="ShowValue"></param>
        public void AddBoolExpression(BoolExpression Exp1,
            BoolOp Op, CompareCol Col, CompareOp ComOp, String Value, String ShowValue,String paramName)
        {


            var Exp2 = new BoolExpression() { Exp = new SimpleBoolExpression(Col, ComOp, Value, ShowValue,Exp1.QueryIns,paramName) };

            var Exp = Exp1.Exp;
            var ComplexExp = Exp as ComplexBoolExpression;

            if (ComplexExp == null)
            {
                ComplexExp = new ComplexBoolExpression(this.Ins);
                ComplexExp.First = new BoolExpression() { Exp = Exp1.Exp };
            }

            ComplexExp.Sequeces.Add(new AddBoolExpression(Op, Exp2));
            Exp1.Exp = ComplexExp;
        }


        public BoolExpresstionFacotry (QueryInstance ins)
        {
            this.Ins = ins;
        }

    }
}
