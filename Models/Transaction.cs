using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternationalBankAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required, MaxLength(3)]
        public string Currency { get; set; } = string.Empty;

        [Required, MaxLength(11)]
        [RegularExpression(@"^[A-Z]{4}[A-Z]{2}[A-Z0-9]{2}([A-Z0-9]{3})?$", ErrorMessage = "Invalid SWIFT code. Charartors Required. 1 - Exactly 4 uppercase letters (bank code). 2 - Exactly 2 uppercase letters (country code). 3 - Exactly 2 characters, uppercase letters or digits (location code).4- An optional group of 3 alphanumeric characters (branch code). Example: DEUTZAFF500")]
        public string SwiftCode { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string RecipientAccountName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string RecipientAccountNumber { get; set; } = string.Empty;

        [Required, MaxLength(11)]
        public string RecipientSwiftCode { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsVerified { get; set; } = false;

        // Foreign key to ApplicationUser (Customer)
        [Required]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual ApplicationUser Customer { get; set; }
    }
}
