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
        public string Adress { set; get; }

        public Customer(int idCustomer = -1, string name = "", string adress = "")
        {
            IdCustomer = idCustomer;
            Name = name;
            Adress = adress;
        }
    }
}