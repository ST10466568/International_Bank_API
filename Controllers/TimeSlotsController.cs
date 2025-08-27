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

        [HttpGet("by-day/{dayOfWeek}")]
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetTimeSlotsByDay(int dayOfWeek)
        {
            try
            {
                if (dayOfWeek < 1 || dayOfWeek > 7)
                {
                    return BadRequest(new { error = "Day of week must be between 1 (Monday) and 7 (Sunday)" });
                }

                var timeSlots = await _context.TimeSlots
                    .Where(t => t.DayOfWeek == dayOfWeek && t.IsActive)
                    .Select(t => new TimeSlotResponse
                    {
                        Id = t.Id,
                        DayOfWeek = t.DayOfWeek,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
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

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetAvailableTimeSlots(
            [FromQuery] DateTime date,
            [FromQuery] Guid? serviceId = null,
            [FromQuery] Guid? staffId = null)
        {
            try
            {
                var dayOfWeek = (int)date.DayOfWeek;
                if (dayOfWeek == 0) dayOfWeek = 7; // Sunday = 7

                var baseTimeSlots = await _context.TimeSlots
                    .Where(t => t.DayOfWeek == dayOfWeek && t.IsActive)
                    .ToListAsync();

                if (!baseTimeSlots.Any())
                {
                    return Ok(new List<TimeSlotResponse>());
                }

                var service = serviceId.HasValue 
                    ? await _context.Services.FindAsync(serviceId.Value)
                    : null;

                var duration = service?.DurationMinutes ?? 30;

                var availableSlots = new List<TimeSlotResponse>();
                var bookedAppointments = await _context.Appointments
                    .Where(a => a.AppointmentDate == date && a.Status != "cancelled")
                    .ToListAsync();

                // Filter by staff if specified
                if (staffId.HasValue)
                {
                    bookedAppointments = bookedAppointments.Where(a => a.StaffId == staffId).ToList();
                }

                foreach (var slot in baseTimeSlots)
                {
                    var slotEndTime = slot.StartTime.AddMinutes(duration);
                    if (slotEndTime <= slot.EndTime)
                    {
                        // Check if this slot conflicts with any booked appointment
                        var hasConflict = bookedAppointments.Any(a => 
                            a.StartTime < slotEndTime && a.EndTime > slot.StartTime);

                        if (!hasConflict)
                        {
                            availableSlots.Add(new TimeSlotResponse
                            {
                                Id = slot.Id,
                                DayOfWeek = slot.DayOfWeek,
                                StartTime = slot.StartTime,
                                EndTime = slotEndTime,
                                IsActive = slot.IsActive,
                                CreatedAt = slot.CreatedAt
                            });
                        }
                    }
                }

                return Ok(availableSlots.OrderBy(s => s.StartTime));
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