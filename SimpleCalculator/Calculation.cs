using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Numerics;

namespace SimpleCalculator
{
    class Calculation
    {
        Dictionary<string, int> priority = new Dictionary<string, int>();
        List<string> special_operators = new List<string>
        {
            "sin","cos","tan","log","ln","fact","sqrt","sin⁻¹","cos⁻¹","tan⁻¹"
        };
        string result = "0";
        public Calculation()
        {
            priority.Add("(", 0);
            priority.Add(")", 0);
            priority.Add("+", 1);
            priority.Add("-", 1);
            priority.Add("×", 2);
            priority.Add("÷", 2);
            priority.Add("%", 2);
            priority.Add("^", 3);
        }

        public string Get_Result(string infix)
        {
            result = Calculate(Infix_To_Postfix(String_To_List(infix)));
            return result;
        }

        public List<string> String_To_List(string input)
        {
            List<string> infix = new List<string>();
            string pattern = @"\d+(\.\d+)?|[()]|[+-]|[×÷%]|[a-z⁻¹]+|\^|π";
            foreach (Match match in Regex.Matches(input, pattern))
                infix.Add(match.Value);
            for(int i=1;i<infix.Count-1;i++)    //找出负数
                if(infix[i-1]=="("&&infix[i]=="-"&&(infix[i+1]=="e"|| infix[i + 1] == "π"||IsNumeric(infix[i + 1])))    //当出现(-1这种形式后，-和数字结合
                {
                    infix[i + 1] = infix[i] + infix[i + 1];
                    infix.RemoveAt(i++);
                }
            return infix;
        }
        public List<string> Infix_To_Postfix(List<string> infix)
        {
            List<string> postfix = new List<string>();
            Stack<string> operators = new Stack<string>();
            for (int i = 0; i < infix.Count; i++)
            {
                if (IsNumeric(infix[i]) || infix[i] == "π" || infix[i]=="e" || infix[i] == "-π" || infix[i] == "-e")
                {//如果是数字，则直接加入后缀表达式
                    postfix.Add(infix[i]);
                }
                else if (special_operators.Contains(infix[i]))
                {
                    operators.Push(infix[i]);
                }
                else if (infix[i] == ")")
                {//如果是)，弹出栈()内的运算符到后缀表达式
                    while (operators.Peek() != "(")
                        postfix.Add(operators.Pop());
                    operators.Pop();
                    if (operators.Count != 0 && special_operators.Contains(operators.Peek()))
                        postfix.Add(operators.Pop());
                }
                else if (infix[i] == "(")
                {//如果是(，则直接压入栈
                    operators.Push(infix[i]);
                }
                else
                {//如果是运算符，优先级高于栈顶或栈空则压入栈，否则栈顶弹出加入后缀表达式
                    if (operators.Count == 0 || priority[infix[i]] > priority[operators.Peek()])
                        operators.Push(infix[i]);
                    else
                    {
                        while (operators.Count != 0 && priority[infix[i]] <= priority[operators.Peek()])
                            postfix.Add(operators.Pop());
                        operators.Push(infix[i]);
                    }
                }
            }
            while (operators.Count != 0)//将栈中剩余运算符加入后缀表达式
                postfix.Add(operators.Pop());
            return postfix;
        }
        public string Calculate(List<string> postfix)
        {
            Stack<string> result = new Stack<string>();
            for (int i = 0; i < postfix.Count; i++)
            {
                if (IsNumeric(postfix[i]))
                {//数字
                    result.Push(postfix[i]);
                }
                else if (postfix[i] == "π")
                {
                    result.Push(Pi());
                }
                else if (postfix[i] == "e")
                {
                    result.Push(E());
                }
                else if (postfix[i] == "-π")
                {
                    result.Push("-" + Pi());
                }
                else if (postfix[i] == "-e")
                {
                    result.Push("-" + E());
                }
                else if (special_operators.Contains(postfix[i]))
                {//特殊运算，直接运算前一个数字
                    string num = result.Pop();
                    if (postfix[i] == "fact")
                        result.Push(Fact(num));
                    else if (postfix[i] == "sqrt")
                        result.Push(Sqrt(num));
                    else if (postfix[i] == "log")
                        result.Push(Log(num));
                    else if (postfix[i] == "ln")
                        result.Push(Ln(num));
                    else if (postfix[i] == "sin")
                        result.Push(Sin(num));
                    else if (postfix[i] == "cos")
                        result.Push(Cos(num));
                    else if (postfix[i] == "tan")
                        result.Push(Tan(num));
                    else if (postfix[i] == "sin⁻¹")
                        result.Push(Arcsin(num));
                    else if (postfix[i] == "cos⁻¹")
                        result.Push(Arccos(num));
                    else if (postfix[i] == "tan⁻¹")
                        result.Push(Arctan(num));
                }
                else
                {//普通运算，直接操作前两个数
                    string num2 = result.Pop();
                    string num1 = result.Pop();
                    int flag = 0;
                    if ((!num1.Contains(".") && num1.Length >= 100) || (!num2.Contains(".") && num2.Length >= 100))
                        flag = 1;
                    if (postfix[i] == "+")
                        result.Push(Add(num1, num2, flag));
                    else if (postfix[i] == "-")
                        result.Push(Sub(num1, num2, flag));
                    else if (postfix[i] == "×")
                        result.Push(Mul(num1, num2, flag));
                    else if (postfix[i] == "÷")
                        result.Push(Div(num1, num2, flag));
                    else if (postfix[i] == "^") {
                        if(!num1.Contains(".")&& !num2.Contains("."))
                            flag = 1;
                        result.Push(Pow(num1, num2, flag));
                    }
                    else if (postfix[i] == "%")
                        result.Push(Mod(num1, num2, flag));
                }
            }
            return result.Pop();
        }

        public bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^(\-|\+)?\d+(\.\d+)?$");
        }
        public string Pi()
        {
            return Math.PI.ToString();
        }
        public string E()
        {
            return Math.E.ToString();
        }
        public string Add(string num1, string num2,int flag=0)
        {
            if(flag==1)
                return (BigInteger.Parse(num1) + BigInteger.Parse(num2)).ToString();
            return (double.Parse(num1) + double.Parse(num2)).ToString();
        }
        public string Sub(string num1, string num2, int flag = 0)
        {
            if (flag == 1)
                return (BigInteger.Parse(num1) - BigInteger.Parse(num2)).ToString();
            return (double.Parse(num1) - double.Parse(num2)).ToString();
        }
        public string Mul(string num1, string num2, int flag = 0)
        {
            if (flag == 1)
                return (BigInteger.Parse(num1) * BigInteger.Parse(num2)).ToString();
            return (double.Parse(num1) * double.Parse(num2)).ToString();
        }
        public string Div(string num1, string num2, int flag = 0)
        {
            if (double.Parse(num2) == 0.0)
                throw new Exception("被除数不能为0！");
            if (flag == 1)
                return (BigInteger.Parse(num1) / BigInteger.Parse(num2)).ToString();
            return (double.Parse(num1) / double.Parse(num2)).ToString();
        }
        public string Mod(string num1, string num2, int flag = 0)
        {
            if (flag == 1)
                return (BigInteger.Parse(num1) % BigInteger.Parse(num2)).ToString();
            return (double.Parse(num1) % double.Parse(num2)).ToString();
        }
        public string Pow(string num1, string num2, int flag = 0)
        {
            if (flag == 1)
                return BigInteger.Pow(BigInteger.Parse(num1), int.Parse(num2)).ToString();
            return Math.Pow(double.Parse(num1), double.Parse(num2)).ToString();
        }
        public string Sqrt(string num, int flag = 0)
        {
            if (double.Parse(num) < 0)
                throw new Exception("二次根号底数不能为负！");
            return Pow(num, "0.5",flag);
        }
        public string Fact(string num)
        {
            BigInteger n = BigInteger.Parse(num);
            if (n < 0)
                throw new Exception("阶乘底数不能为负！");
            for (int i = 1; i < int.Parse(num); i++)
                n *= i;
            return n.ToString();
        }
        public string Log(string num)
        {
            if (double.Parse(num) <= 0.0)
                throw new Exception("对数底数不能为非正数！");
            return Math.Log10(double.Parse(num)).ToString();
        }
        public string Ln(string num)
        {
            if (double.Parse(num) <= 0.0)
                throw new Exception("对数底数不能为非正数！");
            return Math.Log(double.Parse(num)).ToString();
        }
        public string Sin(string num)
        {
            return Math.Sin(double.Parse(num)).ToString();
        }
        public string Cos(string num)
        {
            return Math.Cos(double.Parse(num)).ToString();
        }
        public string Tan(string num)
        {
            if (double.Parse(num) % (Math.PI)>= 1e-10 && double.Parse(num) % (Math.PI / 2) <= 1e-10)
                throw new Exception("正弦函数参数不能为π/2+kπ！");
            return Math.Tan(double.Parse(num)).ToString();
        }
        public string Arcsin(string num)
        {
            if (double.Parse(num) <= 1|| double.Parse(num) >1)
                throw new Exception("反正弦函数参数绝对值不能大于1！");
            return Math.Asin(double.Parse(num)).ToString();
        }
        public string Arccos(string num)
        {
            if (double.Parse(num) <= 1 || double.Parse(num) > 1)
                throw new Exception("反余弦函数参数绝对值不能大于1！");
            return Math.Acos(double.Parse(num)).ToString();
        }
        public string Arctan(string num)
        {
            return Math.Atan(double.Parse(num)).ToString();
        }
    }
}
