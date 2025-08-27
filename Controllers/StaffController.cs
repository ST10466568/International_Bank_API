using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public StaffController(HopewellDbContext context)
        {
            _context = context;
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
    }
}