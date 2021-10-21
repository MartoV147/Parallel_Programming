using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
namespace Parallel_Programming_Hw_2
{
    class Program
    {
        public static bool Test() 
        {
            Menager menager;

            Dictionary<Product, int> Stock = new Dictionary<Product, int>();

            List<Client> Clients = new List<Client>();
            List<Supplier> Suppliers = new List<Supplier>();

            List<Thread> Threads = new List<Thread>();

            Random rand = new Random();

            for (int i = 1; i <= 20; i++)
            {
                Stock.Add(new Product("Barrel " + i, "b", 10.0), rand.Next(1, 15));
            }

            for (int i = 1; i <= 100; i++)
            {
                Clients.Add(new Client("Gosho " + i, "g", "1", "g", "g"));
            }

            for (int i = 1; i <= 5; i++)
            {
                Suppliers.Add(new Supplier("Petar " + i, "p", "p", "p"));
            }

            menager = new Menager(Stock);

            foreach (var c in Clients)
            {
                int numberOfProducts = rand.Next(0, Stock.Count);

                Product p = null;

                Dictionary<Product, int> dictionary = new Dictionary<Product, int>();

                for (int i = 0; i < numberOfProducts - 1; i++)
                {
                    p = Stock.ToList()[rand.Next(0, Stock.Count - 1)].Key;

                    if (dictionary.ContainsKey(p))
                    {
                        dictionary[p] += 1;
                    }
                    else
                    {
                        dictionary.Add(p, rand.Next(1, 10));
                    }
                }

                Thread t = new Thread(() => menager.Purchase(c, dictionary));
                Threads.Add(t);
                t.Start();
                
            }


            foreach (var s in Suppliers)
            {
                int numberOfProducts = rand.Next(0, Stock.Count);

                Product p = null;

                Dictionary<Product, int> dictionary = new Dictionary<Product, int>();

                for (int i = 0; i < numberOfProducts - 1; i++)
                {
                    p = Stock.ToList()[rand.Next(0, Stock.Count - 1)].Key;

                    if (dictionary.ContainsKey(p))
                    {
                        dictionary[p] += 1;
                    }
                    else
                    {
                        dictionary.Add(p, rand.Next(1, 10));
                    }
                }

                Thread t = new Thread(() => menager.Supply(dictionary));
                Threads.Add(t);
                t.Start();
                
            }

            foreach (var item in Threads)
            {
                item.Join();
            }

            return true;
        }


        static void Main(string[] args)
        {
            bool result = Test();


            if (result)
            {
                Console.WriteLine("All done");
            }
            else Console.WriteLine("oops");  
        }
    }
}
