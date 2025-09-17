using System.ComponentModel.DataAnnotations;

namespace HopewellClinicApi.DTOs
{
    // Enhanced Appointment DTOs
    public class CreateAppointmentRequestEnhanced
    {
        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string StartTime { get; set; } = string.Empty;

        public Guid? StaffId { get; set; }

        public string? Notes { get; set; }
    }

    // Patient Management DTOs
    public class UpdatePatientRequest
    {
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }



    public class PatientSummaryDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PatientNumber { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    // Service Management DTOs
    public class CreateServiceRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int DurationMinutes { get; set; }
    }

    public class UpdateServiceRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public bool? IsActive { get; set; }
    }

    // Staff Management DTOs
    public class CreateStaffRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }

    public class UpdateStaffRequest
    {
        public string? PhoneNumber { get; set; }
    }

    public class UpdateAvailabilityRequest
    {
        [Required]
        public int DayOfWeek { get; set; }

        [Required]
        public string StartTime { get; set; } = string.Empty;

        [Required]
        public string EndTime { get; set; } = string.Empty;
    }

    // Doctor Dashboard DTOs
    public class CreateWalkinAppointmentDto
    {
        [Required]
        public string PatientFirstName { get; set; } = string.Empty;

        [Required]
        public string PatientLastName { get; set; } = string.Empty;

        [Required]
        public string PatientPhone { get; set; } = string.Empty;

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string StartTime { get; set; } = string.Empty;
    }

    public class UpdateStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }

    // Nurse Dashboard DTOs
    public class BookAppointmentForPatientDto
    {
        [Required]
        public Guid PatientId { get; set; }

        public Guid? StaffId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string StartTime { get; set; } = string.Empty;
    }

    // Admin Dashboard DTOs
    public class UpdateUserRoleDto
    {
        [Required]
        public string NewRole { get; set; } = string.Empty;
    }

    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AppointmentStatsDto
    {
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class RevenueReportDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalAppointments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ServiceRevenueDto> ServiceBreakdown { get; set; } = new List<ServiceRevenueDto>();
    }

    public class ServiceRevenueDto
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int AppointmentCount { get; set; }
        public decimal Revenue { get; set; }
    }
}
