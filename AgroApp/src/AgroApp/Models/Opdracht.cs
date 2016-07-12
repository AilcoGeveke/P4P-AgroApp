using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Opdracht
    {
        public Customer Customer { private set; get; }
        public User[] Users { private set; get; }
        public string Location { private set; get; }
        public string Description { private set; get; }
        public DateTime? Date { private set; get; }
        public int Idassignment { private set; get; }

        public int Usercount { private set; get; }

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
