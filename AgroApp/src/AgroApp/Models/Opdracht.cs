using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Opdracht
    {
        public Customer customer;
        public User[] users;
        public string location;
        public string description;
        public DateTime? date;
        public int idassignment;

        public int usercount;

        public Opdracht(int idAssignment, string Location, string Description, DateTime? Date, Customer selectedCustomer = null)
        {
            this.idassignment = idAssignment;
            this.location = Location;
            this.description = Description;
            this.date = Date;
            this.customer = selectedCustomer;
        }
    }
}
