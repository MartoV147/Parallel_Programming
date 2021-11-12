using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Parallel_Programming_BarSimulator
{
    class Bar
    {
        public List<Visitor> visitors = new List<Visitor>();
        public List<Drink> drinks = new List<Drink>();
        public double TotalBudget = 0;
        public bool isOpen = true;

        Semaphore semaphore = new Semaphore(10, 10);
        Random rand = new Random();

        public bool Enter(Visitor visitor)
        {
            if (rand.Next(1, 6) <= 2)
            {
                Close();
            }

            semaphore.WaitOne();
            lock (visitors)
            {
                if (isOpen)
                {
                    if (visitor.Age < 18)
                    {
                        Console.WriteLine($"{visitor.Name} isn't old enough!");
                        return false;
                    }
                    else 
                    {
                        visitors.Add(visitor);
                        return true;
                    } 
                }
                else
                {
                    Console.WriteLine("Bar ain't open!");
                    return false;       
                }
            }
        }

        public void Leave(Visitor visitor)
        {
            lock (visitors)
            {
                visitors.Remove(visitor);
            }
            semaphore.Release();
        }

        public void GetDrink(Visitor visitor, Drink drink) 
        {
            if (visitor.Budget >= drink.Price && drink.Quantity >= 1)
            {
                Console.WriteLine($"{visitor.Name} just ordered {drink.Name}");
                drink.Quantity -= 1;
                visitor.Budget -= drink.Price;
                TotalBudget += drink.Price;
                return;
            }
            else if (visitor.Budget < drink.Price)
            {
                Console.WriteLine($"{visitor.Name} doesn't have enough money to buy {drink.Name}");
                return;
            }
            else if (drink.Quantity < 1)
            {
                Console.WriteLine($"{drink.Name} is out of stock");
                return;
            }
        }

        public void Report() 
        {
            foreach (var item in drinks)
            {
                if (item.Quantity == 0)
                {
                    Console.WriteLine($"{ item.Name} is out of stock");
                }
            }
            Console.WriteLine($"Total profit today is {TotalBudget}");
        }

        public void Close()
        {
            lock (visitors)
            {
                foreach (var item in visitors)
                {
                    Console.WriteLine($"{item.Name} is kicked out of the bar.");
                    item.WalkOut();
                    semaphore.Release();
                }
                isOpen = false;
                visitors.Clear();
            }
            Console.WriteLine("Bar is Closed");
            Report();
        }
    }
}