using iPractice.DataAccess.Models;
using System;

namespace iPractice.Api.Models.ApiVersion1.RequestModels
{
    public class AvailabilityRequest
    {
        public long Id { get; set; }
        public Psychologist Psychologist { get; set; }
        public DateTime StartTimeSlot { get; set; }
        public DateTime EndTimeSlot { get; set; }
        public bool IsBooked { get; set; } = false;
    }
}
