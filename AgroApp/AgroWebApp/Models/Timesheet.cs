using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Timesheet
    {
        public int IdTimesheet { set; get; }
        public int IdEmployeeAssignment { set; get; }
        public string WorkType { set; get; }
        public long StartTime { set; get; }
        public long EndTime { set; get; }
        public long TotalTime{ set; get; }
        public string Description { set; get; }
        public Machine[] Machines { set; get; } = new Machine[0];
        public Attachment[] Attachments { set; get; } = new Attachment[0];

        public Timesheet() { }

        public Timesheet(int idTimesheet, int idEmployeeAssignment, string workType, long startTime, long endTime, long totalTime, string description)

        {
            IdTimesheet = idTimesheet;
            IdEmployeeAssignment = idEmployeeAssignment;
            WorkType = workType;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
            Description = description; 
        }
    }
}
