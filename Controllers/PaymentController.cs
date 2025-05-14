using International_Bank.Model;
using InternationalBankAPI.Data; // Added for ApplicationDbContext
using InternationalBankAPI.Models; // Added for Transaction
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InternationalBankAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")] 
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context; // Changed from CustomerContext

        public PaymentController(ApplicationDbContext context) // Changed from CustomerContext
        {
            _context = context;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> ConfirmPayment([FromBody] CreateTransactionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            // Get customer ID from JWT token
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(customerId))
            {
                return Unauthorized("Customer is not logged in.");
            }

            // Verify if the customerId from the token actually exists in the database
            var anyuser = await _context.Users.Select(u => u.Id).FirstOrDefaultAsync();
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Unauthorized("User account not found or token is invalid. Please log in again. Token uID is " + userId + " and database userID is " + anyuser);
            }

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Currency = dto.Currency,
                SwiftCode = dto.SwiftCode,
                RecipientAccountName = dto.RecipientAccountName,
                RecipientAccountNumber = dto.RecipientAccountNumber,
                RecipientSwiftCode = dto.RecipientSwiftCode,
                CreatedDate = DateTime.UtcNow,
                IsVerified = false,
                CustomerId = customerId
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Payment successful!",
                transaction.Id,
                transaction.Amount,
                transaction.Currency,
                transaction.RecipientAccountName
            });
        }
    }
}
