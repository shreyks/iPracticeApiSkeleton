using iPractice.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace iPractice.Api.Models
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