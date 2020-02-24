using System;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = new int[101];
            for(int i = 2; i <= 10; i++)
            {
                for(int j = 2; i * j <= 100; j++)
                {
                    a[i * j] = 1;
                }
            }
            for(int i = 0; i <= 100; i++) {
                if (a[i] == 0)
                {
                    Console.Write(i + ", ");
                }                 
            }
            Console.ReadLine();
        }
    }
}
