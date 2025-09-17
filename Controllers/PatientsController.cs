using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.DTOs;
using HopewellClinicApi.Attributes;

namespace HopewellClinicApi.Controllers
{
[ApiController]
[Route("api/[controller]")]
[JwtAuthorize]
public class PatientsController : ControllerBase
    {
        private readonly HopewellDbContext _context;

        public PatientsController(HopewellDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponse>>> GetPatients()
        {
            try
            {
                var patients = await _context.Patients
                    .Include(p => p.User)
                    .Select(p => new PatientResponse
                    {
                        Id = p.Id,
                        UserId = p.UserId,
                        PatientNumber = p.PatientNumber,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        Phone = p.User.PhoneNumber ?? "",
                        Email = p.User.Email ?? "",
                        DateOfBirth = p.DateOfBirth,
                        Address = p.Address,
                        EmergencyContactName = p.EmergencyContactName,
                        EmergencyContactPhone = p.EmergencyContactPhone,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(patients);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPatients: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientResponse>> GetPatient(Guid id)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (patient == null)
                {
                    return NotFound(new { error = "Patient not found" });
                }

                var response = new PatientResponse
                {
                    Id = patient.Id,
                    UserId = patient.UserId,
                    PatientNumber = patient.PatientNumber,
                    FirstName = patient.User.FirstName,
                    LastName = patient.User.LastName,
                    Phone = patient.User.PhoneNumber ?? "",
                    Email = patient.User.Email ?? "",
                    DateOfBirth = patient.DateOfBirth,
                    Address = patient.Address,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactPhone = patient.EmergencyContactPhone,
                    CreatedAt = patient.CreatedAt,
                    UpdatedAt = patient.UpdatedAt
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientRequest request)
        {
            try
            {
                var patient = await _context.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (patient == null)
                {
                    return NotFound(new { error = "Patient not found" });
                }

                // Update patient fields
                if (request.Address != null)
                    patient.Address = request.Address;

                if (request.PhoneNumber != null)
                    patient.User.PhoneNumber = request.PhoneNumber;

                patient.UpdatedAt = DateTime.UtcNow;
                patient.User.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Patient updated successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpGet("search")]
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
    }
}

