using System;
using System.Collections.Generic;
using System.Text;

namespace iPractice.DataAccess.Models
{
    public class Booking
    {
        public long Id { get; set; }
        public long PsychologistId { get; set; }
        public long ClientId { get; set; }
        public DateTime StartTimeSlot { get; set; }
        public DateTime EndTimeSlot { get; set; }
        public Psychologist Psychologist { get; set; }
        public Client Client { get; set; }
    }
}
