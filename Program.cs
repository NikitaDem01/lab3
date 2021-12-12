using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace lab3
{
    class MyThread
    {
        public Thread Thrd;
        ManualResetEvent mre;
        Person per;

        public MyThread(string name, ManualResetEvent evt, Person pers)
        {
            Thrd = new Thread(Run);
            Thrd.Name = name;
            mre = evt;
            per = pers;
            Thrd.Start();
        }

        void Run()
        {
            Console.WriteLine("Внутри потока " + Thrd.Name);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(per.Name + " " + per.age);
                per.age++;
                Thread.Sleep(500);
            }

            Console.WriteLine(Thrd.Name + " завершен!");

            mre.Set();
        }
    }

    class Person
    {
        public string Name { get; set; }
        public int age { get; set; }
    }
    class Program
    {
        static int x = 0, b = 1, a = 0, potoks = 15, odd = 0, even = 0, obsh = 0, threads = 5;
        static object locker = new object();
        static Mutex mutexObj = new Mutex();
        static Mutex mutexObj2 = new Mutex();
        static string s = "";
        public static bool vivedeno = true;


        static void Main(string[] args)
        {

            //Count0();
            //Count3();
            Count2();
            Console.ReadLine();
        }

        private static void Count2()
        {
            int
            obsh = 50;
            for (int i = 0; i < obsh; i++)
            {
                while (threads < 0)
                {

                }
                Thread myThread = new Thread(new ParameterizedThreadStart(Runmutex));
                myThread.Start(i);
                threads--;
            }

        }

        private static void Runmutex(object x)
        {
            mutexObj2.WaitOne();
            int n = (int)x;
            if ((obsh - even - odd) % 2 == 0)
            {
                odd++;
                threads++;
                mutexObj2.ReleaseMutex();
                Console.WriteLine(odd);
            }
            else
            {
                even++;
                Thread.Sleep(500);
                threads++;
                mutexObj2.ReleaseMutex();
                Console.WriteLine(even);
            }
            Console.WriteLine("Complete");
        }
        private static void Count0()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread myThread = new Thread(Count);
                myThread.Name = "Поток " + i.ToString();
                myThread.Start();
            }
            Console.ReadLine();
        }

        public static void Count()
        {
            lock (locker)
            {
                x = 1;
                for (int i = 1; i < 9; i++)
                {
                    Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, x);
                    x++;
                    Thread.Sleep(100);
                }
            }
        }
        public static void Count3()
        {
            var oleg = new Person() { Name = "Oleg", age = 15 };
            ManualResetEvent evtObj = new ManualResetEvent(false);

            MyThread mt1 = new MyThread("Событийный поток 1", evtObj, oleg);

            Console.WriteLine("Основной поток ожидает событие");

            evtObj.WaitOne();

            Console.WriteLine("Основной поток получил уведомление о событии от первого потока");

            evtObj.Reset();

            mt1 = new MyThread("Событийный поток 2", evtObj, oleg);

            evtObj.WaitOne();

            Console.WriteLine("Основной поток получил уведомление о событии от второго потока");
            Console.ReadLine();
        }
    }
}