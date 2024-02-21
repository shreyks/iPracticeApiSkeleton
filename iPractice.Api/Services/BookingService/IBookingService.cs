using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iPractice.Api.Models;
using iPractice.Api.Models.ApiVersion1.ResponseModels;
using iPractice.DataAccess.Models;

namespace iPractice.Api.Services
{
    public interface IBookingService
    {
        Task<bool> CreateBooking(long clientId, long psychologistId, TimeSlot timeSlot);
    }
}
