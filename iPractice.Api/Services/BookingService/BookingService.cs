using iPractice.DataAccess;

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

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
