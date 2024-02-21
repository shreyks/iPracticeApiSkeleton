using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace iPractice.DataAccess.Models
{
    public class Availability
    {
        public long Id { get; set; }
        public Psychologist Psychologist { get; set; }
        public DateTime StartTimeSlot { get; set; }
        public DateTime EndTimeSlot { get; set; }
        public bool IsBooked { get; set; } = false;
        public Availability() { }
        public Availability(DateTime start, DateTime end , Psychologist psychologist , bool isBooked = false)
        {
            StartTimeSlot = start;
            EndTimeSlot = end;
            IsBooked = isBooked;
            Psychologist = psychologist;
        }
    }
}