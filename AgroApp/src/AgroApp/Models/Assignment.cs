using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Assignment
    {
        public Customer Customer;
        public int IdAssignment;
        public User[] Users;
        public string Location;
        public string Description;
        public DateTime? Date;

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
