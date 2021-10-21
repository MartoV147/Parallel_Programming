using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Programming_Hw_2
{
    public class User
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public User(string name, string username, string password) 
        {
            Name = name;
            Username = username;
            Password = password;
        }
    }
}
