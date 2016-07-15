using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class EmployeeAssignment
    {
        public int IdEmployeeAssignment { get; set; }
        public User Employee { get; set; }
        public Assignment Assignment { get; set; }
        public bool IsVerified { get; set; } = false;
    }
}
