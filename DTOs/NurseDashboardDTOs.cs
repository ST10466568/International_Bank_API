using System.ComponentModel.DataAnnotations;
using HopewellClinicApi.Models;

namespace HopewellClinicApi.DTOs
{
    // Walk-in Appointment DTOs
    public class WalkInAppointmentDto
    {
        [Required]
        public string PatientFirstName { get; set; } = string.Empty;

        [Required]
        public string PatientLastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string PatientEmail { get; set; } = string.Empty;

        [Required]
        public string PatientPhone { get; set; } = string.Empty;

        [Required]
        public Guid ServiceId { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        public string? Notes { get; set; }
    }

    public class WalkInAppointmentResponse
    {
        public Guid AppointmentId { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid PatientId { get; set; }
        public string PatientNumber { get; set; } = string.Empty;
    }

    // Doctor Approval DTOs
    public class DoctorApprovalDto
    {
        [Required]
        public Guid DoctorId { get; set; }

        public string? ApprovalNotes { get; set; }
    }

    public class NurseApprovalResponse
    {
        public string Message { get; set; } = string.Empty;
        public Guid AppointmentId { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Enhanced Appointment Response for Nurses
    public class NurseAppointmentResponse
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public ApprovalStatus ApprovalStatus { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? ApprovedBy { get; set; }
        public string? ApprovedByNurseId { get; set; }
        public DateTime? NurseApprovalDate { get; set; }
        public string? ApprovalNotes { get; set; }
        public bool IsWalkIn { get; set; }
        public decimal? ServicePrice { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Notes { get; set; }
        public string BookingType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Related entities
        public ServiceResponse Service { get; set; } = null!;
        public PatientResponse Patient { get; set; } = null!;
        public StaffResponse? Staff { get; set; }
    }

    // Nurse Dashboard Summary
    public class NurseDashboardSummary
    {
        public int TotalAppointmentsToday { get; set; }
        public int PendingApprovals { get; set; }
        public int WalkInAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public List<NurseAppointmentResponse> RecentAppointments { get; set; } = new();
    }
}
