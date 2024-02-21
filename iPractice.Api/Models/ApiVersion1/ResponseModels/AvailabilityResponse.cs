using iPractice.Api.Models;
using iPractice.DataAccess.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iPractice.Api.Models.ApiVersion1.ResponseModels
{
    public class AvailabilityResponse
    {
        public List<TimeSlot> AvailableSlots { get; set; }
        public AvailabilityResponse(List<Availability> availabilities)
        {
            AvailableSlots = availabilities.Select(a => new TimeSlot { StartTimeSlot = a.StartTimeSlot, EndTimeSlot = a.EndTimeSlot }).ToList();
        }
    }
}
