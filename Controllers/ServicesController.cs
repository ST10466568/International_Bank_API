using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Models;
using HopewellClinicApi.Attributes;

namespace HopewellClinicApi.Controllers
{
[ApiController]
[Route("api/[controller]")]
[JwtAuthorize]
public class ServicesController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public ServicesController(HopewellDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceResponse>>> GetServices()
        {
            try
            {
                var services = await _context.Services
                    .Where(s => s.IsActive)
                    .Select(s => new ServiceResponse
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        DurationMinutes = s.DurationMinutes,
                        Price = s.Price,
                        IsActive = s.IsActive,
                        CreatedAt = s.CreatedAt,
                        UpdatedAt = s.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(services);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost]
        [AuthorizeAdmin]
        public async Task<ActionResult<ServiceResponse>> CreateService([FromBody] ServiceDto request)
        {
            try
            {
                var service = new Service
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    DurationMinutes = request.DurationMinutes,
                    Price = request.Price,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Services.Add(service);
                await _context.SaveChangesAsync();

                var response = new ServiceResponse
                {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    DurationMinutes = service.DurationMinutes,
                    Price = service.Price,
                    IsActive = service.IsActive,
                    CreatedAt = service.CreatedAt,
                    UpdatedAt = service.UpdatedAt
                };

                return CreatedAtAction(nameof(GetServices), new { id = service.Id }, response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        [AuthorizeAdmin]
        public async Task<ActionResult> UpdateService(Guid id, [FromBody] ServiceDto request)
        {
            try
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                {
                    return NotFound(new { error = "Service not found" });
                }

                // Update service fields
                service.Name = request.Name;
                service.Description = request.Description;
                service.DurationMinutes = request.DurationMinutes;
                service.Price = request.Price;
                service.IsActive = request.IsActive;

                service.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Service updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpDelete("{id}")]
        [AuthorizeAdmin]
        public async Task<ActionResult> DeactivateService(Guid id)
        {
            try
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                {
                    return NotFound(new { error = "Service not found" });
                }

                service.IsActive = false;
                service.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Service deactivated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}