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
        public string Adress { private set; get; }

        public Customer(int IdCustomer = -1, string Name = "", string Adress = "")
        {
            this.IdCustomer = IdCustomer;
            this.Name = Name;
            this.Adress = Adress;
        }
    }
}