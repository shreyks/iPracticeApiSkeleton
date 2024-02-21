using iPractice.Api.Models;
using iPractice.Api.Models.ApiVersion1.ResponseModels;
using iPractice.Api.Models.Exception;
using iPractice.DataAccess;
using iPractice.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace iPractice.Api.Services
{
    //Task<IEnumerable<Availability>> GetAvailabilitiesForPsychologist(long psychologistId);
    //Task<Availability> GetAvailabilityById(long id);
    //Task<bool> AddAvailability(Availability availability);
    //Task<bool> UpdateAvailability(long id, Availability availability);
    //Task<bool> DeleteAvailability(long id);
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAvailabilityService _availabilityService;
        public BookingService(ApplicationDbContext context, IAvailabilityService availabilityService)
        {
            _context = context;
            _availabilityService = availabilityService;
        }

        public async Task<bool> CreateBooking(long clientId, long psychologistId, TimeSlot timeSlot)
        {
            // ToImplement: Check that Client exists.
            // ToImplement: Check if the client and psychologist are linked.

            // Check if the psych has available slots.
            var availability = await _context.AvailableSlots
                .Include(psych => psych.Psychologist)
                .FirstOrDefaultAsync(a =>
                    a.Psychologist.Id == psychologistId &&
                    a.StartTimeSlot == timeSlot.StartTimeSlot &&
                    a.EndTimeSlot == timeSlot.EndTimeSlot &&
                    !a.IsBooked);

            if (availability == null)
            {
                throw new PsychologistUnavailableExcpetion();
            }

            availability.IsBooked = true;
            var psych = _context.Psychologists.FirstOrDefault(p => p.Id == psychologistId);
            var client = _context.Clients.FirstOrDefault(c => c.Id == clientId);
            var newBooking = new Booking(
                    client: client,
                    psychologist: psych,
                    startTimeSlot: timeSlot.StartTimeSlot,
                    endTimeSlot: timeSlot.EndTimeSlot);
            await _context.Bookings.AddAsync(newBooking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
