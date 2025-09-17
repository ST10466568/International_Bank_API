using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Models;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NurseController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public NurseController(HopewellDbContext context)
        {
            _context = context;
        }

        [HttpGet("appointments/today")]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetTodaysAppointments()
        {
            try
            {
                var today = DateTime.Today;
                var appointments = await _context.Appointments
                    .Where(a => a.AppointmentDate == today)
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.User)
                    .Select(a => new AppointmentResponse
                    {
                        Id = a.Id,
                        AppointmentDate = a.AppointmentDate,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Status = a.Status,
                        Notes = a.Notes,
                        Service = new ServiceResponse
                        {
                            Id = a.Service.Id,
                            Name = a.Service.Name,
                            Description = a.Service.Description,
                            DurationMinutes = a.Service.DurationMinutes
                        },
                        Patient = a.Patient != null ? new PatientResponse
                        {
                            Id = a.Patient.Id,
                            FirstName = a.Patient.User.FirstName,
                            LastName = a.Patient.User.LastName,
                            Phone = a.Patient.User.PhoneNumber ?? ""
                        } : null,
                        Staff = a.Staff != null ? new StaffResponse
                        {
                            Id = a.Staff.Id,
                            UserId = a.Staff.UserId,
                            StaffNumber = a.Staff.StaffNumber,
                            FirstName = a.Staff.User.FirstName,
                            LastName = a.Staff.User.LastName,
                            Role = "staff",
                            Phone = a.Staff.User.PhoneNumber,
                            IsActive = a.Staff.User.IsActive
                        } : null
                    })
                    .OrderBy(a => a.StartTime)
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("patients/search")]
        public async Task<ActionResult<IEnumerable<PatientSummaryDto>>> SearchPatients([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new { error = "Search query is required" });
                }

                var patients = await _context.Patients
                    .Include(p => p.User)
                    .Where(p => p.User.FirstName.Contains(query) ||
                               p.User.LastName.Contains(query) ||
                               p.PatientNumber.Contains(query) ||
                               p.User.PhoneNumber.Contains(query))
                    .Select(p => new PatientSummaryDto
                    {
                        Id = p.Id,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        PatientNumber = p.PatientNumber,
                        Phone = p.User.PhoneNumber,
                        Email = p.User.Email
                    })
                    .ToListAsync();

                return Ok(patients);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost("appointments/book-for-patient")]
        public async Task<ActionResult> BookAppointmentForPatient([FromBody] BookAppointmentForPatientDto request)
        {
            try
            {
                // Parse time string to TimeOnly
                if (!TimeOnly.TryParse(request.StartTime, out var startTime))
                {
                    return BadRequest(new { error = "Invalid start time format. Use HH:mm:ss or HH:mm" });
                }

                // Validate patient exists
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.PatientId);
                if (patient == null)
                {
                    return BadRequest(new { error = "Patient not found" });
                }

                // Validate service exists
                var service = await _context.Services.FindAsync(request.ServiceId);
                if (service == null)
                {
                    return BadRequest(new { error = "Service not found" });
                }

                // Calculate end time based on service duration
                var endTime = startTime.AddMinutes(service.DurationMinutes);

                // Check for time conflicts
                var conflictingAppointment = await _context.Appointments
                    .Where(a => a.AppointmentDate == request.AppointmentDate &&
                               a.Status != "cancelled" &&
                               ((a.StartTime <= startTime && a.EndTime > startTime) ||
                                (a.StartTime < endTime && a.EndTime >= endTime) ||
                                (a.StartTime >= startTime && a.EndTime <= endTime)))
                    .FirstOrDefaultAsync();

                if (conflictingAppointment != null)
                {
                    return BadRequest(new { error = "Time slot is not available" });
                }

                // Create the appointment
                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    PatientId = request.PatientId,
                    ServiceId = request.ServiceId,
                    StaffId = request.StaffId,
                    AppointmentDate = request.AppointmentDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = "confirmed",
                    BookingType = "nurse_booking",
                    Notes = "Appointment booked by nurse",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Appointment booked successfully", appointmentId = appointment.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error booking appointment: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}
