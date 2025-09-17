using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/time-slots")]
    public class TimeSlotsController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public TimeSlotsController(HopewellDbContext context)
        {
            _context = context;
        }

        [HttpGet("by-day/{day}")]
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetTimeSlotsByDay(int day)
        {
            try
            {
                if (day < 0 || day > 6)
                {
                    return BadRequest(new { error = "Day must be between 0 (Sunday) and 6 (Saturday)" });
                }

                // Convert from 0=Sunday, 1=Monday format to our database format (1=Monday, 7=Sunday)
                int dayOfWeek = day == 0 ? 7 : day;

                var timeSlots = await _context.TimeSlots
                    .Where(t => t.DayOfWeek == dayOfWeek && t.IsActive)
                    .Select(t => new TimeSlotResponse
                    {
                        Id = t.Id,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
                        DayOfWeek = t.DayOfWeek,
                        IsActive = t.IsActive,
                        CreatedAt = t.CreatedAt
                    })
                    .OrderBy(t => t.StartTime)
                    .ToListAsync();

                return Ok(timeSlots);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("test-filter")]
        public async Task<ActionResult> TestFilter([FromQuery] DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0) dayOfWeek = 7;
            
            var allSlots = await _context.TimeSlots.Where(t => t.IsActive).ToListAsync();
            var filteredSlots = allSlots.Where(t => t.DayOfWeek == dayOfWeek).ToList();
            
            return Ok(new {
                requestedDate = date.ToString("yyyy-MM-dd"),
                requestedDayOfWeek = dayOfWeek,
                totalSlots = allSlots.Count,
                filteredSlotsCount = filteredSlots.Count,
                allSlots = allSlots.Select(s => new { s.Id, s.DayOfWeek, s.StartTime }),
                filteredSlots = filteredSlots.Select(s => new { s.Id, s.DayOfWeek, s.StartTime })
            });
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetAvailableTimeSlots(
            [FromQuery] DateTime? date = null)
        {
            try
            {
                if (date.HasValue)
                {
                    // Get the day of week for the requested date (1=Monday, 7=Sunday)
                    var dayOfWeek = (int)date.Value.DayOfWeek;
                    if (dayOfWeek == 0) dayOfWeek = 7; // Convert Sunday from 0 to 7

                    // Get all active time slots first, then filter by day of week in memory
                    var allTimeSlots = await _context.TimeSlots
                        .Where(t => t.IsActive)
                        .ToListAsync();

                    // Filter by day of week
                    var dayTimeSlots = allTimeSlots
                        .Where(t => t.DayOfWeek == dayOfWeek)
                        .ToList();

                    var existingAppointments = await _context.Appointments
                        .Where(a => a.AppointmentDate == date.Value && a.Status == "confirmed")
                        .ToListAsync();

                    var availableSlots = dayTimeSlots
                        .Where(slot => !existingAppointments.Any(apt => apt.StartTime == slot.StartTime))
                        .Select(slot => new TimeSlotResponse
                        {
                            Id = slot.Id,
                            StartTime = slot.StartTime,
                            EndTime = slot.EndTime,
                            DayOfWeek = slot.DayOfWeek,
                            IsActive = slot.IsActive,
                            CreatedAt = slot.CreatedAt
                        })
                        .OrderBy(s => s.StartTime)
                        .ToList();

                    return Ok(availableSlots);
                }
                else
                {
                    // Get all active time slots
                    var timeSlots = await _context.TimeSlots
                        .Where(t => t.IsActive)
                        .Select(t => new TimeSlotResponse
                        {
                            Id = t.Id,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            DayOfWeek = t.DayOfWeek,
                            IsActive = t.IsActive,
                            CreatedAt = t.CreatedAt
                        })
                        .OrderBy(t => t.StartTime)
                        .ToListAsync();

                    return Ok(timeSlots);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetTimeSlots()
        {
            try
            {
                var timeSlots = await _context.TimeSlots
                    .Where(t => t.IsActive)
                    .Select(t => new TimeSlotResponse
                    {
                        Id = t.Id,
                        DayOfWeek = t.DayOfWeek,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
                        IsActive = t.IsActive,
                        CreatedAt = t.CreatedAt
                    })
                    .ToListAsync();

                return Ok(timeSlots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}