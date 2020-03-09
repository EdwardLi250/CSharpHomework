using System;
using System.Threading;

namespace ConsoleApp10
{
    public delegate void ClockHandler();
    public class Clock
    {
        public event ClockHandler OnTick;
        public event ClockHandler OnAlarm;
        public string set_time;
        public void Start()
        {
            bool flag = true;
            while (flag)
            {
                Thread.Sleep(1000);
                OnTick();
                if(DateTime.Now.ToString() == set_time)
                {
                    OnAlarm();
                    flag = false;
                }
            }
        }
    }

    public class User
    {
        public Clock Clock1 = new Clock();
        public User(string alarm_time)
        {
            Clock1.set_time = alarm_time;
            Clock1.OnTick += Tick;
            Clock1.OnAlarm += Alarm;
            static void Tick()
            {
                Console.WriteLine("Tick:" + DateTime.Now.ToString());
            }
            static void Alarm()
            {
                Console.WriteLine("Alarm!");
            }
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            string alarm_time = "";
            Console.WriteLine("请输入闹铃时间（格式：dd/mm/yyyy hour:min:sec）：");
            alarm_time = Console.ReadLine();
            User user1 = new User(alarm_time);
            user1.Clock1.Start();
        }
    }
}
