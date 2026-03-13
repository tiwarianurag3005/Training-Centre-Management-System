using System;

namespace TrainingCentreManagement.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public int BatchId { get; set; }

        public DateTime Date { get; set; }

        public string Topic { get; set; }
    }
}