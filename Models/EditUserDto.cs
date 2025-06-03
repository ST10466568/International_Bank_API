using System.ComponentModel.DataAnnotations;

namespace InternationalBankAPI.Models
{
    public class EditUserDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address. A valid email looks like 'user@example.com'.")]
        public string? Email { get; set; }
        public string? IDNumber { get; set; }
    }
}