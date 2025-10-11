using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HopewellClinicApi.Models;
using HopewellClinicApi.DTOs;
using System.Security.Cryptography;
using HopewellClinicApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HopewellClinicApi.Services;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HopewellDbContext _context;
        private readonly JwtService _jwtService;
        private static readonly Dictionary<string, UserSession> _activeSessions = new();

        public AuthController(UserManager<ApplicationUser> userManager, HopewellDbContext context, JwtService jwtService)
        {
            _userManager = userManager;
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterPatientDto request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
            {
                return BadRequest(new { error = "User with this email already exists." });
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.Phone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            await _userManager.AddToRoleAsync(user, "patient");

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                PatientNumber = $"PAT{DateTime.UtcNow.Ticks}", // Simple unique number
                DateOfBirth = request.DateOfBirth,
                Address = request.Address
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }

            // Generate JWT token
            var jwtToken = await _jwtService.GenerateToken(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                token = jwtToken,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    roles = userRoles
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);
                if (_activeSessions.ContainsKey(token))
                {
                    _activeSessions.Remove(token);
                }
            }

            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var session = GetSessionFromHeader(Request, out var errorResponse);
            if (session == null)
            {
                return errorResponse!;
            }

            var user = await _userManager.FindByIdAsync(session.UserId.ToString());
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            // If user is a patient, include patient information
            if (session.Roles.Contains("patient"))
            {
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                return Ok(new
                {
                    id = user.Id,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    roles = session.Roles,
                    patientId = patient?.Id,
                    patientNumber = patient?.PatientNumber
                });
            }

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                roles = session.Roles
            });
        }

        private static string GenerateSessionToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public static UserSession? GetSessionFromToken(string token)
        {
            if (!_activeSessions.TryGetValue(token, out var session))
            {
                return null;
            }

            if (session.ExpiresAt < DateTime.UtcNow)
            {
                _activeSessions.Remove(token);
                return null;
            }

            return session;
        }

        private static UserSession? GetSessionFromHeader(HttpRequest request, out IActionResult? errorResult)
        {
            errorResult = null;
            var value = request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(value) || !value.StartsWith("Bearer "))
            {
                errorResult = new UnauthorizedObjectResult(new { error = "No valid token provided" });
                return null;
            }

            var token = value.Substring("Bearer ".Length);
            return GetSessionFromToken(token);
        }
    }

    public class UserSession
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}

