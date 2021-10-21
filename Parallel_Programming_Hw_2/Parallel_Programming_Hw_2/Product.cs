using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Programming_Hw_2
{
    public class Product
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double UnitPrice { get; set; }

        public Product(string name, string description, double unitPrice) 
        {
            Name = name;
            Description = description;
            UnitPrice = unitPrice;
        }
    }
}
