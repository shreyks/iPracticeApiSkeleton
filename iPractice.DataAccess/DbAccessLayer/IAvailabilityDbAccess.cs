using iPractice.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iPractice.DataAccess.DbAccessLayer
{
    public interface IAvailabilityDbAccess
    {
        Task<List<Availability>> GetAvailabilitiesForClientFromDb(long clientId);
        Task<List<Availability>> GetAvailabilitiesForPsychologistFromDb(long psychologistId);
        Task<bool> PsychologistExistsInDb(long psychologistId);
        Task<Availability> AddAvailabilityToDb(TimeSlot timeSlot, long psychologistId);
    }
}
