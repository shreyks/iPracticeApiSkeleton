using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iPractice.Api.Models;
using iPractice.Api.Models.ApiVersion1.ResponseModels;
using iPractice.DataAccess.Models;

namespace iPractice.Api.Services
{
    public interface IAvailabilityService
    {
        Task<AvailabilityResponse> GetAvailabilitiesForPsychologist(long psychologistId);
        Task<AvailabilityResponse> GetAvailabilitiesForClient(long clientId);
        Task<AvailabilityResponse> AddAvailability(long psychologistId, TimeSlot timeslot);
        Task<bool> DeleteAvailability(long id);
    }
}
