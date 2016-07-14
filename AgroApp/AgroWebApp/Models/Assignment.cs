using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Assignment
    {
        public int IdAssignment { set; get; }
        public string Location { set; get; }
        public string Description { set; get; }
        public long Date { set; get; }

        public Customer Customer { set; get; }
        public List<User> Employees { set; get; }
        public int EmployeeCount { set; get; }

        public Assignment() { }

        public Assignment(long date, int idAssignment = -1, string location = "", string description = "")

        {
            IdAssignment = idAssignment;
            Location = location;
            Description = description;
            Date = date;
        }
    }
}
