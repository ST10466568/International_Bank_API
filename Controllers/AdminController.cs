using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Models;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly HopewellDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminController(HopewellDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("create-staff")]
        public async Task<ActionResult<UserCreationResponse>> CreateStaff([FromBody] CreateStaffRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new UserCreationResponse 
                    { 
                        Success = false, 
                        Error = "User with this email already exists" 
                    });
                }

                // Create new user
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new UserCreationResponse 
                    { 
                        Success = false, 
                        Error = "Failed to create user", 
                        Data = result.Errors 
                    });
                }

                // Assign role
                await _userManager.AddToRoleAsync(user, request.Role);

                // Create staff record
                var staff = new Staff
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    StaffNumber = $"STF{DateTime.Now:yyyyMMdd}{user.Id.ToString().Substring(0, 4).ToUpper()}",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Staff.Add(staff);
                await _context.SaveChangesAsync();

                return Ok(new UserCreationResponse 
                { 
                    Success = true, 
                    Message = "Staff user created successfully", 
                    Data = new { staffId = staff.Id, userId = user.Id } 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new UserCreationResponse 
                { 
                    Success = false, 
                    Error = "Internal server error: " + ex.Message 
                });
            }
        }

        [HttpPost("create-patient")]
        public async Task<ActionResult<UserCreationResponse>> CreatePatient([FromBody] CreatePatientRequest request)
        {
            try
            {
                // Validate date of birth format
                if (!DateTime.TryParse(request.DateOfBirth, out var dateOfBirth))
                {
                    return BadRequest(new UserCreationResponse 
                    { 
                        Success = false, 
                        Error = "Invalid date of birth format. Use YYYY-MM-DD" 
                    });
                }

                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new UserCreationResponse 
                    { 
                        Success = false, 
                        Error = "User with this email already exists" 
                    });
                }

                // Create new user
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.Phone,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new UserCreationResponse 
                    { 
                        Success = false, 
                        Error = "Failed to create user", 
                        Data = result.Errors 
                    });
                }

                // Assign role
                await _userManager.AddToRoleAsync(user, request.Role);

                // Create patient record
                var patient = new Patient
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    PatientNumber = $"PAT{DateTime.Now:yyyyMMdd}{user.Id.ToString().Substring(0, 4).ToUpper()}",
                    DateOfBirth = dateOfBirth,
                    Address = request.Address,
                    EmergencyContactName = request.EmergencyContact,
                    EmergencyContactPhone = request.EmergencyPhone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                var patientResponse = new PatientResponse
                {
                    Id = patient.Id,
                    UserId = user.Id,
                    PatientNumber = patient.PatientNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    DateOfBirth = patient.DateOfBirth,
                    Address = patient.Address,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactPhone = patient.EmergencyContactPhone,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(new UserCreationResponse 
                { 
                    Success = true, 
                    Message = "Patient created successfully", 
                    Data = patientResponse 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new UserCreationResponse 
                { 
                    Success = false, 
                    Error = "Internal server error: " + ex.Message 
                });
            }
        }

        [HttpPost("create-user")]
        public async Task<ActionResult<UserCreationResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new UserCreationResponse 
                    { 
                        Success = false, 
                        Error = "User with this email already exists" 
                    });
                }

                // Validate patient-specific fields if provided
                DateTime? dateOfBirth = null;
                if (!string.IsNullOrEmpty(request.DateOfBirth))
                {
                    if (!DateTime.TryParse(request.DateOfBirth, out var parsedDate))
                    {
                        return BadRequest(new UserCreationResponse 
                        { 
                            Success = false, 
                            Error = "Invalid date of birth format. Use YYYY-MM-DD" 
                        });
                    }
                    dateOfBirth = parsedDate;
                }

                // Create new user
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.Phone,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new UserCreationResponse 
                    { 
                        Success = false, 
                        Error = "Failed to create user", 
                        Data = result.Errors 
                    });
                }

                // Assign role
                await _userManager.AddToRoleAsync(user, request.Role);

                object? responseData = null;

                // Create appropriate record based on role
                if (request.Role.ToLower() == "patient" && !string.IsNullOrEmpty(request.DateOfBirth))
                {
                    // Create patient record
                    var patient = new Patient
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        PatientNumber = $"PAT{DateTime.Now:yyyyMMdd}{user.Id.ToString().Substring(0, 4).ToUpper()}",
                        DateOfBirth = dateOfBirth,
                        Address = request.Address,
                        EmergencyContactName = request.EmergencyContact,
                        EmergencyContactPhone = request.EmergencyPhone,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();

                    responseData = new PatientResponse
                    {
                        Id = patient.Id,
                        UserId = user.Id,
                        PatientNumber = patient.PatientNumber,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        DateOfBirth = patient.DateOfBirth,
                        Address = patient.Address,
                        EmergencyContactName = patient.EmergencyContactName,
                        EmergencyContactPhone = patient.EmergencyContactPhone,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt
                    };
                }
                else if (request.Role.ToLower() != "patient")
                {
                    // Create staff record
                    var staff = new Staff
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        StaffNumber = $"STF{DateTime.Now:yyyyMMdd}{user.Id.ToString().Substring(0, 4).ToUpper()}",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Staff.Add(staff);
                    await _context.SaveChangesAsync();

                    responseData = new { staffId = staff.Id, userId = user.Id };
                }

                return Ok(new UserCreationResponse 
                { 
                    Success = true, 
                    Message = $"User created successfully as {request.Role}", 
                    Data = responseData 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new UserCreationResponse 
                { 
                    Success = false, 
                    Error = "Internal server error: " + ex.Message 
                });
            }
        }

        [HttpPut("users/{userId}")]
        public async Task<ActionResult> UpdateUserStatus(Guid userId, [FromBody] UpdateStaffStatusRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }

                user.IsActive = request.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new { error = "Failed to update user status" });
                }

                return Ok(new { message = "User status updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("reports/appointment-stats")]
        public async Task<ActionResult<AppointmentStatsDto>> GetAppointmentStats([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var appointments = await _context.Appointments
                    .Where(a => a.AppointmentDate >= start && a.AppointmentDate <= end)
                    .ToListAsync();

                var stats = new AppointmentStatsDto
                {
                    StartDate = start,
                    EndDate = end,
                    TotalAppointments = appointments.Count,
                    CompletedAppointments = appointments.Count(a => a.Status == "completed"),
                    CancelledAppointments = appointments.Count(a => a.Status == "cancelled"),
                    PendingAppointments = appointments.Count(a => a.Status == "pending" || a.Status == "confirmed")
                };

                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Patient)
                    .Include(u => u.Staff)
                    .ToListAsync();

                var usersWithRoles = new List<object>();

                foreach (var user in users)
                {
                    // Get the user's actual roles from the database
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var primaryRole = userRoles.FirstOrDefault() ?? "user";

                    // Determine role based on actual roles and records
                    string displayRole;
                    if (userRoles.Contains("admin"))
                        displayRole = "admin";
                    else if (userRoles.Contains("doctor"))
                        displayRole = "doctor";
                    else if (userRoles.Contains("nurse"))
                        displayRole = "nurse";
                    else if (userRoles.Contains("staff"))
                        displayRole = "staff";
                    else if (userRoles.Contains("patient"))
                        displayRole = "patient";
                    else if (user.Staff != null)
                        displayRole = "staff";
                    else if (user.Patient != null)
                        displayRole = "patient";
                    else
                        displayRole = "user";

                    usersWithRoles.Add(new
                    {
                        Id = user.Id,
                        Email = user.Email ?? "",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Phone = user.PhoneNumber,
                        Role = displayRole,
                        ActualRoles = userRoles, // Include actual roles for debugging
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        PatientInfo = user.Patient != null ? new
                        {
                            PatientId = user.Patient.Id,
                            PatientNumber = user.Patient.PatientNumber,
                            DateOfBirth = user.Patient.DateOfBirth,
                            Address = user.Patient.Address,
                            EmergencyContact = user.Patient.EmergencyContactName,
                            EmergencyPhone = user.Patient.EmergencyContactPhone
                        } : null,
                        StaffInfo = user.Staff != null ? new
                        {
                            StaffId = user.Staff.Id,
                            StaffNumber = user.Staff.StaffNumber
                        } : null
                    });
                }

                return Ok(new { success = true, data = usersWithRoles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet("services")]
        public async Task<ActionResult<object>> GetServices()
        {
            try
            {
                var services = await _context.Services
                    .Where(s => s.IsActive)
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        DurationMinutes = s.DurationMinutes,
                        Price = s.Price,
                        IsActive = s.IsActive
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = services });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Internal server error: " + ex.Message });
            }
        }

        [HttpPost("walkin-appointment")]
        public async Task<ActionResult<object>> CreateWalkInAppointment([FromBody] AdminWalkInAppointmentRequest request)
        {
            try
            {
                // Validate patient exists
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == request.PatientId);
                if (patient == null)
                {
                    return BadRequest(new { success = false, error = "Patient not found" });
                }

                // Validate service exists
                var service = await _context.Services.FindAsync(request.ServiceId);
                if (service == null)
                {
                    return BadRequest(new { success = false, error = "Service not found" });
                }

                // Validate staff exists
                var staff = await _context.Staff.FindAsync(request.StaffId);
                if (staff == null)
                {
                    return BadRequest(new { success = false, error = "Staff member not found" });
                }

                // Parse appointment date and times
                if (!DateTime.TryParse(request.AppointmentDate, out var appointmentDate))
                {
                    return BadRequest(new { success = false, error = "Invalid appointment date format. Use YYYY-MM-DD" });
                }

                if (!TimeOnly.TryParse(request.StartTime, out var startTime))
                {
                    return BadRequest(new { success = false, error = "Invalid start time format. Use HH:mm:ss or HH:mm" });
                }

                TimeOnly endTime;
                if (string.IsNullOrEmpty(request.EndTime))
                {
                    // Calculate end time based on service duration
                    endTime = startTime.Add(TimeSpan.FromMinutes(service.DurationMinutes));
                }
                else
                {
                    if (!TimeOnly.TryParse(request.EndTime, out endTime))
                    {
                        return BadRequest(new { success = false, error = "Invalid end time format. Use HH:mm:ss or HH:mm" });
                    }
                }

                // Check for time conflicts
                var conflictingAppointment = await _context.Appointments
                    .Where(a => a.AppointmentDate == appointmentDate &&
                               a.StaffId == request.StaffId &&
                               a.Status != "cancelled" &&
                               ((a.StartTime <= startTime && a.EndTime > startTime) ||
                                (a.StartTime < endTime && a.EndTime >= endTime) ||
                                (a.StartTime >= startTime && a.EndTime <= endTime)))
                    .FirstOrDefaultAsync();

                if (conflictingAppointment != null)
                {
                    return BadRequest(new { success = false, error = "Staff member is not available at this time" });
                }

                // Create the walk-in appointment
                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    PatientId = request.PatientId,
                    ServiceId = request.ServiceId,
                    StaffId = request.StaffId,
                    AppointmentDate = appointmentDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = "walkin",
                    BookingType = "walkin",
                    IsWalkIn = true,
                    ServicePrice = service.Price,
                    PaymentStatus = "pending",
                    Notes = request.Notes,
                    ApprovalStatus = ApprovalStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Walk-in appointment created successfully",
                    data = new
                    {
                        appointmentId = appointment.Id,
                        patientId = appointment.PatientId,
                        patientName = $"{patient.User.FirstName} {patient.User.LastName}",
                        serviceName = service.Name,
                        staffName = staff.User?.FirstName + " " + staff.User?.LastName,
                        appointmentDate = appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                        startTime = appointment.StartTime.ToString("HH:mm"),
                        endTime = appointment.EndTime.ToString("HH:mm"),
                        status = appointment.Status
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = "Internal server error: " + ex.Message });
            }
        }

        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            try
            {
                var roles = await _roleManager.Roles
                    .Select(r => r.Name ?? "")
                    .ToListAsync();

                return Ok(roles);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPut("users/{userId}/role")]
        public async Task<ActionResult> UpdateUserRole(Guid userId, [FromBody] UpdateUserRoleDto request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return NotFound(new { error = "User not found" });
                }

                // Get current roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                
                // Remove all current roles
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                
                // Add new role
                var result = await _userManager.AddToRoleAsync(user, request.NewRole);
                if (!result.Succeeded)
                {
                    return BadRequest(new { error = "Failed to update user role" });
                }

                return Ok(new { message = "User role updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("reports/revenue")]
        public async Task<ActionResult<RevenueReportDto>> GetRevenueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var start = startDate ?? DateTime.Today.AddDays(-30);
                var end = endDate ?? DateTime.Today;

                var appointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Where(a => a.AppointmentDate >= start && a.AppointmentDate <= end && a.Status == "completed")
                    .ToListAsync();

                var serviceBreakdown = appointments
                    .GroupBy(a => a.Service)
                    .Select(g => new ServiceRevenueDto
                    {
                        ServiceId = g.Key.Id,
                        ServiceName = g.Key.Name,
                        AppointmentCount = g.Count(),
                        Revenue = g.Count() * 100 // Assuming $100 per appointment - this should be configurable
                    })
                    .ToList();

                var report = new RevenueReportDto
                {
                    StartDate = start,
                    EndDate = end,
                    TotalAppointments = appointments.Count,
                    TotalRevenue = serviceBreakdown.Sum(s => s.Revenue),
                    ServiceBreakdown = serviceBreakdown
                };

                return Ok(report);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}

