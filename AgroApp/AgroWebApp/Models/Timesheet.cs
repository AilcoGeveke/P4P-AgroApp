using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Timesheet
    {
        public int IdTimesheet { set; get; }
        public string WorkType { set; get; }
        public long StartTime { set; get; }
        public long EndTime { set; get; }
        public long TotalTime{ set; get; }
        public string Description { set; get; }
        public Machine[] Machines { set; get; }
        public Attachment[] Attachments { set; get; }

        public Timesheet() { }

        public Timesheet(int idTimesheet, string workType, long startTime, long endTime, long totalTime, string description)

        {
            IdTimesheet = idTimesheet;
            WorkType = workType;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
            Description = description; 
        }
    }
}
