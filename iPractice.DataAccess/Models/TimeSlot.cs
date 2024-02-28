using System;

namespace iPractice.DataAccess.Models
{
    public class TimeSlot
    {
        public DateTime StartTimeSlot { get; set; }
        public DateTime EndTimeSlot { get; set; }
        public TimeSlot(DateTime startTimeSlot, DateTime endTimeSlot)
        {
            StartTimeSlot = startTimeSlot;
            EndTimeSlot = endTimeSlot;
        }

    }
}