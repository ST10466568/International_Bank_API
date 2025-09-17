using System.ComponentModel.DataAnnotations;

namespace HopewellClinicApi.DTOs
{
    // Step 1: Date selection validation
    public class DateValidationRequest
    {
        [Required]
        public DateTime Date { get; set; }
    }

    public class DateValidationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<DateTime> SuggestedDates { get; set; } = new();
    }

    // Step 2: Doctors on duty
    public class DoctorOnDutyRequest
    {
        [Required]
        public DateTime Date { get; set; }
        public Guid? ServiceId { get; set; }
    }

    public class DoctorOnDutyResponse
    {
        public List<DoctorOnDutyDto> Doctors { get; set; } = new();
    }

    public class DoctorOnDutyDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public double Rating { get; set; }
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> Services { get; set; } = new();
    }

    // Step 3: Available time slots by doctor
    public class AvailableSlotsByDoctorRequest
    {
        [Required]
        public Guid DoctorId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public Guid? ServiceId { get; set; }
    }

    public class AvailableSlotsByDoctorResponse
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public List<TimeSlotDto> AvailableSlots { get; set; } = new();
    }

    public class TimeSlotDto
    {
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public bool IsAvailable { get; set; }
        public Guid? DoctorId { get; set; }
    }

    // Step 4: Staff on duty
    public class StaffOnDutyRequest
    {
        [Required]
        public DateTime Date { get; set; }
        public string? Role { get; set; }
    }

    public class StaffOnDutyResponse
    {
        public List<StaffOnDutyDto> Staff { get; set; } = new();
    }

    public class StaffOnDutyDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public bool IsAvailable { get; set; }
    }

    // Step 5: Enhanced appointment creation
    public class CreateBookingAppointmentRequest
    {
        [Required]
        public Guid PatientId { get; set; }
        [Required]
        public Guid DoctorId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }
        public string? Notes { get; set; }
        public Guid? ServiceId { get; set; }
    }

    // Error response for booking validation
    public class BookingErrorResponse
    {
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }
    }

    // Enhanced appointment response
    public class AppointmentBookingResponse
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ConfirmationNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
