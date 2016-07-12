using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class EmployeeAssignment
    {
        public int IdEmployeeAssignment { private set; get; }
        public User Werknemer { private set; get; }
        public Assignment Opdracht { private set; get; }
        public DateTime? Date { private set; get; }

        public EmployeeAssignment(int idEmployeeAssignment, User werknemer, Assignment opdracht, DateTime? date)
        {
            Idassignment = idEmployeeAssignment;
            Location = location;
            Description = description;
            Date = date;
            Customer = selectedCustomer;
        }
    }
}
