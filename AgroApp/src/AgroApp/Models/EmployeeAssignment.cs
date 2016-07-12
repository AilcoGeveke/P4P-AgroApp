using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class EmployeeAssignment
    {
        public int IdEmployeeAssignment { private set; get; }
        public User Employee { private set; get; }
        public Assignment Assignment { private set; get; }

        public EmployeeAssignment(int idEmployeeAssignment, User employee, Assignment assignment)
        {
            IdEmployeeAssignment = idEmployeeAssignment;
            Employee = employee;
            Assignment = assignment;
        }
    }
}
