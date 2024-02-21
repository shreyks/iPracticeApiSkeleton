using iPractice.DataAccess.Models;
using System;
using System.Security.Cryptography.X509Certificates;

namespace iPractice.Api.Models.DataTransferObjects
{
    public class AvailabilityDto
    {
        public long Id { get; set; }
        public Psychologist Psychologist { get; set; }
        public DateTime StartTimeSlot { get; set; }
        public DateTime EndTimeSlot { get; set; }
        public bool IsBooked { get; set; } = false;
        
        public AvailabilityDto(Availability availability) { 
            Id = availability.Id;
            Psychologist = availability.Psychologist;
            StartTimeSlot = availability.StartTimeSlot;
            EndTimeSlot = availability.EndTimeSlot;
            IsBooked = availability.IsBooked;
        }
    }
}
