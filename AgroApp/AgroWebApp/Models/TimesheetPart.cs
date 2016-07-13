using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class TimesheetPart
    {
        public int IdTimesheetPart { set; get; }
        public long StartTime { set; get; }
        public long EndTime { set; get; }
        public long TotalTime{ set; get; }
        public string Description { set; get; }

        public TimesheetPart() { }

        public TimesheetPart(int idTimesheetPart, long startTime, long endTime, long totalTime, string description)

        {
            IdTimesheetPart = idTimesheetPart;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
            Description = description; 
        }
    }
}
