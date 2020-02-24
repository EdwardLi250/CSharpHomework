using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            double inputNum;
            Console.Write("输入一个正整数:");
            input = Console.ReadLine();
            bool flag = double.TryParse(input, out inputNum);
            if (flag == false)
            {
                Console.WriteLine("输入不是正整数！");
                Console.ReadKey();
                return;
            }
            for(int i = 2; i <= inputNum; i++)
            {
                if (inputNum % i != 0)
                    continue;
                else
                    while (inputNum % i == 0)
                    {
                        inputNum /= i;
                        Console.Write(i + ", ");                 
                   
                    }
            }
            Console.ReadKey();
            return;
        }
    }
}
