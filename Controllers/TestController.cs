using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HopewellClinicApi.Data;
using HopewellClinicApi.Attributes;

namespace HopewellClinicApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtAuthorize]
    public class TestController : ControllerBase
    {
        [HttpGet("auth-test")]
        public IActionResult AuthTest()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            
            return Ok(new
            {
                message = "JWT Authentication working!",
                userId = userId,
                userEmail = userEmail,
                userRoles = userRoles,
                timestamp = DateTime.UtcNow,
                isAuthenticated = User.Identity?.IsAuthenticated ?? false
            });
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult PublicTest()
        {
            return Ok(new
            {
                message = "Public endpoint working!",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("database-test")]
        [AllowAnonymous]
        public async Task<IActionResult> DatabaseTest()
        {
            try
            {
                // Test database connection
                var context = HttpContext.RequestServices.GetRequiredService<HopewellDbContext>();
                var canConnect = await context.Database.CanConnectAsync();
                
                if (canConnect)
                {
                    var userCount = await context.Users.CountAsync();
                    var patientCount = await context.Patients.CountAsync();
                    var serviceCount = await context.Services.CountAsync();
                    
                    return Ok(new
                    {
                        message = "Database connection successful!",
                        userCount = userCount,
                        patientCount = patientCount,
                        serviceCount = serviceCount,
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "Database connection failed!",
                        timestamp = DateTime.UtcNow
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    message = "Database test error",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
