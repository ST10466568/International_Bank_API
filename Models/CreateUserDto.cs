using System.ComponentModel.DataAnnotations;

namespace InternationalBankAPI.Models
{
    public class CreateUserDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address. A valid email looks like 'user@example.com'.")]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number must be atleast 13 characters.")]
        public string IDNumber { get; set; }

        [Required]
        public string Role { get; set; } // "Customer", "Employee", etc.
    }
}