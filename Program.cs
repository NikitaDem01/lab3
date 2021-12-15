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
        Animal ani;

        public MyThread(string name, ManualResetEvent evt, Animal anim)
        {
            Thrd = new Thread(Run);
            Thrd.Name = name;
            mre = evt;
            ani = anim;
            Thrd.Start();
        }

        void Run()
        {
            Console.WriteLine("Внутри потока " + Thrd.Name);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(ani.Name + " " + ani.age);
                ani.age++;
                Thread.Sleep(500);
            }

            Console.WriteLine(Thrd.Name + " завершен!");

            mre.Set();
        }
    }

    class Animal
    {
        public string Name { get; set; }
        public int age { get; set; }
    }

    class SharedRes
    {

        public static int Count = 0;

        public static Mutex Mtx = new Mutex();

    }

    // В этом потоке переменная SharedRes.Count инкрементируется,

    class IncThread
    {

        int num;

        public Thread Thrd;

        public IncThread(string name, int n)
        {

            Thrd = new Thread(Run);

            num = n;

            Thrd.Name = name;

            Thrd.Start();

        }

        // Точка входа в поток,

        void Run()
        {

            Console.WriteLine(Thrd.Name + " ожидает мьютекс.");

            // Получить мьютекс.

            SharedRes.Mtx.WaitOne();

            Console.WriteLine(Thrd.Name + " получает мьютекс.");

            do
            {

                Thread.Sleep(500);

                SharedRes.Count++;

                Console.WriteLine("В потоке " + Thrd.Name + ", SharedRes.Count = " + SharedRes.Count);

                num--;

            } while (num > 0);

            Console.WriteLine(Thrd.Name + " освобождает мьютекс.");

            // Освободить мьютекс.

            SharedRes.Mtx.ReleaseMutex();

        }

    }

    // В этом потоке переменная SharedRes.Count декрементируется,

    class DecThread
    {

        int num;

        public Thread Thrd;

        public DecThread(string name, int n)
        {

            Thrd = new Thread(new ThreadStart(this.Run));

            num = n;

            Thrd.Name = name;

            Thrd.Start();

        }

        // Точка входа в поток,

        void Run()
        {

            Console.WriteLine(Thrd.Name + " ожидает мьютекс.");

            // Получить мьютекс.

            SharedRes.Mtx.WaitOne();

            Console.WriteLine(Thrd.Name + " получает мьютекс.");

            do
            {

                Thread.Sleep(500);

                SharedRes.Count--;

                Console.WriteLine("В потоке " + Thrd.Name + ", SharedRes.Count = " + SharedRes.Count);

                num--;

            } while (num > 0);

            Console.WriteLine(Thrd.Name + " освобождает мьютекс.");

            // Освободить мьютекс.

            SharedRes.Mtx.ReleaseMutex();

        }
    }
    class Program
    {
        static int x = 0;
        static object locker = new object();
        public static bool vivedeno = true;


        static void Main(string[] args)
        {
            //Count0();
            Count2();
            //Count3();
            Console.ReadLine();
        }

        private static void Count2()
        {
            IncThread mt1 = new IncThread("Инкрементирующий Поток", 5);
            Thread.Sleep(1); // разрешить инкрементирующему потоку начаться
            DecThread mt2 = new DecThread("Декрементирующий Поток", 5);
            mt1.Thrd.Join();
            mt2.Thrd.Join();
            Console.ReadLine();
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
            var tiger = new Animal() { Name = "Tiger", age = 15 };
            ManualResetEvent evtObj = new ManualResetEvent(false);

            MyThread mt1 = new MyThread("Событийный поток 1", evtObj, tiger);

            Console.WriteLine("Основной поток ожидает событие");

            evtObj.WaitOne();

            Console.WriteLine("Основной поток получил уведомление о событии от первого потока");

            evtObj.Reset();

            mt1 = new MyThread("Событийный поток 2", evtObj, tiger);

            evtObj.WaitOne();

            Console.WriteLine("Основной поток получил уведомление о событии от второго потока");
            Console.ReadLine();
        }
    }
}