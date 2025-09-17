using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopewellClinicApi.Models
{
    [Table("doctor_shifts")]
    public class DoctorShift
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("doctor_id")]
        [Required]
        public Guid DoctorId { get; set; }

        [Column("day_of_week")]
        public int DayOfWeek { get; set; } // 0=Sunday, 1=Monday, etc.

        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("DoctorId")]
        public virtual ApplicationUser Doctor { get; set; } = null!;
    }
}
