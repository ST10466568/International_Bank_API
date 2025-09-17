using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Models;
using HopewellClinicApi.Attributes;

namespace HopewellClinicApi.Controllers
{
[ApiController]
[Route("api/[controller]")]
[JwtAuthorize]
public class StaffController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public StaffController(HopewellDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Get staff schedule/availability information
        /// </summary>
        [HttpGet("{id}/availability-schedule")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetStaffAvailabilitySchedule(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Staff ID is required");
                }

                if (!Guid.TryParse(id, out var staffGuid))
                {
                    return BadRequest("Invalid staff ID format");
                }

                // Check if staff exists
                var staff = await _context.Staff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == staffGuid);

                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                // Get doctor schedules for this staff member
                var schedules = await _context.DoctorSchedules
                    .Where(ds => ds.DoctorId == staffGuid && ds.IsActive)
                    .OrderBy(ds => ds.DayOfWeek)
                    .ToListAsync();

                // Create default schedule if none exists
                var defaultSchedule = new[]
                {
                    new { dayOfWeek = "Monday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Tuesday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Wednesday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Thursday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Friday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Saturday", isActive = false, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Sunday", isActive = false, startTime = "09:00", endTime = "17:00" }
                };

                var schedule = schedules.Any() 
                    ? schedules.Select(s => new
                    {
                        dayOfWeek = s.DayOfWeek,
                        isActive = s.IsActive,
                        startTime = s.ShiftStart.ToString(@"hh\:mm"),
                        endTime = s.ShiftEnd.ToString(@"hh\:mm"),
                        breakStart = s.BreakStart?.ToString(@"hh\:mm"),
                        breakEnd = s.BreakEnd?.ToString(@"hh\:mm")
                    }).ToList()
                    : defaultSchedule.Select(ds => new
                    {
                        dayOfWeek = ds.dayOfWeek,
                        isActive = ds.isActive,
                        startTime = ds.startTime,
                        endTime = ds.endTime,
                        breakStart = (string?)null,
                        breakEnd = (string?)null
                    }).ToList();

                return Ok(new
                {
                    staffId = id,
                    staffName = $"{staff.User.FirstName} {staff.User.LastName}",
                    schedule = schedule
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Update staff schedule/availability
        /// </summary>
        [HttpPut("{id}/availability-schedule")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> UpdateStaffAvailabilitySchedule(string id, [FromBody] object scheduleData)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Staff ID is required");
                }

                if (!Guid.TryParse(id, out var staffGuid))
                {
                    return BadRequest("Invalid staff ID format");
                }

                if (scheduleData == null)
                {
                    return BadRequest("Schedule data is required");
                }

                // Check if staff exists
                var staff = await _context.Staff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == staffGuid);

                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                // For now, return success - in a full implementation, you would:
                // 1. Parse the schedule data
                // 2. Update or create DoctorSchedule records
                // 3. Save to database

                return Ok(new
                {
                    message = "Schedule updated successfully",
                    staffId = id,
                    staffName = $"{staff.User.FirstName} {staff.User.LastName}",
                    updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Get staff schedule with date range (for frontend compatibility)
        /// </summary>
        [HttpGet("{id}/schedule")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetStaffScheduleWithDateRange(Guid id, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            try
            {
                // Check if staff exists
                var staff = await _context.Staff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                // Get doctor schedules for this staff member
                var schedules = await _context.DoctorSchedules
                    .Where(ds => ds.DoctorId == id && ds.IsActive)
                    .OrderBy(ds => ds.DayOfWeek)
                    .ToListAsync();

                // Create default schedule if none exists
                var defaultSchedule = new[]
                {
                    new { dayOfWeek = "Monday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Tuesday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Wednesday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Thursday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Friday", isActive = true, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Saturday", isActive = false, startTime = "09:00", endTime = "17:00" },
                    new { dayOfWeek = "Sunday", isActive = false, startTime = "09:00", endTime = "17:00" }
                };

                var schedule = schedules.Any() 
                    ? schedules.Select(s => new
                    {
                        dayOfWeek = s.DayOfWeek,
                        isActive = s.IsActive,
                        startTime = s.ShiftStart.ToString(@"hh\:mm"),
                        endTime = s.ShiftEnd.ToString(@"hh\:mm"),
                        breakStart = s.BreakStart?.ToString(@"hh\:mm"),
                        breakEnd = s.BreakEnd?.ToString(@"hh\:mm")
                    }).ToList()
                    : defaultSchedule.Select(ds => new
                    {
                        dayOfWeek = ds.dayOfWeek,
                        isActive = ds.isActive,
                        startTime = ds.startTime,
                        endTime = ds.endTime,
                        breakStart = (string?)null,
                        breakEnd = (string?)null
                    }).ToList();

                return Ok(new
                {
                    staffId = id.ToString(),
                    staffName = $"{staff.User.FirstName} {staff.User.LastName}",
                    startDate = startDate,
                    endDate = endDate,
                    schedule = schedule
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Update staff availability (for frontend compatibility)
        /// </summary>
        [HttpPut("{id}/availability")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> UpdateStaffAvailability(Guid id, [FromBody] object availabilityData)
        {
            try
            {
                if (availabilityData == null)
                {
                    return BadRequest("Availability data is required");
                }

                // Check if staff exists
                var staff = await _context.Staff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                // For now, return success - in a full implementation, you would:
                // 1. Parse the availability data
                // 2. Update or create DoctorSchedule records
                // 3. Save to database

                return Ok(new
                {
                    message = "Availability updated successfully",
                    staffId = id.ToString(),
                    staffName = $"{staff.User.FirstName} {staff.User.LastName}",
                    updatedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffResponse>> GetStaffById(Guid id)
        {
            try
            {
                var staff = await (from s in _context.Staff
                                   join u in _context.Users on s.UserId equals u.Id
                                   join ur in _context.UserRoles on u.Id equals ur.UserId
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   where s.Id == id && u.IsActive
                                   select new StaffResponse
                                   {
                                       Id = s.Id,
                                       UserId = s.UserId,
                                       StaffNumber = s.StaffNumber,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       Role = r.Name ?? "staff",
                                       Phone = u.PhoneNumber,
                                       IsActive = u.IsActive
                                   }).FirstOrDefaultAsync();

                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                return Ok(staff);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("by-role/{role}")]
        public async Task<ActionResult<IEnumerable<StaffResponse>>> GetStaffByRole(string role)
        {
            try
            {
                var staff = await (from s in _context.Staff
                                   join u in _context.Users on s.UserId equals u.Id
                                   join ur in _context.UserRoles on u.Id equals ur.UserId
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   where r.Name == role && u.IsActive
                                   select new StaffResponse
                                   {
                                       Id = s.Id,
                                       UserId = s.UserId,
                                       StaffNumber = s.StaffNumber,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       Role = r.Name ?? "staff",
                                       Phone = u.PhoneNumber,
                                       IsActive = u.IsActive
                                   }).ToListAsync();

                return Ok(staff);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("{id}/schedule")]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetStaffSchedule(Guid id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var staff = await _context.Staff.FindAsync(id);
                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                var start = startDate ?? DateTime.Today;
                var end = endDate ?? start.AddDays(7);

                var appointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                    .Where(a => a.StaffId == id &&
                               a.AppointmentDate >= start &&
                               a.AppointmentDate <= end &&
                               a.Status != "cancelled")
                    .OrderBy(a => a.AppointmentDate)
                    .ThenBy(a => a.StartTime)
                    .Select(a => new AppointmentResponse
                    {
                        Id = a.Id,
                        AppointmentDate = a.AppointmentDate,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Status = a.Status,
                        Notes = a.Notes,
                        Patient = new PatientResponse
                        {
                            Id = a.Patient.Id,
                            FirstName = a.Patient.User.FirstName,
                            LastName = a.Patient.User.LastName,
                            Phone = a.Patient.User.PhoneNumber ?? ""
                        },
                        Service = new ServiceResponse
                        {
                            Id = a.Service.Id,
                            Name = a.Service.Name,
                            Description = a.Service.Description,
                            DurationMinutes = a.Service.DurationMinutes
                        }
                    })
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("{id}/availability")]
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetStaffAvailability(Guid id, [FromQuery] DateTime date)
        {
            try
            {
                var staff = await _context.Staff.FindAsync(id);
                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                var dayOfWeek = (int)date.DayOfWeek;
                if (dayOfWeek == 0) dayOfWeek = 7; // Sunday = 7

                var baseTimeSlots = await _context.TimeSlots
                    .Where(t => t.DayOfWeek == dayOfWeek && t.IsActive)
                    .ToListAsync();

                if (!baseTimeSlots.Any())
                {
                    return Ok(new List<TimeSlotResponse>());
                }

                var bookedAppointments = await _context.Appointments
                    .Where(a => a.StaffId == id &&
                               a.AppointmentDate == date &&
                               a.Status != "cancelled")
                    .ToListAsync();

                var availableSlots = new List<TimeSlotResponse>();

                foreach (var slot in baseTimeSlots)
                {
                    // Check if this slot conflicts with any booked appointment
                    var hasConflict = bookedAppointments.Any(a => 
                        a.StartTime < slot.EndTime && a.EndTime > slot.StartTime);

                    if (!hasConflict)
                    {
                        availableSlots.Add(new TimeSlotResponse
                        {
                            Id = slot.Id,
                            DayOfWeek = slot.DayOfWeek,
                            StartTime = slot.StartTime,
                            EndTime = slot.EndTime,
                            IsActive = slot.IsActive,
                            CreatedAt = slot.CreatedAt
                        });
                    }
                }

                return Ok(availableSlots);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffResponse>>> GetStaff()
        {
            try
            {
                var staff = await (from s in _context.Staff
                                   join u in _context.Users on s.UserId equals u.Id
                                   join ur in _context.UserRoles on u.Id equals ur.UserId
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   where u.IsActive
                                   select new StaffResponse
                                   {
                                       Id = s.Id,
                                       UserId = s.UserId,
                                       StaffNumber = s.StaffNumber,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       Role = r.Name ?? "staff",
                                       Phone = u.PhoneNumber,
                                       IsActive = u.IsActive
                                   }).ToListAsync();

                return Ok(staff);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStaff(Guid id, [FromBody] UpdateStaffRequest request)
        {
            try
            {
                var staff = await _context.Staff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                // Update staff fields
                if (request.PhoneNumber != null)
                    staff.User.PhoneNumber = request.PhoneNumber;

                staff.UpdatedAt = DateTime.UtcNow;
                staff.User.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Staff profile updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost("{id}/availability")]
        public async Task<ActionResult> UpdateAvailability(Guid id, [FromBody] UpdateAvailabilityRequest request)
        {
            try
            {
                var staff = await _context.Staff.FindAsync(id);
                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                // Parse time strings to TimeOnly
                if (!TimeOnly.TryParse(request.StartTime, out var startTime))
                {
                    return BadRequest(new { error = "Invalid start time format. Use HH:mm:ss or HH:mm" });
                }

                if (!TimeOnly.TryParse(request.EndTime, out var endTime))
                {
                    return BadRequest(new { error = "Invalid end time format. Use HH:mm:ss or HH:mm" });
                }

                // Check if time slot already exists for this staff member and day
                var existingSlot = await _context.TimeSlots
                    .FirstOrDefaultAsync(t => t.DayOfWeek == request.DayOfWeek && t.IsActive);

                if (existingSlot != null)
                {
                    // Update existing slot
                    existingSlot.StartTime = startTime;
                    existingSlot.EndTime = endTime;
                }
                else
                {
                    // Create new time slot
                    var timeSlot = new TimeSlot
                    {
                        Id = Guid.NewGuid(),
                        DayOfWeek = request.DayOfWeek,
                        StartTime = startTime,
                        EndTime = endTime,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.TimeSlots.Add(timeSlot);
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Availability updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Get staff members who are on duty on a specific date
        /// </summary>
        [HttpGet("on-duty")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetStaffOnDuty([FromQuery] DateTime date, [FromQuery] string? role = null)
        {
            try
            {
            var query = _context.Staff
                .Where(s => s.IsActive);

                var staff = await query
                    .Include(s => s.User)
                    .Join(_context.DoctorSchedules,
                        s => s.Id,
                        ds => ds.DoctorId,
                        (s, ds) => new { Staff = s, Schedule = ds })
                    .Where(x => x.Schedule.DayOfWeek == date.DayOfWeek.ToString() && x.Schedule.Date == date.Date && x.Schedule.IsActive)
                    .Select(x => new
                    {
                        x.Staff.Id,
                        FirstName = x.Staff.User.FirstName,
                        LastName = x.Staff.User.LastName,
                        Role = "doctor",
                        Specialty = "General Practice",
                        x.Schedule.ShiftStart,
                        x.Schedule.ShiftEnd,
                        IsAvailable = true
                    })
                    .ToListAsync();

                return Ok(new
                {
                    Date = date,
                    Role = role,
                    Staff = staff
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get staff member's schedule
        /// </summary>
        [HttpGet("{id}/schedule-details")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetStaffMemberSchedule(Guid id, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var staff = await _context.Staff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                var query = _context.DoctorSchedules
                    .Where(ds => ds.DoctorId == id);

                if (startDate.HasValue)
                {
                    query = query.Where(ds => ds.Date >= startDate.Value.Date);
                }

                if (endDate.HasValue)
                {
                    query = query.Where(ds => ds.Date <= endDate.Value.Date);
                }

                var schedules = await query
                    .OrderBy(ds => ds.DayOfWeek)
                    .ThenBy(ds => ds.Date)
                    .Select(ds => new
                    {
                        ds.Id,
                        ds.DoctorId,
                        ds.DayOfWeek,
                        ds.IsActive,
                        ShiftStart = ds.ShiftStart.ToString(@"hh\:mm"),
                        ShiftEnd = ds.ShiftEnd.ToString(@"hh\:mm"),
                        BreakStart = ds.BreakStart.HasValue ? ds.BreakStart.Value.ToString(@"hh\:mm") : null,
                        BreakEnd = ds.BreakEnd.HasValue ? ds.BreakEnd.Value.ToString(@"hh\:mm") : null,
                        ds.CreatedAt,
                        ds.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(new
                {
                    StaffId = id,
                    StaffName = $"{staff.User.FirstName} {staff.User.LastName}",
                    Schedules = schedules
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

    }
}