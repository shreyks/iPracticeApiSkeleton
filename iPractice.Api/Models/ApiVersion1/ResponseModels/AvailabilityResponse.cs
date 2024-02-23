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
        public List<TimeSlot> AvailableTimeSlots { get; set; }
        public AvailabilityResponse(List<Availability> availableTimeSlots)
        {
            AvailableTimeSlots = availableTimeSlots.Select(a => new TimeSlot(a.StartTimeSlot, a.EndTimeSlot)).ToList();
        }
    }
}
