using InternationalBankAPI.Data;
using InternationalBankAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using International_Bank.Model;

namespace InternationalBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CUSTOMER creates transaction
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) // Good to add this explicit check too
            {
                return Unauthorized("User identifier not found in token.");
            }
            var anyuser = await _context.Users.Select(u => u.Id).FirstOrDefaultAsync();
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return Unauthorized("User account not found or token is invalid. Please log in again. uID is " + userId + " and database userID is " + anyuser);
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
                CustomerId = userId
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("unverified")]
        public async Task<IActionResult> GetUnverified()
        {
            var unverified = await _context.Transactions
                .Include(t => t.Customer)
                .Where(t => !t.IsVerified)
                .ToListAsync();

            return Ok(unverified);
        }

        // EMPLOYEE verifies transaction
        [Authorize(Roles = "Employee")]
        [HttpPut("verify/{id}")]
        public async Task<IActionResult> VerifyTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return NotFound("Transaction not found");

            if (transaction.IsVerified)
                return BadRequest("Transaction is already verified.");

            transaction.IsVerified = true;
            await _context.SaveChangesAsync();

            return Ok("Transaction verified for SWIFT processing.");
        }

        // EMPLOYEE or CUSTOMER can view their transactions
        [Authorize(Roles = "Employee,Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (User.IsInRole("Customer"))
            {
                var customerTransactions = await _context.Transactions
                    .Where(t => t.CustomerId == userId)
                    .ToListAsync();

                return Ok(customerTransactions);
            }

            // Employees see all
            var allTransactions = await _context.Transactions
                .Include(t => t.Customer)
                .ToListAsync();

            return Ok(allTransactions);
        }
    }
}
