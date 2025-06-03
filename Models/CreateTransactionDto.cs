using System.ComponentModel.DataAnnotations;

namespace International_Bank.Model
{
    public class CreateTransactionDto
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{4}[A-Z]{2}[A-Z0-9]{2}([A-Z0-9]{3})?$", ErrorMessage = "Invalid email address. A valid email looks like 'user@example.com'")]
        public string SwiftCode { get; set; }

        [Required]
        public string RecipientAccountName { get; set; }

        [Required]
        public string RecipientAccountNumber { get; set; }

        [Required]
        public string RecipientSwiftCode { get; set; }
    }
}