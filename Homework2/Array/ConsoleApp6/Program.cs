using System;

//2.编程求一个整数数组的最大值、最小值、平均值和所有数组元素的和
namespace ConsoleApp6
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.Write("请输入整数数组元素个数：");
            int n = Int32.Parse(Console.ReadLine());
            int[] input = new int[n];
            Console.WriteLine("请输入数组元素,以Enter键分隔：");
            for(int i = 0; i < n; i++)
            {
                input[i] = Int32.Parse(Console.ReadLine());
            }
            int max = Int32.MinValue, min = Int32.MaxValue;
            int sum = 0;
            int average = 0;
            for(int i = 0; i < n; i++)
            {
                if (input[i] > max)
                {
                    max = input[i];
                }
                if (input[i] < min)
                {
                    min = input[i];
                }
                sum += input[i];
                average = sum / n;
            }
            Console.WriteLine("最大值；" + max);
            Console.WriteLine("最小值：" + min);
            Console.WriteLine("平均值：" + average);
            Console.WriteLine("总和：" + sum);
        }
    }
}
