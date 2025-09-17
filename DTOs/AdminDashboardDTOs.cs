using System.ComponentModel.DataAnnotations;

namespace HopewellClinicApi.DTOs
{
    // Service Management DTOs
    public class ServiceDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes")]
        public int DurationMinutes { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal? Price { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class ServiceResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public decimal? Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Reports DTOs
    public class AppointmentStatisticsResponse
    {
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int WalkInAppointments { get; set; }
        public double CompletionRate { get; set; }
        public double CancellationRate { get; set; }
        public List<DailyAppointmentStats> DailyStats { get; set; } = new();
    }

    public class DailyAppointmentStats
    {
        public DateTime Date { get; set; }
        public int TotalAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int WalkInAppointments { get; set; }
    }

    public class ServiceUsageResponse
    {
        public List<ServiceUsageStats> ServiceStats { get; set; } = new();
        public int TotalServicesUsed { get; set; }
        public string MostPopularService { get; set; } = string.Empty;
        public string LeastPopularService { get; set; } = string.Empty;
    }

    public class ServiceUsageStats
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int UsageCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public double UsagePercentage { get; set; }
    }

    public class RevenueReportResponse
    {
        public decimal TotalRevenue { get; set; }
        public decimal PendingRevenue { get; set; }
        public decimal RefundedRevenue { get; set; }
        public List<DailyRevenueStats> DailyRevenue { get; set; } = new();
        public List<ServiceRevenueStats> ServiceRevenue { get; set; } = new();
        public List<DoctorRevenueStats> DoctorRevenue { get; set; } = new();
    }

    public class DailyRevenueStats
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int AppointmentCount { get; set; }
    }

    public class ServiceRevenueStats
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int AppointmentCount { get; set; }
        public double RevenuePercentage { get; set; }
    }

    public class DoctorRevenueStats
    {
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int AppointmentCount { get; set; }
        public double RevenuePercentage { get; set; }
    }

    // Admin Dashboard Summary
    public class AdminDashboardSummary
    {
        public int TotalUsers { get; set; }
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalNurses { get; set; }
        public int TotalServices { get; set; }
        public int ActiveServices { get; set; }
        public int TotalAppointmentsToday { get; set; }
        public decimal TodayRevenue { get; set; }
        public List<ServiceResponse> RecentServices { get; set; } = new();
    }
}

