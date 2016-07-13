using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Customer
    {
        public int IdCustomer { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }

        public Customer() { }

        public Customer(int idCustomer = -1, string name = "", string address = "")

        {
            IdCustomer = idCustomer;
            Name = name;
            Address = address;
           
        }
    }
}
