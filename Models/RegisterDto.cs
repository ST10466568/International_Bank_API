using System.ComponentModel.DataAnnotations;

namespace InternationalBankAPI.Models
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number must be 13 characters.")]
        public string IDNumber { get; set; }

        [Required]
        [RegularExpression("^(Customer|Employee)$", ErrorMessage = "Role must be either 'Customer' or 'Employee'.")]
        public string Role { get; set; }
    }
}
