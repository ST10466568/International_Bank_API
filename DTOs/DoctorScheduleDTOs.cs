using System.ComponentModel.DataAnnotations;

namespace HopewellClinicApi.DTOs
{
    // Doctor schedule management DTOs
    public class DoctorScheduleDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateDoctorScheduleRequest
    {
        [Required]
        public List<DoctorScheduleItemDto> Schedule { get; set; } = new();
    }

    public class DoctorScheduleItemDto
    {
        [Required]
        public string DayOfWeek { get; set; } = string.Empty;
        
        [Required]
        public bool IsActive { get; set; }
        
        [Required]
        public TimeSpan ShiftStart { get; set; }
        
        [Required]
        public TimeSpan ShiftEnd { get; set; }
        
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
    }

    public class DoctorScheduleManagementResponse
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public List<DoctorScheduleDto> WeeklySchedule { get; set; } = new();
    }

    // Doctor availability DTOs
    public class DoctorAvailabilityRequest
    {
        [Required]
        public DateTime Date { get; set; }
        
        public TimeSpan? Time { get; set; }
    }

    public class DoctorAvailabilityManagementResponse
    {
        public bool IsAvailable { get; set; }
        public string Reason { get; set; } = string.Empty;
        public TimeSpan? NextAvailableTime { get; set; }
        public List<TimeSpan> AvailableSlots { get; set; } = new();
    }

    // Enhanced doctor on duty response
    public class DoctorOnDutyEnhancedDto
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
        public string DayOfWeek { get; set; } = string.Empty;
        public TimeSpan? BreakStart { get; set; }
        public TimeSpan? BreakEnd { get; set; }
    }

    // Time slot generation request
    public class GenerateTimeSlotsRequest
    {
        [Required]
        public Guid DoctorId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public int ServiceDuration { get; set; } = 30;
        
        public Guid? ServiceId { get; set; }
    }

    // Enhanced time slot DTO
    public class TimeSlotEnhancedDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public bool IsAvailable { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;
        public bool IsDuringBreak { get; set; }
        public string? ConflictReason { get; set; }
    }

    // Schedule conflict DTO
    public class ScheduleConflictDto
    {
        public Guid AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
    }

    // Doctor schedule summary
    public class DoctorScheduleSummaryDto
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public int ActiveDays { get; set; }
        public TimeSpan TotalWeeklyHours { get; set; }
        public List<string> WorkingDays { get; set; } = new();
        public List<string> OffDays { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }
}
