using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Programming_Hw_2
{
    public class Client : User
    {
        public string Email { get; set; }

        public string ShippingAddress { get; set; }

        public Client(string name, string username, string password, string email, string shippingAddress) 
            : base(name, username, password) 
        {
            Email = email;
            ShippingAddress = shippingAddress;
        }
    }
}
