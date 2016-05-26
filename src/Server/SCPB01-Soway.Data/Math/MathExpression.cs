using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data.Math
{

    /// <summary>
    /// 表示个四则混合的表达式
    /// </summary>
    public class MathExpression
    {




        public static bool IsMathexprese(string exp)
        {
           
            foreach(var op in operators )
            {
                if (exp.Contains(op))
                    return true;
            }
            return false;
        }
        

            public delegate string GetOperator(String Exp);
            private static  string opRatorString = "+-()*/";
            private static  char[] operators = { '+', '-', '(', ')', '*', '/' };


            //中序转换成后序表达式再计算
            // 如：23+56/(102-100)*((36-24)/(8-6))
            // 转换成：23|56|102|100|-|/|*|36|24|-|8|6|-|/|*|+"
            //以便利用栈的方式都进行计算。
            /// <summary>
            /// 计算表达式的值
            /// </summary>
            /// <param name="Expression">表达式，传入的值可以有变量 </param>
            /// <param name="GetOP">计算变量值的委托</param>
            /// <returns>计算的结果</returns>
            /// <remarks>
            /// 这里面，GETOP委托是为了将变量转化为数值
            /// 如
            /// <example>
            /// <code>
            /// public string Get(String str){
            /// if(Str=="A")
            ///     return "1";
            /// else if (Str=="B")
            ///     return "2";
            /// return "0";
            /// }
            /// public void Foo(){
            /// string c  =new  MathExpression().CalculateParenthesesExpression("A+B",new GetOperator(Get));
            /// ////c = "3";
            /// }
            /// 
            /// </code></example>
            /// </remarks>
            public string CalculateParenthesesExpression(string Expression, GetOperator GetOP)
            {
                ArrayList operatorList = new ArrayList();
                string operator1;
                string ExpressionString = "";
                string operand3;
                string[] experison = Expression.Split(operators, StringSplitOptions.RemoveEmptyEntries);
                if (experison.Length > 0)
                    for (int i = 0; i < experison.Length; i++)
                        Expression = Expression.Replace(experison[i], GetOP(experison[i]));

                while (Expression.Length > 0)
                {
                    operand3 = "";
                    //取数字处理
                    if (opRatorString.IndexOf(Expression[0]) < 0)
                    {
                        while (opRatorString.IndexOf(Expression[0]) < 0)
                        {
                            operand3 += Expression[0].ToString();
                            Expression = Expression.Substring(1);
                            if (Expression == "") break;


                        }
                        ExpressionString += operand3.ToString() + "|";
                    }

                    //取“(”处理
                    if (Expression.Length > 0 && Expression[0].ToString() == "(")
                    {
                        operatorList.Add("(");
                        Expression = Expression.Substring(1);
                    }

                    //取“)”处理
                    operand3 = "";
                    if (Expression.Length > 0 && Expression[0].ToString() == ")")
                    {
                        do
                        {

                            if (operatorList[operatorList.Count - 1].ToString() != "(")
                            {
                                operand3 += operatorList[operatorList.Count - 1].ToString() + "|";
                                operatorList.RemoveAt(operatorList.Count - 1);
                            }
                            else
                            {
                                operatorList.RemoveAt(operatorList.Count - 1);
                                break;
                            }

                        } while (true);
                        ExpressionString += operand3;
                        Expression = Expression.Substring(1);
                    }

                    //取运算符号处理
                    operand3 = "";
                    if (Expression.Length > 0 && (Expression[0].ToString() == "*" || Expression[0].ToString() == "/" || Expression[0].ToString() == "+" || Expression[0].ToString() == "-"))
                    {
                        operator1 = Expression[0].ToString();
                        if (operatorList.Count > 0)
                        {

                            if (operatorList[operatorList.Count - 1].ToString() == "(" || verifyOperatorPriority(operator1, operatorList[operatorList.Count - 1].ToString()))
                            {
                                operatorList.Add(operator1);
                            }
                            else
                            {
                                operand3 += operatorList[operatorList.Count - 1].ToString() + "|";
                                operatorList.RemoveAt(operatorList.Count - 1);
                                operatorList.Add(operator1);
                                ExpressionString += operand3;

                            }

                        }
                        else
                        {
                            operatorList.Add(operator1);
                        }
                        Expression = Expression.Substring(1);
                    }
                }

                operand3 = "";
                if (operatorList.Count == 0)
                    return ExpressionString.Substring(0, ExpressionString.Length - 1);
                while (operatorList.Count != 0)
                {
                    operand3 += operatorList[operatorList.Count - 1].ToString() + "|";
                    operatorList.RemoveAt(operatorList.Count - 1);
                }

                ExpressionString += operand3.Substring(0, operand3.Length - 1); ;


                return CalculateParenthesesExpressionEx(ExpressionString);

            }


            // 第二步:把转换成后序表达的式子计算
            //23|56|102|100|-|/|*|36|24|-|8|6|-|/|*|+"
            private string CalculateParenthesesExpressionEx(string Expression)
            {
                //定义两个栈
                ArrayList operandList = new ArrayList();
                float operand1;
                float operand2;
                string[] operand3;

                Expression = Expression.Replace(" ", "");
                operand3 = Expression.Split(Convert.ToChar("|"));
                try
                {
                    for (int i = 0; i < operand3.Length; i++)
                    {
                        if (Char.IsNumber(operand3[i], 0))
                        {
                            operandList.Add(operand3[i].ToString());
                        }
                        else
                        {
                            //两个操作数退栈和一个操作符退栈计算
                            operand2 = (float)Convert.ToDouble(operandList[operandList.Count - 1]);
                            operandList.RemoveAt(operandList.Count - 1);
                            operand1 = (float)Convert.ToDouble(operandList[operandList.Count - 1]);
                            operandList.RemoveAt(operandList.Count - 1);
                            operandList.Add(calculate(operand1, operand2, operand3[i]).ToString());
                        }

                    }


                    return operandList[0].ToString();
                }
                catch (Exception ex)
                {
                    return "0";
                }
            }



            //判断两个运算符优先级别
            private bool verifyOperatorPriority(string Operator1, string Operator2)
            {

                if (Operator1 == "*" && Operator2 == "+")
                    return true;
                else if (Operator1 == "*" && Operator2 == "-")
                    return true;
                else if (Operator1 == "/" && Operator2 == "+")
                    return true;
                else if (Operator1 == "/" && Operator2 == "-")
                    return true;
                else
                    return false;
            }

            //计算
            private double calculate(double operand1, double operand2, string operator2)
            {
                switch (operator2)
                {
                    case "*":
                        operand1 *= operand2;
                        break;
                    case "/":
                        if (operand2 == 0)
                            throw new Exception();
                        operand1 /= operand2;

                        break;
                    case "+":
                        operand1 += operand2;
                        break;
                    case "-":
                        operand1 -= operand2;
                        break;
                    default:
                        break;
                }
                return operand1;
            



        }
    }
}
