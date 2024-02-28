using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using iPractice.DataAccess.Models;
using iPractice.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using iPractice.Api.Models;
using iPractice.Api.Models.Exception;
using iPractice.Api.Models.ApiVersion1.ResponseModels;
using TimeSlot = iPractice.Api.Models.TimeSlot;

namespace iPractice.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PsychologistController : ControllerBase
    {
        private readonly ILogger<PsychologistController> _logger;
        private readonly IAvailabilityService _availabilityService;

        public PsychologistController(ILogger<PsychologistController> logger, IAvailabilityService availabilityService)
        {
            _logger = logger;
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public string Get()
        {
            return "Success";
        }

        /// <summary>
        /// Gets the availabilities for a specific psychologist.
        /// </summary>
        /// <param name="psychologistId">The ID of the psychologist.</param>
        /// <returns>A list of availability blocks.</returns>
        [HttpGet("{psychologistId}/availabilities")]
        public async Task<ActionResult> GetAvailabilities(long psychologistId)
        {
            try
            {
                var availabilities = await _availabilityService.GetAvailabilitiesForPsychologist(psychologistId);
                return Ok(availabilities);
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
        /// Add a block of time during which the psychologist is available during normal business hours.
        /// </summary>
        /// <param name="psychologistId"></param>
        /// <param name="availability">Availability</param>
        /// <returns>Ok if the availability was created</returns>
        [HttpPost("{psychologistId}/availability")]
        [ProducesResponseType(typeof(AvailabilityResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AvailabilityResponse>> CreateAvailability([FromRoute] long psychologistId, [FromBody] TimeSlot timeSlot)
        {
            try
            {
                AvailabilityResponse response = await _availabilityService.AddAvailability(psychologistId,timeSlot);
                return Ok(response); 
            }
            catch (PsychologistAbsentException ex)
            {
                // Log the exception details
                _logger.LogError(ex.Message);
                return BadRequest( "Internal server error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "There is an internal server error.");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        /// <summary>
        /// Update availability of a psychologist
        /// </summary>
        /// <param name="psychologistId">The psychologist's ID</param>
        /// <param name="availabilityId">The ID of the availability block</param>
        /// <returns>List of availability slots</returns>
        [HttpPut("{psychologistId}/availability/{availabilityId}")]
        [ProducesResponseType(typeof(Availability), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Availability>> UpdateAvailability([FromRoute] long psychologistId, [FromRoute] long availabilityId, [FromBody] Availability availability)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update availability of a psychologist
        /// </summary>
        /// <param name="psychologistId">The psychologist's ID</param>
        /// <param name="availabilityId">The ID of the availability block</param>
        /// <returns>List of availability slots</returns>
        [HttpDelete("{psychologistId}/availability")]
        [ProducesResponseType(typeof(Availability), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Availability>> UpdateAvailability([FromBody] Availability availabilityInput)
        {
            throw new NotImplementedException();
        }
    }
}
