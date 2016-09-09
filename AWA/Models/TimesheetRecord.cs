using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AWA.Models
{
    public class TimesheetRecord
    {
        public int TimesheetRecordId { set; get; }

        public string WorkType { set; get; }
        public long StartTime { set; get; }
        public long EndTime { set; get; }
        public long TotalTime { set; get; }
        public string Description { set; get; }

        public int? MachineId { get; set; }
        public Machine Machine { set; get; }

        public int? AttachmentId { get; set; }
        public Attachment Attachment { set; get; }

        public int TimesheetId { set; get; }
        public Timesheet Timesheet { get; set; }
    }
}
