using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.Models;
using HopewellClinicApi.DTOs;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public AppointmentsController(HopewellDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments()
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
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
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentResponse>> GetAppointment(Guid id)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                var response = new AppointmentResponse
                {
                    Id = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    StartTime = appointment.StartTime,
                    EndTime = appointment.EndTime,
                    Status = appointment.Status,
                    Notes = appointment.Notes,
                    Service = new ServiceResponse
                    {
                        Id = appointment.Service.Id,
                        Name = appointment.Service.Name,
                        Description = appointment.Service.Description,
                        DurationMinutes = appointment.Service.DurationMinutes
                    },
                    Patient = appointment.Patient != null ? new PatientResponse
                    {
                        Id = appointment.Patient.Id,
                        FirstName = appointment.Patient.User.FirstName,
                        LastName = appointment.Patient.User.LastName,
                        Phone = appointment.Patient.User.PhoneNumber ?? ""
                    } : null,
                    Staff = appointment.Staff != null ? new StaffResponse
                    {
                        Id = appointment.Staff.Id,
                        UserId = appointment.Staff.UserId,
                        StaffNumber = appointment.Staff.StaffNumber,
                        FirstName = appointment.Staff.User.FirstName,
                        LastName = appointment.Staff.User.LastName,
                        Role = "staff",
                        Phone = appointment.Staff.User.PhoneNumber,
                        IsActive = appointment.Staff.User.IsActive
                    } : null
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentResponse>> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentRequest request)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                // Update appointment fields
                if (request.AppointmentDate.HasValue)
                    appointment.AppointmentDate = request.AppointmentDate.Value;
                if (request.StartTime.HasValue)
                    appointment.StartTime = request.StartTime.Value;
                if (request.EndTime.HasValue)
                    appointment.EndTime = request.EndTime.Value;
                if (request.Notes != null)
                    appointment.Notes = request.Notes;
                if (request.Status != null)
                    appointment.Status = request.Status;

                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Appointment updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CancelAppointment(Guid id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                appointment.Status = "cancelled";
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Appointment cancelled successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost("{id}/assign-staff")]
        public async Task<ActionResult> AssignStaff(Guid id, [FromBody] AssignStaffRequest request)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                var staff = await _context.Staff.FindAsync(request.StaffId);
                if (staff == null)
                {
                    return NotFound(new { error = "Staff member not found" });
                }

                appointment.StaffId = request.StaffId;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Staff assigned successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("available-slots")]
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetAvailableSlots()
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
                        IsActive = t.IsActive
                    })
                    .ToListAsync();

                return Ok(timeSlots);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointmentsByPatient(Guid patientId)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Where(a => a.PatientId == patientId)
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
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
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentResponse>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                // Validate patient exists
                var patient = await _context.Patients.FindAsync(request.PatientId);
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

                // Check if the time slot is available
                var conflictingAppointment = await _context.Appointments
                    .Where(a => a.AppointmentDate == request.AppointmentDate &&
                               a.Status != "cancelled" &&
                               ((a.StartTime <= request.StartTime && a.EndTime > request.StartTime) ||
                                (a.StartTime < request.EndTime && a.EndTime >= request.EndTime) ||
                                (a.StartTime >= request.StartTime && a.EndTime <= request.EndTime)))
                    .FirstOrDefaultAsync();

                if (conflictingAppointment != null)
                {
                    return BadRequest(new { error = "Time slot is not available" });
                }

                var appointment = new Appointment
                {
                    PatientId = request.PatientId,
                    ServiceId = request.ServiceId,
                    AppointmentDate = request.AppointmentDate,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Notes = request.Notes,
                    Status = "pending"
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Return the created appointment
                var createdAppointment = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                    .Include(a => a.Staff)
                    .FirstOrDefaultAsync(a => a.Id == appointment.Id);

                if (createdAppointment == null)
                {
                    return StatusCode(500, new { error = "Failed to retrieve created appointment" });
                }

                var response = new AppointmentResponse
                {
                    Id = createdAppointment.Id,
                    AppointmentDate = createdAppointment.AppointmentDate,
                    StartTime = createdAppointment.StartTime,
                    EndTime = createdAppointment.EndTime,
                    Status = createdAppointment.Status,
                    Notes = createdAppointment.Notes,
                    Service = new ServiceResponse
                    {
                        Id = createdAppointment.Service.Id,
                        Name = createdAppointment.Service.Name,
                        Description = createdAppointment.Service.Description,
                        DurationMinutes = createdAppointment.Service.DurationMinutes
                    },
                    Patient = new PatientResponse
                    {
                        Id = createdAppointment.Patient.Id,
                        FirstName = createdAppointment.Patient.User.FirstName,
                        LastName = createdAppointment.Patient.User.LastName,
                        Phone = createdAppointment.Patient.User.PhoneNumber ?? ""
                    },
                    Staff = null    // New appointment doesn't have staff assigned yet
                };

                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}
