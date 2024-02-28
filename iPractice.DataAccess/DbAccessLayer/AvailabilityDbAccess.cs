using iPractice.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iPractice.DataAccess.DbAccessLayer
{
    public class AvailabilityDbAccess : IAvailabilityDbAccess
    {
        private readonly ApplicationDbContext _context;

        public AvailabilityDbAccess( ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Availability>> GetAvailabilitiesForClientFromDb(long clientId)
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
                .Where(availability => !availability.IsBooked)
                .ToListAsync();

            return availabilities;
        }

        public async Task<List<Availability>> GetAvailabilitiesForPsychologistFromDb(long psychologistId)
        {
            // Fetch with psychologistId as foreign key.
            return await _context.AvailableSlots
                                    .Include(psych => psych.Psychologist)
                                    .Where(availability => availability.Psychologist.Id == psychologistId
                                        && !availability.IsBooked)
                                    .ToListAsync();
        }

        public async Task<bool> PsychologistExistsInDb(long psychologistId)
        {
            return await _context.Psychologists.AnyAsync(p => p.Id == psychologistId);
        }

        public async Task<Availability> AddAvailabilityToDb(TimeSlot timeSlot, long psychologistId)
        {
            var psych = _context.Psychologists.FirstOrDefault(p => p.Id == psychologistId);
            var availability = new Availability(timeSlot.StartTimeSlot, timeSlot.EndTimeSlot, psych);

            // Check if an Availability record already exists for this psychologist and time
            var exists = await _context.AvailableSlots
                                      .AnyAsync(a => a.Psychologist.Id == psychologistId && a.StartTimeSlot == timeSlot.StartTimeSlot);

            if (!exists)
            {
                await _context.AvailableSlots.AddAsync(availability);
            }

            await _context.SaveChangesAsync();
            return availability;
        }

    }
}
