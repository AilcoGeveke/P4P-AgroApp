using System;
using System.Collections.Generic;

namespace AWA.Models
{
    public class Timesheet
    {
        public int TimesheetId { set; get; }

        public string WorkType { set; get; }
        public DateTime StartTime { set; get; }
        public DateTime EndTime { set; get; }
        public DateTime TotalTime { set; get; }
        public string Description { set; get; }

        public Machine[] Machines { set; get; }
        public Attachment[] Attachments { set; get; }

        public int EmployeeAssignmentId { set; get; }
        public ICollection<EmployeeAssignment> EmployeeAssignments { get; set; }
    }
}
