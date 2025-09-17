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
        public async Task<ActionResult> CreateStaff([FromBody] CreateStaffRequest request)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { error = "User with this email already exists" });
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
                    return BadRequest(new { error = "Failed to create user", details = result.Errors });
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

                return Ok(new { message = "Staff user created successfully", staffId = staff.Id });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
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
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Select(u => new UserResponseDto
                    {
                        Id = u.Id,
                        Email = u.Email ?? "",
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Role = "user", // This would need to be determined from UserRoles
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
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

