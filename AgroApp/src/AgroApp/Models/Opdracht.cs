using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Opdracht
    {
        public Customer Customer;
        public User[] Users;
        public string Location;
        public string Description;
        public DateTime? Date;
        public int Idassignment;

        public int usercount;

        public Opdracht(int idAssignment, string location, string description, DateTime? date, Customer selectedCustomer = null)
        {
            Idassignment = idAssignment;
            Location = location;
            Description = description;
            Date = date;
            Customer = selectedCustomer;
        }
    }
}
