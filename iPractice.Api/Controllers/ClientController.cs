using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using iPractice.Api.Models;
using iPractice.Api.Models.Exception;
using iPractice.Api.Services;
using iPractice.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeSlot = iPractice.Api.Models.TimeSlot;

namespace iPractice.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IAvailabilityService _availabilityService;
        private readonly IBookingService _bookingService;

        public ClientController(ILogger<ClientController> logger, IAvailabilityService availabilityService, IBookingService bookingService)
        {
            _logger = logger;
            _availabilityService = availabilityService;
            _bookingService = bookingService;
        }
        
        /// <summary>
        /// The client can see when his psychologists are available.
        /// Get available slots from his two psychologists.
        /// </summary>
        /// <param name="clientId">The client ID</param>
        /// <returns>All time slots for the selected client</returns>
        [HttpGet("{clientId}/timeslots")]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> GetAvailableTimeSlots(long clientId)
        {
            try
            {
                var availabilities = await _availabilityService.GetAvailabilitiesForClient(clientId);
                return Ok( availabilities);

            }
            catch (PsychologistAbsentException ex)
            {
                // Log the exception details
                _logger.LogError(ex.Message);
                return BadRequest("Internal server error: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching availabilities.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Create an appointment for a given availability slot
        /// </summary>
        /// <param name="clientId">The client ID</param>
        /// <param name="timeSlot">Identifies the client and availability slot</param>
        /// <returns>Ok if appointment was made</returns>
        [HttpPost("{clientId}/appointment")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAppointment(long clientId, long psychologistId, [FromBody] TimeSlot timeSlot)
        {
            try
            {
                DataAccess.Models.TimeSlot dataAccessTimeSlot = new iPractice.DataAccess.Models.TimeSlot(timeSlot.StartTimeSlot, timeSlot.EndTimeSlot);
                var bookingDetails = await _bookingService.CreateBooking(clientId, psychologistId, dataAccessTimeSlot);
                if (bookingDetails)
                {
                    return Ok("Booking succesfull.");
                }
                else
                    return BadRequest("Booking Failed.");             

            }
            catch (PsychologistUnavailableExcpetion ex)
            {
                // Log the exception details
                _logger.LogError(ex.Message);
                return BadRequest("Internal server error: " + ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching availabilities.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
