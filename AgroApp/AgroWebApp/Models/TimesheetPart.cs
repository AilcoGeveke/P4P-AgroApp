using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class Timesheet
    {
        public int IdTimesheet { set; get; }
        public long StartTime { set; get; }
        public long EndTime { set; get; }
        public long TotalTime{ set; get; }
        public string Description { set; get; }

        public Timesheet() { }

        public Timesheet(int idTimesheet, long startTime, long endTime, long totalTime, string description)

        {
            IdTimesheet = idTimesheet;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
            Description = description; 
        }
    }
}
