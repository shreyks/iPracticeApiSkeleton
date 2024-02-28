using iPractice.Api.Controllers;
using iPractice.Api.Models;
using iPractice.Api.Models.ApiVersion1.ResponseModels;
using iPractice.Api.Services;
using iPractice.DataAccess.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TimeSlot = iPractice.Api.Models.TimeSlot;

namespace iPractice.Api.Tests.Controllers
{
    public class PsychologistControllerTests
    {
        private Mock<IAvailabilityService> _mockAvailabilityService;
        private Mock<ILogger<PsychologistController>> _mockLogger;
        private PsychologistController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockAvailabilityService = new Mock<IAvailabilityService>();
            _mockLogger = new Mock<ILogger<PsychologistController>>();
            _controller = new PsychologistController(_mockLogger.Object, _mockAvailabilityService.Object);
        }

        [TestMethod]
        public async Task CreateAvailabilityTestAsync()
        {
            long psychologistId = 1;

            var availabilities = new List<Availability>();
            var expectedResponse = new AvailabilityResponse(availabilities);

            _mockAvailabilityService.Setup(service => service.GetAvailabilitiesForPsychologist(psychologistId))
                .ReturnsAsync(expectedResponse);


            TimeSlot testTimeSlot = new TimeSlot(DateTime.Now, DateTime.Now.AddMinutes(60));

            var result = await _controller.CreateAvailability(psychologistId, testTimeSlot);

            List<TimeSlot> timeSlots = new List<TimeSlot>
            {
                new TimeSlot(testTimeSlot.StartTimeSlot, testTimeSlot.StartTimeSlot.AddMinutes(30)),
                new TimeSlot(testTimeSlot.StartTimeSlot.AddMinutes(30), testTimeSlot.EndTimeSlot)
            };
            Assert.IsNotNull(result);
            Assert.AreEqual(timeSlots.Count, result.Value.AvailableTimeSlots.Count);

            for (int i = 0; i < timeSlots.Count; i++)
            {
                Assert.AreEqual(timeSlots[i].StartTimeSlot, result.Value.AvailableTimeSlots[i].StartTimeSlot);
                Assert.AreEqual(timeSlots[i].EndTimeSlot, result.Value.AvailableTimeSlots[i].EndTimeSlot);
            }
        }

    }
}
