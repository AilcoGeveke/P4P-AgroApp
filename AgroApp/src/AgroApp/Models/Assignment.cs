using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Assignment
    {
        public Customer Customer { private set; get; }
        public int IdAssignment { private set; get; }
        public User[] Users { private set; get; }
        public string Location { private set; get; }
        public string Description { private set; get; }
        public DateTime? Date { private set; get; }

        public int gebruikerCount;

        public Assignment(int idAssignment, string location, string description, DateTime? date, Customer customer = null)
        {
            IdAssignment = idAssignment;
            Location = location;
            Description = description;
            Date = date;
            Customer = customer;
        }
    }
}
