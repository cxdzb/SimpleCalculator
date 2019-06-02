using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace SimpleCalculator
{
    public partial class MainWindow : Window
    {
        Infix infix = new Infix();  //新建中缀表达式和计算器
        Calculation calculation = new Calculation();
        DispatcherTimer timer;  //设置时钟，用于刷新时间
        Dictionary<string, double> Record = new Dictionary<string, double>();   //新建记录
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();  //设定时钟，0.1秒调用一次Get_Time
            timer.Tick += new EventHandler(Get_Time);
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Start();  //开启时钟

            Binding binding1 = new Binding
            {
                Source = infix,
                Path = new PropertyPath("Formula")
            };  //绑定数据，将三个输入框分别绑定计算式、输入和时间
            formula.SetBinding(TextBox.TextProperty, binding1);
            Binding binding2 = new Binding
            {
                Source = infix,
                Path = new PropertyPath("Input")
            };
            input.SetBinding(TextBox.TextProperty, binding2);
            Binding binding3 = new Binding
            {
                Source = infix,
                Path = new PropertyPath("Current")
            };
            time.SetBinding(TextBox.TextProperty, binding3);
        }

        private void Get_Time(object sender, EventArgs e)   //获取时间
        {
            infix.Current = System.DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
        }
        //数字键
        private void Button_0_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "0";
        }
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "1";
        }
        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "2";
        }
        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "3";
        }
        private void Button_4_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "4";
        }
        private void Button_5_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "5";
        }
        private void Button_6_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "6";
        }
        private void Button_7_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "7";
        }
        private void Button_8_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "8";
        }
        private void Button_9_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "9";
        }
        private void Button_pi_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "π";
        }
        private void Button_e_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += "e";
        }
        //括号和点
        private void Button_left_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula += "(";
            infix.Input = "";
        }
        private void Button_right_Click(object sender, RoutedEventArgs e)
        {
            if (infix.Input == "")
            {
                if (infix.Formula.EndsWith(")"))
                    infix.Input = "";
                else
                    infix.Input = "0";
            }
            infix.Formula += infix.Input+")";
            infix.Input = "";
        }
        private void Button_point_Click(object sender, RoutedEventArgs e)
        {
            infix.Input += ".";
        }
        //二元运算符
        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula += infix.Input + "+";
            infix.Input = "";
        }
        private void Button_sub_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula += infix.Input + "-";
            infix.Input = "";
        }
        private void Button_mul_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula += infix.Input + "×";
            infix.Input = "";
        }
        private void Button_div_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula += infix.Input + "÷";
            infix.Input = "";
        }
        private void Button_mod_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula += infix.Input + "%";
            infix.Input = "";
        }
        private void Button_pow_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula += infix.Input + "^";
            infix.Input = "";
        }
        //一元运算符
        private void Button_sin_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("sin");
        }
        private void Button_cos_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("cos");
        }
        private void Button_tan_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("tan");
        }
        private void Button_factorial_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("fact");
        }
        private void Button_log_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("log");
        }
        private void Button_SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("sqrt");
        }
        private void Button_arcsin_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("sin⁻¹");
        }
        private void Button_arccos_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("cos⁻¹");
        }
        private void Button_arctan_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("tan⁻¹");
        }
        private void Button_ln_Click(object sender, RoutedEventArgs e)
        {
            Special_Operater("ln");
        }
        private void Button_neg_Click(object sender, RoutedEventArgs e)
        {
            if (infix.Input.Contains("-"))
                infix.Input = infix.Input.Substring(2, infix.Input.Length - 3);
            else
                infix.Input = "(-" + infix.Input + ")";
        }
        //清除键
        private void Button_CE_Click(object sender, RoutedEventArgs e)
        {
            infix.Input = "";
        }
        private void Button_C_Click(object sender, RoutedEventArgs e)
        {
            infix.Formula = "";
            infix.Input = "";
        }
        private void Button_del_Click(object sender, RoutedEventArgs e)
        {
            if(infix.Input.Length>0)
                infix.Input = infix.Input.Remove(infix.Input.Length - 1);
        }

        private void Special_Operater(string op)
        {
            string t = infix.Formula;
            if (t.EndsWith(")"))
            {
                int i = 0, num = 1;
                for (i = t.LastIndexOf(")") - 1; ; i--)
                {
                    if (t.ToCharArray()[i] == '(')
                        num--;
                    else if (t.ToCharArray()[i] == ')')
                        num++;
                    if (num == 0)
                        break;
                }
                infix.Formula = t.Insert(i, op);
            }
            else
            {
                infix.Formula += op + "(" + infix.Input + ")";
            }
            infix.Input = "";
        }
        //求值
        private void Button_equal_Click(object sender, RoutedEventArgs ex)
        {
            if(!(infix.Formula=="" && infix.Input==""))
                try
                {
                    if (infix.Input == ""|| Regex.Matches(infix.Formula.Substring(infix.Formula.Length-1), @"(\d|e|\)|π)").Count==1)
                        infix.Input = calculation.Get_Result(infix.Formula);
                    else
                    {
                        infix.Formula += infix.Input;
                        infix.Input = calculation.Get_Result(infix.Formula);
                    }
                    if (1 - (double.Parse(infix.Input) % 1) <= 1e-10)   //控制精度
                        infix.Input = ((int)(double.Parse(infix.Input) + 1)).ToString();
                    else if(double.Parse(infix.Input) % 1 <= 1e-10)
                        infix.Input = ((int)double.Parse(infix.Input)).ToString();

                    if(!Record.ContainsKey(infix.Formula))  //添加到记录
                        Record.Add(infix.Formula, double.Parse(infix.Input));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
        }
        //历史
        private void Record_DropDown(object sender, EventArgs e)
        {
            ComboBox x = sender as ComboBox;
            x.Items.Clear();
            foreach (KeyValuePair<string,double> r in Record)  //添加端口
                x.Items.Add(r.Key+"="+r.Value);
        }
    }
}
