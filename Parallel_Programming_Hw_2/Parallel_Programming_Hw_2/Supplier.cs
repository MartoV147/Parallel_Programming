using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Programming_Hw_2
{
    public class Supplier : User
    {
        public string CampanyName { get; set; }

        public Supplier(string name, string username, string password, string campanyName)
            : base(name, username, password)
        {
            CampanyName = campanyName;
        }
    }
}
