using System;

namespace Calculator
{
    class Calculator
    {
        public static double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN;//设置计算结果默认值为NaN，用于检测异常等

            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    break;
                case "s":
                    result = num1 - num2;
                    break;
                case "m":
                    result = num1 * num2;
                    break;
                case "d":
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    break;
                default: break;
            }
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool end = false;//true时程序结束
            while (!end)
            {
                string numInput1 = "";
                string numInput2 = "";
                double result = 0;

                Console.Write("输入第一个操作数：");
                numInput1 = Console.ReadLine();

                double cleanNum1 = 0;
                while (!double.TryParse(numInput1, out cleanNum1))
                {
                    Console.Write("该输入不符合条件，请输入一个整数: ");
                    numInput1 = Console.ReadLine();
                }

                Console.Write("输入第二个操作数: ");
                numInput2 = Console.ReadLine();

                double cleanNum2 = 0;
                while (!double.TryParse(numInput2, out cleanNum2))
                {
                    Console.Write("该输入不符合条件，请输入一个整数: ");
                    numInput2 = Console.ReadLine();
                }

                Console.WriteLine("选择计算类型:  a - 加法  s - 减法  m - 乘法  d - 除法：");
                string op = Console.ReadLine();

                try
                {
                    result = Calculator.DoOperation(cleanNum1, cleanNum2, op);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("该计算有误，无法完成\n");
                    }
                    else Console.WriteLine("计算结果: {0:0.##}\n", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("计算出现异常\n - 异常种类: " + e.Message);
                }
                Console.WriteLine("\n");

                Console.Write("输入c再点击Enter键以结束程序，输入任何其他按键再点击Enter键以进行下一次运算: ");
                if (Console.ReadLine() == "c") end = true;

                Console.WriteLine("\n");
            }
            return;
        }
    }
}