using System;
using System.Threading;

namespace Lab3
{
    public class Ex1
    {
        public static void Start()
        {
            var wh1 = new AutoResetEvent(false);
            var wh2 = new AutoResetEvent(false);
            var wh3 = new AutoResetEvent(false);

            var x1 = 1;
            var x2 = 2;
            var x3 = 3;
            var x4 = 4;
            var x5 = 5;
            var x6 = 6;

            var mult = 0;
            var sum = 0;

            // x1 * x2 + x3 + x4 * x5 + x6

            var T0 = new Thread(() =>
            {
                mult = x1 * x2;
                wh1.Set();
            });

            var T1 = new Thread(() =>
            {
                wh1.WaitOne();
                sum = mult + x3;
                wh2.Set();
            });

            var T2 = new Thread(() =>
            {
                wh2.WaitOne();
                mult = x4 * x5;
                wh3.Set();
            });

            var T3 = new Thread(() =>
            {
                wh3.WaitOne();
                Console.WriteLine($"Result: {sum + mult + x6}");
            });

            T0.Start();
            T1.Start();
            T2.Start();
            T3.Start();
            T3.Join();
        }
    }
}
