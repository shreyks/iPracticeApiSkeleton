using iPractice.Api.Controllers;
using iPractice.Api.Models;
using iPractice.Api.Models.ApiVersion1.ResponseModels;
using iPractice.Api.Models.Exception;
using iPractice.DataAccess;
using iPractice.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly ApplicationDbContext _context;

        public AvailabilityService(ILogger<AvailabilityService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> ClientExists(long clientId)
        {
            return await _context.Clients.AnyAsync(c => c.Id == clientId);
        }

        public async Task<AvailabilityResponse> GetAvailabilitiesForPsychologist(long psychologistId)
        {

            bool psychologistExists = await this.PsychologistExistsInDb(psychologistId);
            if (!psychologistExists)
                throw new PsychologistAbsentException();
            // Fetch with psychologistId as foreign key.
            List<Availability> availableSlots = await GetAvailabilitiesForPsychologistFromDb(psychologistId);
            return new AvailabilityResponse(availableSlots);
        }

        public async Task<AvailabilityResponse> GetAvailabilitiesForClient(long clientId)
        {
            // Check if Client exists and raise an exception accordingly.

            // Fetch with psychologistId as foreign key.
            List<Availability> availableSlots = await GetAvailabilitiesForPsychologistFromDb(clientId);
            return new AvailabilityResponse(availableSlots);
        }

        public async Task<AvailabilityResponse> AddAvailability( long psychologistId, TimeSlot timeSlot)
        {
            bool psychologistExists = await this.PsychologistExistsInDb(psychologistId);
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
                TimeSlot intervalTimeSlot = new TimeSlot { StartTimeSlot = startTime, EndTimeSlot = intervalEnd };

                // Add the interval time slot to the database.
                var addedAvailabilty = await this.AddAvailabilityToDb(intervalTimeSlot, psychologistId);

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


        private async Task<List<Availability>> GetAvailabilitiesForClientFromDb(long clientId)
        {
            var client = await _context.Clients
                .Include(x => x.Psychologists)
                .FirstOrDefaultAsync(x => x.Id == clientId);

            if (client.Psychologists == null || !client.Psychologists.Any())
            {
                return new List<Availability>();
            }
            var psychologystIds = client.Psychologists.Select(x => x.Id).ToList();

            List<Availability> availabilities = await _context.AvailableSlots
                .Include(x => x.Psychologist)
                .Where(x => psychologystIds.Any(id => id == x.Psychologist.Id))
                .ToListAsync();

            return availabilities;
        }

        private async Task<List<Availability>> GetAvailabilitiesForPsychologistFromDb(long psychologistId)
        {
            // Fetch with psychologistId as foreign key.
            return await _context.AvailableSlots
                                    .Include(psych => psych.Psychologist)
                                    .Where(availability => availability.Psychologist.Id == psychologistId)
                                    .ToListAsync();
        }

        private async Task<bool> PsychologistExistsInDb(long psychologistId)
        {
            return await _context.Psychologists.AnyAsync(p => p.Id == psychologistId);
        }

        private async Task<Availability> AddAvailabilityToDb(TimeSlot timeSlot, long psychologistId)
        {
            var psych = _context.Psychologists.FirstOrDefault(p => p.Id == psychologistId);
            var availability = new Availability(timeSlot.StartTimeSlot, timeSlot.EndTimeSlot, psych);

            // Check if an Availability record already exists for this psychologist and time
            var exists = await _context.AvailableSlots
                                      .AnyAsync(a => a.Psychologist.Id == psychologistId && a.StartTimeSlot == timeSlot.StartTimeSlot);

            if(!exists)
            {
                await _context.AvailableSlots.AddAsync(availability);
            }

            await _context.SaveChangesAsync();
            return availability;
        }
    }
}
