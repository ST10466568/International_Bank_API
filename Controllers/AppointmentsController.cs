using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HopewellClinicApi.Data;
using HopewellClinicApi.Models;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Attributes;
using HopewellClinicApi.Services;

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

        /// <summary>
        /// Get appointments for a specific doctor
        /// </summary>
        [HttpGet("doctor/{doctorId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctorAppointments(string doctorId)
        {
            try
            {
                if (string.IsNullOrEmpty(doctorId))
                {
                    return BadRequest("Doctor ID is required");
                }

                if (!Guid.TryParse(doctorId, out var doctorGuid))
                {
                    return BadRequest("Invalid doctor ID format");
                }

                var appointments = await _context.Appointments
                    .Where(a => (a.StaffId == doctorGuid || a.DoctorId == doctorGuid))
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.User)
                    .OrderBy(a => a.AppointmentDate)
                    .ThenBy(a => a.StartTime)
                    .ToListAsync();

                var appointmentResults = appointments.Select(a => new
                {
                    id = a.Id,
                    appointmentDate = a.AppointmentDate.ToString("yyyy-MM-dd"),
                    startTime = a.StartTime.ToString("HH:mm:ss"),
                    endTime = a.EndTime.ToString("HH:mm:ss"),
                    status = a.Status,
                    notes = a.Notes,
                    patient = new
                    {
                        id = a.Patient.Id,
                        firstName = a.Patient.User.FirstName,
                        lastName = a.Patient.User.LastName,
                        email = a.Patient.User.Email,
                        phone = a.Patient.User.PhoneNumber,
                        dateOfBirth = a.Patient.DateOfBirth.HasValue ? a.Patient.DateOfBirth.Value.ToString("yyyy-MM-dd") : null
                    },
                    service = new
                    {
                        id = a.Service.Id,
                        name = a.Service.Name,
                        durationMinutes = a.Service.DurationMinutes
                    }
                }).ToList();

                return Ok(appointmentResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet]
        [JwtAuthorize]
        public async Task<ActionResult<IEnumerable<AppointmentResponse>>> GetAppointments()
        {
            try
            {
                var appointments = await _context.Appointments
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
                        .ThenInclude(p => p.User)
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.User)
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
        public async Task<ActionResult<IEnumerable<TimeSlotResponse>>> GetAvailableSlots(
            [FromQuery] DateTime date,
            [FromQuery] Guid? serviceId = null)
        {
            try
            {
                // Get all active time slots
                var allTimeSlots = await _context.TimeSlots
                    .Where(t => t.IsActive)
                    .ToListAsync();

                // Get existing confirmed appointments for that date
                var existingAppointments = await _context.Appointments
                    .Where(a => a.AppointmentDate == date && a.Status == "confirmed")
                    .ToListAsync();

                // Filter out booked slots
                var availableSlots = allTimeSlots
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting available slots: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
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
                    .ToListAsync();

                return Ok(appointments);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost]
        [JwtAuthorize]
        public async Task<ActionResult<AppointmentResponse>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                // Parse time strings to TimeOnly
                if (!TimeOnly.TryParse(request.StartTime, out var startTime))
                {
                    return BadRequest(new { error = "Invalid start time format. Use HH:mm:ss or HH:mm" });
                }

                TimeOnly endTime;
                if (string.IsNullOrEmpty(request.EndTime))
                {
                    // Calculate end time based on service duration
                    var service = await _context.Services
                        .FirstOrDefaultAsync(s => s.Id == request.ServiceId);
                    if (service == null)
                    {
                        return BadRequest(new { error = "Service not found" });
                    }
                    endTime = startTime.Add(TimeSpan.FromMinutes(service.DurationMinutes));
                }
                else
                {
                    if (!TimeOnly.TryParse(request.EndTime, out endTime))
                    {
                        return BadRequest(new { error = "Invalid end time format. Use HH:mm:ss or HH:mm" });
                    }
                }

                // Validate patient exists
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.PatientId);
                if (patient == null)
                {
                    return BadRequest(new { error = "Patient not found" });
                }

                // Service validation already done above when calculating end time

                // Check if the time slot is available
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

                // Create the appointment with ALL required fields
                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(), // ✅ Add the missing Id field
                    PatientId = request.PatientId,
                    ServiceId = request.ServiceId,
                    AppointmentDate = request.AppointmentDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Notes = request.Notes,
                    Status = "pending",
                    BookingType = "online", // ✅ Add the missing BookingType field
                    CreatedAt = DateTime.UtcNow, // ✅ Add the missing CreatedAt field
                    UpdatedAt = DateTime.UtcNow  // ✅ Add the missing UpdatedAt field
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Return the created appointment with proper includes
                var createdAppointment = await _context.Appointments
                    .Include(a => a.Service)
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User) // ✅ Include User for Patient
                    .Include(a => a.Staff)
                        .ThenInclude(s => s.User) // ✅ Include User for Staff
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
                        DurationMinutes = createdAppointment.Service.DurationMinutes,
                        IsActive = createdAppointment.Service.IsActive,
                        CreatedAt = createdAppointment.Service.CreatedAt,
                        UpdatedAt = createdAppointment.Service.UpdatedAt
                    },
                    Patient = new PatientResponse
                    {
                        Id = createdAppointment.Patient.Id,
                        FirstName = createdAppointment.Patient.User.FirstName,
                        LastName = createdAppointment.Patient.User.LastName,
                        Phone = createdAppointment.Patient.User.PhoneNumber ?? ""
                    },
                    Staff = null // New appointment doesn't have staff assigned yet
                };

                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, response);
            }
            catch (Exception ex)
            {
                // ✅ Better error logging
                Console.WriteLine($"Error creating appointment: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateAppointmentStatus(Guid id, [FromBody] UpdateStatusDto request)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                appointment.Status = request.Status;
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Appointment status updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("today")]
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

        // NURSE DASHBOARD ENDPOINTS

        [HttpPost("walkin")]
        [AuthorizeNurse]
        public async Task<ActionResult<WalkInAppointmentResponse>> CreateWalkInAppointment([FromBody] WalkInAppointmentDto dto)
        {
            try
            {
                // Validate service exists
                var service = await _context.Services.FindAsync(dto.ServiceId);
                if (service == null)
                {
                    return BadRequest(new { error = "Service not found" });
                }

                // Validate doctor exists
                var doctor = await _context.Staff.FindAsync(dto.DoctorId);
                if (doctor == null)
                {
                    return BadRequest(new { error = "Doctor not found" });
                }

                // Check if patient already exists by email
                var existingPatient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.User.Email == dto.PatientEmail);

                Patient patient;
                if (existingPatient == null)
                {
                    // Create new patient
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = dto.PatientEmail,
                        Email = dto.PatientEmail,
                        FirstName = dto.PatientFirstName,
                        LastName = dto.PatientLastName,
                        PhoneNumber = dto.PatientPhone,
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

                // Auto-assign current date and time
                var now = DateTime.Now;
                var appointmentDate = now.Date;
                var startTime = new TimeOnly(now.Hour, now.Minute);
                var endTime = startTime.AddMinutes(service.DurationMinutes);

                // Check for time conflicts
                var conflictingAppointment = await _context.Appointments
                    .Where(a => a.AppointmentDate == appointmentDate &&
                               a.StaffId == dto.DoctorId &&
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
                    ServiceId = dto.ServiceId,
                    StaffId = dto.DoctorId,
                    AppointmentDate = appointmentDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = "walkin",
                    BookingType = "walkin",
                    IsWalkIn = true,
                    ServicePrice = service.Price,
                    PaymentStatus = "pending",
                    Notes = dto.Notes,
                    ApprovalStatus = ApprovalStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(new WalkInAppointmentResponse
                {
                    AppointmentId = appointment.Id,
                    Message = "Walk-in appointment created successfully",
                    PatientId = patient.Id,
                    PatientNumber = patient.PatientNumber
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating walk-in appointment: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("{id}/approve-for-doctor")]
        [AuthorizeNurse]
        public async Task<ActionResult<NurseApprovalResponse>> ApproveForDoctor(Guid id, [FromBody] DoctorApprovalDto dto)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return NotFound(new { error = "Appointment not found" });
                }

                // Validate doctor exists
                var doctor = await _context.Staff.FindAsync(dto.DoctorId);
                if (doctor == null)
                {
                    return BadRequest(new { error = "Doctor not found" });
                }

                // Check for time conflicts with the assigned doctor
                var conflictingAppointment = await _context.Appointments
                    .Where(a => a.Id != id &&
                               a.AppointmentDate == appointment.AppointmentDate &&
                               a.StaffId == dto.DoctorId &&
                               a.Status != "cancelled" &&
                               ((a.StartTime <= appointment.StartTime && a.EndTime > appointment.StartTime) ||
                                (a.StartTime < appointment.EndTime && a.EndTime >= appointment.EndTime) ||
                                (a.StartTime >= appointment.StartTime && a.EndTime <= appointment.EndTime)))
                    .FirstOrDefaultAsync();

                if (conflictingAppointment != null)
                {
                    return BadRequest(new { error = "Doctor is not available at this time" });
                }

                // Update appointment with nurse approval
                appointment.StaffId = dto.DoctorId;
                appointment.Status = "confirmed";
                appointment.ApprovalStatus = ApprovalStatus.Approved;
                appointment.ApprovedByNurseId = User.Identity?.Name; // Get current nurse ID
                appointment.NurseApprovalDate = DateTime.UtcNow;
                appointment.ApprovedAt = DateTime.UtcNow;
                appointment.ApprovedBy = dto.DoctorId.ToString();
                if (!string.IsNullOrEmpty(dto.ApprovalNotes))
                {
                    appointment.ApprovalNotes = dto.ApprovalNotes;
                }
                appointment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new NurseApprovalResponse
                {
                    Message = "Appointment approved and assigned to doctor successfully",
                    AppointmentId = appointment.Id,
                    Status = appointment.Status
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Get available time slots by doctor - Frontend compatible endpoint
        /// </summary>
        [HttpGet("available-slots-by-doctor")]
        public async Task<ActionResult<object>> GetAvailableSlotsByDoctor(
            [FromQuery] Guid doctorId,
            [FromQuery] DateTime date,
            [FromQuery] Guid? serviceId = null)
        {
            try
            {
                // Use the BookingService to get slots from doctor schedule
                var bookingService = HttpContext.RequestServices.GetRequiredService<HopewellClinicApi.Services.BookingService>();
                var response = await bookingService.GetAvailableSlotsByDoctorAsync(doctorId, date, serviceId);
                
                return Ok(new
                {
                    doctorId = response.DoctorId,
                    date = response.Date,
                    slots = response.AvailableSlots.Select(s => new
                    {
                        id = s.Id,
                        startTime = s.StartTime.ToString(@"hh\:mm\:ss"),
                        endTime = s.EndTime.ToString(@"hh\:mm\:ss"),
                        duration = s.Duration,
                        isAvailable = s.IsAvailable,
                        doctorId = s.DoctorId
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        /// <summary>
        /// Enhanced endpoint: Get available time slots with doctor filtering
        /// </summary>
        [HttpGet("available-slots-enhanced")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetAvailableSlots(
            [FromQuery] DateTime date,
            [FromQuery] Guid? doctorId = null,
            [FromQuery] Guid? serviceId = null)
        {
            try
            {
                var query = _context.TimeSlots
                    .Where(ts => ts.IsActive);

                if (doctorId.HasValue)
                {
                    query = query.Where(ts => ts.DoctorId == doctorId.Value);
                }

                if (date != default)
                {
                    query = query.Where(ts => ts.Date == date.Date);
                }

                var slots = await query
                    .Select(ts => new
                    {
                        ts.Id,
                        ts.StartTime,
                        ts.EndTime,
                        ts.Duration,
                        ts.IsAvailable,
                        ts.DoctorId,
                        Doctor = ts.Doctor != null ? new
                        {
                            ts.Doctor.Id,
                            FirstName = ts.Doctor.User.FirstName,
                            LastName = ts.Doctor.User.LastName,
                            Specialty = "General Practice"
                        } : null
                    })
                    .ToListAsync();

                return Ok(new
                {
                    Date = date,
                    DoctorId = doctorId,
                    ServiceId = serviceId,
                    Slots = slots
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Enhanced endpoint: Get doctors on duty for a specific date
        /// </summary>
        [HttpGet("doctors-on-duty")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> GetDoctorsOnDuty([FromQuery] DateTime date, [FromQuery] Guid? serviceId = null)
        {
            try
            {
                var doctors = await _context.Staff
                    .Include(s => s.User)
                    .Where(s => s.IsActive)
                    .Join(_context.DoctorSchedules,
                        s => s.Id,
                        ds => ds.DoctorId,
                        (s, ds) => new { Staff = s, Schedule = ds })
                    .Where(x => x.Schedule.Date == date.Date && x.Schedule.IsActive)
                    .Select(x => new
                    {
                        x.Staff.Id,
                        FirstName = x.Staff.User.FirstName,
                        LastName = x.Staff.User.LastName,
                        Specialty = "General Practice",
                        x.Schedule.ShiftStart,
                        x.Schedule.ShiftEnd,
                        IsAvailable = true
                    })
                    .ToListAsync();

                return Ok(new
                {
                    Date = date,
                    ServiceId = serviceId,
                    Doctors = doctors
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}