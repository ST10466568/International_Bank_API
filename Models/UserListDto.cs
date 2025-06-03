using System.ComponentModel.DataAnnotations;

namespace InternationalBankAPI.Models
{
    public class UserListDto
    {
        public string Id { get; set; }
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address. A valid email looks like 'user@example.com'.")]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? AccountNumber { get; set; }
        public string IDNumber { get; set; }
        public string? Role { get; set; }
        public string Username { get; set; }
    
    }
}