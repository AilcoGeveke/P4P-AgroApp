using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class User
    {
        public string Name { private set; get; }
        public string Email { private set; get; }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
