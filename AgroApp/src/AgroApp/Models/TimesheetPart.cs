using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Models
{
    public class TimesheetPart

    {
        public int IdTimesheetPart { set; get; }
        public int IdEmployeeAssignment { set; get; }
        public string Description { set; get; }
        public long StartTime { set; get; }
        public long EndTime { set; get; }
        public long TotalTime { set; get; }


        public TimesheetPart(int idTimesheetPart, int idEmployeeAssignment, long startTime = 0, long endTime = 0, long totalTime = 0, string description = "")
        {
            IdEmployeeAssignment = idEmployeeAssignment;
            IdTimesheetPart = idTimesheetPart;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            TotalTime = totalTime;
        }
    }
}
