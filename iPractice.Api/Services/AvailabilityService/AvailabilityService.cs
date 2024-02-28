using iPractice.Api.Controllers;
using iPractice.Api.Models;
using iPractice.Api.Models.ApiVersion1.ResponseModels;
using iPractice.Api.Models.Exception;
using iPractice.DataAccess;
using iPractice.DataAccess.DbAccessLayer;
using iPractice.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSlot = iPractice.Api.Models.TimeSlot;

namespace iPractice.Api.Services
{
    //Task<IEnumerable<Availability>> GetAvailabilitiesForPsychologist(long psychologistId);
    //Task<Availability> GetAvailabilityById(long id);
    //Task<bool> AddAvailability(Availability availability);
    //Task<bool> UpdateAvailability(long id, Availability availability);
    //Task<bool> DeleteAvailability(long id);
    public class AvailabilityService : IAvailabilityService
    {

        ILogger<AvailabilityService> _logger;
        private readonly IAvailabilityDbAccess _availabilityDbAccess;

        public AvailabilityService(ILogger<AvailabilityService> logger, IAvailabilityDbAccess availabilityDbAccess)
        {
            _logger = logger;
            _availabilityDbAccess = availabilityDbAccess;
        }

        public async Task<AvailabilityResponse> GetAvailabilitiesForPsychologist(long psychologistId)
        {

            bool psychologistExists = await _availabilityDbAccess.PsychologistExistsInDb(psychologistId);
            if (!psychologistExists)
                throw new PsychologistAbsentException();
            // Fetch with psychologistId as foreign key.
            List<Availability> availableSlots = await _availabilityDbAccess.GetAvailabilitiesForPsychologistFromDb(psychologistId);
            return new AvailabilityResponse(availableSlots);
        }

        public async Task<AvailabilityResponse> GetAvailabilitiesForClient(long clientId)
        {
            // Check if Client exists and raise an exception accordingly.

            // Fetch with psychologistId as foreign key.
            List<Availability> availableSlots = await _availabilityDbAccess.GetAvailabilitiesForClientFromDb(clientId);
            return new AvailabilityResponse(availableSlots);
        }

        public async Task<AvailabilityResponse> AddAvailability( long psychologistId, TimeSlot timeSlot)
        {
            bool psychologistExists = await _availabilityDbAccess.PsychologistExistsInDb(psychologistId);
            if (!psychologistExists)
                throw new PsychologistAbsentException();

            DateTime startTime = timeSlot.StartTimeSlot;
            DateTime endTime = timeSlot.EndTimeSlot;
            
            List<Availability> availableSlots = new List<Availability>();
            // Loop through each interval from start to end, in 30-minute increments.
            // Assuming that the time slot is in perfect 30m intervals. All edge cases will be handled here.
            while (startTime < endTime)
            {
                DateTime intervalEnd = startTime.AddMinutes(30) > endTime ? endTime : startTime.AddMinutes(30);
                iPractice.DataAccess.Models.TimeSlot intervalTimeSlot = new iPractice.DataAccess.Models.TimeSlot(startTime, intervalEnd);

                // Add the interval time slot to the database.
                var addedAvailabilty = await _availabilityDbAccess.AddAvailabilityToDb(intervalTimeSlot, psychologistId);

                availableSlots.Add(addedAvailabilty);
                // Move to the next interval.
                startTime = intervalEnd;
            }
            return new AvailabilityResponse(availableSlots);

        }

        public async Task<bool> DeleteAvailability(long id)
        {
            // Make a DeleteAvailabilityFromDb() function that erases the
            // availability data row that matches the id.
            throw new NotImplementedException();
        }
    }
}
