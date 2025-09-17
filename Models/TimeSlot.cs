using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopewellClinicApi.Models
{
    [Table("time_slots")]
    public class TimeSlot
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("day_of_week")]
        public int DayOfWeek { get; set; }

        [Column("start_time")]
        public TimeOnly StartTime { get; set; }

        [Column("end_time")]
        public TimeOnly EndTime { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsBooked { get; set; }

        // New properties for enhanced booking system
        [Column("doctor_id")]
        public Guid? DoctorId { get; set; }

        [Column("date")]
        public DateTime? Date { get; set; }

        [Column("duration")]
        public int Duration { get; set; } = 30; // in minutes

        [Column("is_available")]
        public bool IsAvailable { get; set; } = true;

        [Column("appointment_id")]
        public Guid? AppointmentId { get; set; }

        // Navigation properties
        [ForeignKey("DoctorId")]
        public virtual Staff? Doctor { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }
    }
}