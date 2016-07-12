using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Customer
    {
        public int IdCustomer { private set; get; }
        public string Name { private set; get; }
        public string Address { private set; get; }

        public Customer(int idCustomer = -1, string name = "", string address = "")
        {
            IdCustomer = idCustomer;
            Name = name;
            Address = address;
        }
    }
}