using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InternationalBankAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        public string IDNumber { get; set; }

        public string? AccountNumber { get; set; } // Only used if user is a Customer

        public string UsernameCustom { get; set; } // Only used if user is a Customer

        [Required]
        public DateTime Created_Date { get; set; }

        [Required]
        public string Created_By { get; set; }

        public DateTime? Modified_Date { get; set; }
        public string? Modified_By { get; set; }
    }
}
