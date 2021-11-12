using System;
using System.Collections.Generic;
using System.Threading;

namespace Parallel_Programming_BarSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Bar bar = new Bar();
            for (int i = 1; i < 15; i++)
            {
                bar.drinks.Add(new Drink("drink " + i, i * 0.3, i + 2));
            }

            List<Thread> visitorThreads = new List<Thread>();
            for (int i = 1; i < 50; i++)
            {
                int age = GetRandomAge();
                double budget = GetRandomBudget();
                var visitor = new Visitor(i.ToString(), age, budget, bar);

                var thread = new Thread(visitor.PaintTheTownRed);
                thread.Start();
                visitorThreads.Add(thread);
            }

            foreach (var t in visitorThreads)
            { 
                t.Join();
            } 
            Console.WriteLine();
            Console.WriteLine("The party is over.");
            bar.Close();
            Console.ReadLine();
        }

        public static int GetRandomAge() 
        {
            Random random = new Random();
            return random.Next(1, 70);
        }

        public static double GetRandomBudget()
        {
            Random random = new Random();
            return random.NextDouble() * 100;
        }
    }
}
