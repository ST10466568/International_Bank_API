using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HopewellClinicApi.Services;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Attributes;
using System.Security.Claims;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtAuthorize]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly DoctorScheduleService _scheduleService;
        private readonly ILogger<DoctorScheduleController> _logger;

        public DoctorScheduleController(DoctorScheduleService scheduleService, ILogger<DoctorScheduleController> logger)
        {
            _scheduleService = scheduleService;
            _logger = logger;
        }

        /// <summary>
        /// Get doctor's weekly shift schedule (Frontend compatible)
        /// </summary>
        [HttpGet("{id}/shifts")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctorShifts(Guid id)
        {
            try
            {
                var doctor = await _scheduleService.GetDoctorAsync(id);
                if (doctor == null)
                {
                    return NotFound(new { error = "DOCTOR_NOT_FOUND", message = "Doctor not found" });
                }

                var shifts = await _scheduleService.GetDoctorWeeklyShiftsAsync(id);
                
                var shiftResults = shifts.Select(s => new
                {
                    id = s.Id,
                    dayOfWeek = s.DayOfWeek,
                    startTime = s.ShiftStart.ToString(@"hh\:mm"),
                    endTime = s.ShiftEnd.ToString(@"hh\:mm"),
                    isActive = s.IsActive,
                    doctorId = s.DoctorId
                }).ToList();

                return Ok(shiftResults);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doctor shifts for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while retrieving the shifts" });
            }
        }

        /// <summary>
        /// Update doctor's weekly shift schedule (Frontend compatible)
        /// </summary>
        [HttpPut("{id}/shifts")]
        [JwtAuthorize]
        public async Task<ActionResult<object>> UpdateDoctorShifts(Guid id, [FromBody] UpdateDoctorShiftsRequest request)
        {
            try
            {
                var doctor = await _scheduleService.GetDoctorAsync(id);
                if (doctor == null)
                {
                    return NotFound(new { error = "DOCTOR_NOT_FOUND", message = "Doctor not found" });
                }

                var updatedShifts = await _scheduleService.UpdateDoctorWeeklyShiftsAsync(id, request.Shifts);
                
                var shiftResults = updatedShifts.Select(s => new
                {
                    id = s.Id,
                    dayOfWeek = s.DayOfWeek,
                    startTime = s.ShiftStart.ToString(@"hh\:mm"),
                    endTime = s.ShiftEnd.ToString(@"hh\:mm"),
                    isActive = s.IsActive,
                    doctorId = s.DoctorId
                }).ToList();

                return Ok(new
                {
                    success = true,
                    message = "Shift schedule updated successfully",
                    updatedShifts = shiftResults
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor shifts for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while updating the shifts" });
            }
        }

        /// <summary>
        /// Get doctor's weekly schedule
        /// </summary>
        [HttpGet("{id}/schedule")]
        public async Task<ActionResult<DoctorScheduleManagementResponse>> GetDoctorSchedule(
            Guid id, 
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var schedule = await _scheduleService.GetDoctorScheduleAsync(id, startDate, endDate);
                return Ok(schedule);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doctor schedule for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while retrieving the schedule" });
            }
        }

        /// <summary>
        /// Update doctor's schedule
        /// </summary>
        [HttpPut("{id}/schedule")]
        public async Task<ActionResult<DoctorScheduleManagementResponse>> UpdateDoctorSchedule(
            Guid id, 
            [FromBody] UpdateDoctorScheduleRequest request)
        {
            try
            {
                var schedule = await _scheduleService.UpdateDoctorScheduleAsync(id, request);
                return Ok(schedule);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating doctor schedule for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while updating the schedule" });
            }
        }

        /// <summary>
        /// Check doctor availability at specific time
        /// </summary>
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<DoctorAvailabilityManagementResponse>> CheckDoctorAvailability(
            Guid id, 
            [FromQuery] DateTime date, 
            [FromQuery] TimeSpan? time = null)
        {
            try
            {
                var availability = await _scheduleService.CheckDoctorAvailabilityAsync(id, date, time);
                return Ok(availability);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking doctor availability for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while checking availability" });
            }
        }

        /// <summary>
        /// Generate available time slots for doctor
        /// </summary>
        [HttpGet("{id}/time-slots")]
        public async Task<ActionResult<List<TimeSlotEnhancedDto>>> GetAvailableTimeSlots(
            Guid id, 
            [FromQuery] DateTime date, 
            [FromQuery] int serviceDuration = 30)
        {
            try
            {
                var slots = await _scheduleService.GenerateAvailableSlotsAsync(id, date, serviceDuration);
                return Ok(slots);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating time slots for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while generating time slots" });
            }
        }

        /// <summary>
        /// Get doctor schedule summary
        /// </summary>
        [HttpGet("{id}/summary")]
        public async Task<ActionResult<DoctorScheduleSummaryDto>> GetDoctorScheduleSummary(Guid id)
        {
            try
            {
                var summary = await _scheduleService.GetDoctorScheduleSummaryAsync(id);
                return Ok(summary);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting doctor schedule summary for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while retrieving the summary" });
            }
        }

        /// <summary>
        /// Initialize default schedule for doctor
        /// </summary>
        [HttpPost("{id}/initialize-schedule")]
        public async Task<ActionResult> InitializeDefaultSchedule(Guid id)
        {
            try
            {
                await _scheduleService.InitializeDefaultScheduleAsync(id);
                return Ok(new { message = "Default schedule initialized successfully" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing default schedule for doctor: {DoctorId}", id);
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while initializing the schedule" });
            }
        }

        /// <summary>
        /// Get current doctor's own schedule (for authenticated doctor)
        /// </summary>
        [HttpGet("my-schedule")]
        public async Task<ActionResult<DoctorScheduleManagementResponse>> GetMySchedule(
            [FromQuery] DateTime? startDate = null, 
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(doctorId) || !Guid.TryParse(doctorId, out var doctorGuid))
                {
                    return Unauthorized(new { error = "INVALID_TOKEN", message = "Invalid or missing user token" });
                }

                var schedule = await _scheduleService.GetDoctorScheduleAsync(doctorGuid, startDate, endDate);
                return Ok(schedule);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current doctor's schedule");
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while retrieving the schedule" });
            }
        }

        /// <summary>
        /// Update current doctor's own schedule (for authenticated doctor)
        /// </summary>
        [HttpPut("my-schedule")]
        public async Task<ActionResult<DoctorScheduleManagementResponse>> UpdateMySchedule([FromBody] UpdateDoctorScheduleRequest request)
        {
            try
            {
                var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(doctorId) || !Guid.TryParse(doctorId, out var doctorGuid))
                {
                    return Unauthorized(new { error = "INVALID_TOKEN", message = "Invalid or missing user token" });
                }

                var schedule = await _scheduleService.UpdateDoctorScheduleAsync(doctorGuid, request);
                return Ok(schedule);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = "DOCTOR_NOT_FOUND", message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating current doctor's schedule");
                return StatusCode(500, new { error = "INTERNAL_ERROR", message = "An error occurred while updating the schedule" });
            }
        }
    }
}
