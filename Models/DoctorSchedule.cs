using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HopewellClinicApi.Models
{
    public class DoctorSchedule
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(10)]
        public string DayOfWeek { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan ShiftStart { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan ShiftEnd { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Column(TypeName = "time")]
        public TimeSpan? BreakStart { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? BreakEnd { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("DoctorId")]
        public virtual Staff? Doctor { get; set; }
    }
}
