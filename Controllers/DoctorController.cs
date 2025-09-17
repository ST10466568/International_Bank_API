using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Models;
using HopewellClinicApi.Attributes;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeDoctor]
    public class DoctorController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public DoctorController(HopewellDbContext context)
        {
            _context = context;
        }

        [HttpGet("{doctorId}/patients")]
        public async Task<ActionResult<IEnumerable<PatientSummaryDto>>> GetDoctorPatients(Guid doctorId)
        {
            try
            {
                // Verify doctor exists
                var doctor = await _context.Staff
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.Id == doctorId);

                if (doctor == null)
                {
                    return NotFound(new { error = "Doctor not found" });
                }

                // Get patients who have appointments with this doctor
                var patients = await _context.Appointments
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Where(a => a.StaffId == doctorId)
                    .Select(a => new PatientSummaryDto
                    {
                        Id = a.Patient.Id,
                        FirstName = a.Patient.User.FirstName,
                        LastName = a.Patient.User.LastName,
                        PatientNumber = a.Patient.PatientNumber,
                        Phone = a.Patient.User.PhoneNumber,
                        Email = a.Patient.User.Email
                    })
                    .Distinct()
                    .ToListAsync();

                return Ok(patients);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("{doctorId}/appointments/upcoming")]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetUpcomingAppointments(Guid doctorId)
        {
            try
            {
                // Verify doctor exists
                var doctor = await _context.Staff.FindAsync(doctorId);
                if (doctor == null)
                {
                    return NotFound(new { error = "Doctor not found" });
                }

                var today = DateTime.Today;
                var appointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.User)
                    .Where(a => a.StaffId == doctorId && a.AppointmentDate >= today && a.Status != "cancelled")
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

        [HttpPost("appointments/walkin")]
        public async Task<ActionResult> CreateWalkinAppointment([FromBody] CreateWalkinAppointmentDto request)
        {
            try
            {
                // Parse time string to TimeOnly
                if (!TimeOnly.TryParse(request.StartTime, out var startTime))
                {
                    return BadRequest(new { error = "Invalid start time format. Use HH:mm:ss or HH:mm" });
                }

                // Validate doctor exists
                var doctor = await _context.Staff.FindAsync(request.DoctorId);
                if (doctor == null)
                {
                    return BadRequest(new { error = "Doctor not found" });
                }

                // Validate service exists
                var service = await _context.Services.FindAsync(request.ServiceId);
                if (service == null)
                {
                    return BadRequest(new { error = "Service not found" });
                }

                // Check if patient already exists by phone number
                var existingPatient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.User.PhoneNumber == request.PatientPhone);

                Patient patient;
                if (existingPatient == null)
                {
                    // Create new patient
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = $"{request.PatientFirstName.ToLower()}.{request.PatientLastName.ToLower()}@walkin.local",
                        Email = $"{request.PatientFirstName.ToLower()}.{request.PatientLastName.ToLower()}@walkin.local",
                        FirstName = request.PatientFirstName,
                        LastName = request.PatientLastName,
                        PhoneNumber = request.PatientPhone,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(user);

                    patient = new Patient
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        PatientNumber = $"WALK{DateTime.Now:yyyyMMdd}{user.Id.ToString().Substring(0, 4).ToUpper()}",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Patients.Add(patient);
                }
                else
                {
                    patient = existingPatient;
                }

                // Calculate end time based on service duration
                var endTime = startTime.AddMinutes(service.DurationMinutes);

                // Check for time conflicts
                var conflictingAppointment = await _context.Appointments
                    .Where(a => a.AppointmentDate == request.AppointmentDate &&
                               a.StaffId == request.DoctorId &&
                               a.Status != "cancelled" &&
                               ((a.StartTime <= startTime && a.EndTime > startTime) ||
                                (a.StartTime < endTime && a.EndTime >= endTime) ||
                                (a.StartTime >= startTime && a.EndTime <= endTime)))
                    .FirstOrDefaultAsync();

                if (conflictingAppointment != null)
                {
                    return BadRequest(new { error = "Doctor is not available at this time" });
                }

                // Create the walk-in appointment
                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    PatientId = patient.Id,
                    ServiceId = request.ServiceId,
                    StaffId = request.DoctorId,
                    AppointmentDate = request.AppointmentDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = "confirmed",
                    BookingType = "walkin",
                    Notes = "Walk-in appointment",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Walk-in appointment created successfully", appointmentId = appointment.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating walk-in appointment: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        // NEW ENDPOINTS FOR DOCTOR DASHBOARD

        // Doctor Shift Management
        [HttpGet("{doctorId}/shifts")]
        public async Task<ActionResult<IEnumerable<DoctorShiftResponse>>> GetDoctorShifts(Guid doctorId)
        {
            try
            {
                var shifts = await _context.DoctorShifts
                    .Where(ds => ds.DoctorId == doctorId)
                    .OrderBy(ds => ds.DayOfWeek)
                    .ThenBy(ds => ds.StartTime)
                    .Select(ds => new DoctorShiftResponse
                    {
                        Id = ds.Id,
                        DoctorId = ds.DoctorId,
                        DayOfWeek = ds.DayOfWeek,
                        StartTime = ds.StartTime,
                        EndTime = ds.EndTime,
                        IsActive = ds.IsActive,
                        CreatedAt = ds.CreatedAt,
                        UpdatedAt = ds.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(shifts);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPut("{doctorId}/shifts")]
        public async Task<ActionResult> UpdateDoctorShifts(Guid doctorId, [FromBody] List<CreateDoctorShiftRequest> shifts)
        {
            try
            {
                // Remove existing shifts for this doctor
                var existingShifts = await _context.DoctorShifts
                    .Where(ds => ds.DoctorId == doctorId)
                    .ToListAsync();

                _context.DoctorShifts.RemoveRange(existingShifts);

                // Add new shifts
                foreach (var shiftRequest in shifts)
                {
                    var shift = new DoctorShift
                    {
                        DoctorId = doctorId,
                        DayOfWeek = shiftRequest.DayOfWeek,
                        StartTime = shiftRequest.StartTime,
                        EndTime = shiftRequest.EndTime,
                        IsActive = shiftRequest.IsActive,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.DoctorShifts.Add(shift);
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Doctor shifts updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // Appointment Approval System
        [HttpPut("appointments/{appointmentId}/approve")]
        public async Task<ActionResult> ApproveAppointment(Guid appointmentId, [FromBody] ApproveAppointmentRequest request)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(appointmentId);
                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                appointment.ApprovalStatus = ApprovalStatus.Approved;
                appointment.Status = "confirmed";
                appointment.ApprovedAt = DateTime.UtcNow;
                appointment.ApprovedBy = User.Identity?.Name; // Get current user ID
                if (!string.IsNullOrEmpty(request.Notes))
                {
                    appointment.Notes = request.Notes;
                }
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Appointment approved successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPut("appointments/{appointmentId}/reject")]
        public async Task<ActionResult> RejectAppointment(Guid appointmentId, [FromBody] RejectAppointmentRequest request)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(appointmentId);
                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                appointment.ApprovalStatus = ApprovalStatus.Rejected;
                appointment.Status = "cancelled";
                appointment.RejectionReason = request.RejectionReason;
                appointment.ApprovedAt = DateTime.UtcNow;
                appointment.ApprovedBy = User.Identity?.Name; // Get current user ID
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Appointment rejected successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // Enhanced Doctor Appointments
        [HttpGet("{doctorId}/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentWithApprovalResponse>>> GetAllDoctorAppointments(Guid doctorId)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.User)
                    .Where(a => a.StaffId == doctorId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ThenBy(a => a.StartTime)
                    .Select(a => new AppointmentWithApprovalResponse
                    {
                        Id = a.Id,
                        AppointmentDate = a.AppointmentDate,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Status = a.Status,
                        ApprovalStatus = a.ApprovalStatus,
                        RejectionReason = a.RejectionReason,
                        ApprovedAt = a.ApprovedAt,
                        ApprovedBy = a.ApprovedBy,
                        Notes = a.Notes,
                        BookingType = a.BookingType,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt,
                        Service = new ServiceResponse
                        {
                            Id = a.Service.Id,
                            Name = a.Service.Name,
                            Description = a.Service.Description,
                            DurationMinutes = a.Service.DurationMinutes
                        },
                        Patient = new PatientResponse
                        {
                            Id = a.Patient.Id,
                            FirstName = a.Patient.User.FirstName,
                            LastName = a.Patient.User.LastName,
                            Phone = a.Patient.User.PhoneNumber ?? ""
                        },
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

        [HttpGet("{doctorId}/schedule")]
        public async Task<ActionResult<DoctorScheduleResponse>> GetDoctorSchedule(Guid doctorId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var appointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.User)
                    .Where(a => a.StaffId == doctorId && 
                               a.AppointmentDate >= startDate && 
                               a.AppointmentDate <= endDate)
                    .OrderBy(a => a.AppointmentDate)
                    .ThenBy(a => a.StartTime)
                    .Select(a => new AppointmentWithApprovalResponse
                    {
                        Id = a.Id,
                        AppointmentDate = a.AppointmentDate,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Status = a.Status,
                        ApprovalStatus = a.ApprovalStatus,
                        RejectionReason = a.RejectionReason,
                        ApprovedAt = a.ApprovedAt,
                        ApprovedBy = a.ApprovedBy,
                        Notes = a.Notes,
                        BookingType = a.BookingType,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt,
                        Service = new ServiceResponse
                        {
                            Id = a.Service.Id,
                            Name = a.Service.Name,
                            Description = a.Service.Description,
                            DurationMinutes = a.Service.DurationMinutes
                        },
                        Patient = new PatientResponse
                        {
                            Id = a.Patient.Id,
                            FirstName = a.Patient.User.FirstName,
                            LastName = a.Patient.User.LastName,
                            Phone = a.Patient.User.PhoneNumber ?? ""
                        },
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

                var shifts = await _context.DoctorShifts
                    .Where(ds => ds.DoctorId == doctorId && ds.IsActive)
                    .Select(ds => new DoctorShiftResponse
                    {
                        Id = ds.Id,
                        DoctorId = ds.DoctorId,
                        DayOfWeek = ds.DayOfWeek,
                        StartTime = ds.StartTime,
                        EndTime = ds.EndTime,
                        IsActive = ds.IsActive,
                        CreatedAt = ds.CreatedAt,
                        UpdatedAt = ds.UpdatedAt
                    })
                    .ToListAsync();

                var response = new DoctorScheduleResponse
                {
                    Date = startDate,
                    Appointments = appointments,
                    Shifts = shifts
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        // Patient Details for Doctors
        [HttpGet("patients/{patientId}")]
        public async Task<ActionResult<PatientDetailsForDoctorResponse>> GetPatientDetailsForDoctor(Guid patientId)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .Include(p => p.Appointments)
                        .ThenInclude(a => a.Service)
                    .Include(p => p.Appointments)
                        .ThenInclude(a => a.Staff)
                            .ThenInclude(s => s.User)
                    .FirstOrDefaultAsync(p => p.Id == patientId);

                if (patient == null)
                {
                    return NotFound(new { error = "Patient not found" });
                }

                var appointmentHistory = patient.Appointments
                    .OrderByDescending(a => a.AppointmentDate)
                    .Select(a => new AppointmentWithApprovalResponse
                    {
                        Id = a.Id,
                        AppointmentDate = a.AppointmentDate,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        Status = a.Status,
                        ApprovalStatus = a.ApprovalStatus,
                        RejectionReason = a.RejectionReason,
                        ApprovedAt = a.ApprovedAt,
                        ApprovedBy = a.ApprovedBy,
                        Notes = a.Notes,
                        BookingType = a.BookingType,
                        CreatedAt = a.CreatedAt,
                        UpdatedAt = a.UpdatedAt,
                        Service = new ServiceResponse
                        {
                            Id = a.Service.Id,
                            Name = a.Service.Name,
                            Description = a.Service.Description,
                            DurationMinutes = a.Service.DurationMinutes
                        },
                        Patient = new PatientResponse
                        {
                            Id = a.Patient.Id,
                            FirstName = a.Patient.User.FirstName,
                            LastName = a.Patient.User.LastName,
                            Phone = a.Patient.User.PhoneNumber ?? ""
                        },
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
                    .ToList();

                var response = new PatientDetailsForDoctorResponse
                {
                    Id = patient.Id,
                    PatientNumber = patient.PatientNumber,
                    FirstName = patient.User.FirstName,
                    LastName = patient.User.LastName,
                    Email = patient.User.Email ?? "",
                    Phone = patient.User.PhoneNumber ?? "",
                    DateOfBirth = patient.DateOfBirth ?? DateTime.MinValue,
                    Address = patient.Address,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactPhone = patient.EmergencyContactPhone,
                    CreatedAt = patient.CreatedAt,
                    UpdatedAt = patient.UpdatedAt,
                    AppointmentHistory = appointmentHistory
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
