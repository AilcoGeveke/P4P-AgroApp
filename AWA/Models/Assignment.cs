using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AWA.Models
{
    public class Assignment
    {
        public int AssignmentId { set; get; }
        public string Location { set; get; }
        public string Description { set; get; }
        public long? Date { set; get; }
            
        public int CustomerId { get; set; }
        public Customer Customer { set; get; }

        public ICollection<Timesheet> Timesheets { set; get; }

        [NotMapped] public ICollection<User> Employees { get; set; }
    }
}
    