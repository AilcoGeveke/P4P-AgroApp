using System;

namespace AWA.Models
{
    public class Timesheet
    {
        public int TimesheetId { set; get; }

        public string WorkType { set; get; }
        public TimeSpan StartTime { set; get; }
        public TimeSpan EndTime { set; get; }
        public TimeSpan TotalTime { set; get; }
        public string Description { set; get; }

        public Machine[] Machines { set; get; }
        public Attachment[] Attachments { set; get; }

        public int EmployeeAssignmentId { set; get; }
        public EmployeeAssignment EmployeeAssignment { get; set; }
    }
}
