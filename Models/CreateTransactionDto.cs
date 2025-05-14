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
        public string SwiftCode { get; set; }

        [Required]
        public string RecipientAccountName { get; set; }

        [Required]
        public string RecipientAccountNumber { get; set; }

        [Required]
        public string RecipientSwiftCode { get; set; }
    }
}