using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Programming_BarSimulator
{
    class Drink
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public Drink(string name, double price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}
