using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleApp4
{
    public static class ThreadExtension
    {
        public static void WaitAll(this IEnumerable<Thread> threads)
        {
            if (threads != null)
            {
                foreach (Thread thread in threads)
                { thread.Join(); }
            }
        }
    }

    class Array
    {
        public int[,] Arr { get; set; }
        private int Count4 { get; set; }
        private static Random Rand { get; set; }
        private static object Locker { get; set; }

        static Array()
        {
            Rand = new Random(Guid.NewGuid().GetHashCode());
            Locker = new object();
        }

        public Array()
        {
            Arr = new int[Rand.Next(2, 20), Rand.Next(2, 20)];
            GenerateArray();
        }

        public Array(int n, int m)
        {
            Arr = new int[n, m];
            GenerateArray();
        }

        private void GenerateArray()
        {
            for (int i = 0; i < Arr.GetLength(0); i++)
            {
                for (int j = 0; j < Arr.GetLength(1); j++)
                {
                    Arr[i, j] = Rand.Next(0, 4);
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < Arr.GetLength(0); i++)
            {
                for (int j = 0; j < Arr.GetLength(1); j++)
                {
                    Console.Write($"{Arr[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        public int Calculate4()
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < Arr.GetLength(0); i++)
            {
                Thread thread = new Thread(ThreadCalculate);
                thread.Start(i);

                threads.Add(thread);
            }

            threads.WaitAll();

            return Count4;
        }

        public void ThreadCalculate(object index)
        {
            var i = (int)index;

            for (int j = 0; j < Arr.GetLength(1); j++)
            {
                if (i + 1 < Arr.GetLength(0) && j + 1 < Arr.GetLength(1))
                {
                    var tmp = new List<int>();

                    tmp.Add(Arr[i, j]);
                    tmp.Add(Arr[i + 1, j]);
                    tmp.Add(Arr[i, j + 1]);
                    tmp.Add(Arr[i + 1, j + 1]);

                    tmp = tmp.Distinct().ToList();

                    if (tmp.Count == 4)
                    {
                        lock (Locker)
                        {
                            Count4++;
                        }
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var obj = new Array();
            obj.Print();

            Console.WriteLine();
            Console.WriteLine(obj.Calculate4());

            Console.ReadKey();
        }
    }
}
