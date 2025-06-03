using InternationalBankAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using InternationalBankAPI.Data;
using Microsoft.EntityFrameworkCore; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization; // Added for AllowAnonymous
using Microsoft.IdentityModel.Tokens;

namespace InternationalBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Declare the field
        private readonly ApplicationDbContext _context; // Declare the field
        private readonly IConfiguration _configuration;


        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                UsernameCustom = model.Username,
                IDNumber = model.IDNumber,
                Created_Date = DateTime.UtcNow,
                Created_By = "System",
                AccountNumber = model.Role == "Customer" ? GenerateAccountNumber() : null
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Assign role
            if (!await _roleManager.RoleExistsAsync(model.Role))
                return BadRequest("Invalid role");

            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok(new { message = "User created", role = model.Role });
        }
        
       [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> EmployeeLogin([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid login credentials.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid login credentials.");

            var roles = await _userManager.GetRolesAsync(user);

            // ✅ Only allow Employees to login here
            if (!roles.Contains("Employee"))
                return Unauthorized("Access denied. Not an employee account.");

            var token = GenerateJwtToken(user, roles);

            return Ok(new
            {
                message = "Login successful",
                token,
                email = user.Email,
                role = roles.FirstOrDefault(),
                name = user.Name
            });
        }

        [AllowAnonymous]
        [HttpPost("customer/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] CustomerLoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Look up customer by custom fields
            var customer = await _context.Users.FirstOrDefaultAsync(c =>
                c.UsernameCustom == model.Username &&
                c.AccountNumber == model.AccountNumber);

            if (customer == null || !(await _userManager.CheckPasswordAsync(customer, model.Password)))
                return Unauthorized("Invalid credentials");

            // Get roles (should be "Customer")
            var roles = await _userManager.GetRolesAsync(customer);
            var token = GenerateJwtToken(customer, roles); // Reuse method

            return Ok(new
            {
                message = "Login successful",
                token,
                email = customer.Email,
                role = roles.FirstOrDefault(),
                name = customer.Name,
                accountNumber = customer.AccountNumber
            });
        }



        private string GenerateAccountNumber()
        {
            var random = new Random();
            string bankCode = "INTB"; // Prefix for InternationalBank
            string uniquePart = random.Next(10000000, 99999999).ToString(); // 8-digit number
            return $"{bankCode}{uniquePart}";
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "")
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize(Roles = "Employee,Admin")]
        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                UsernameCustom = model.Username,
                IDNumber = model.IDNumber,
                AccountNumber = GenerateAccountNumber(),
                Created_Date = DateTime.UtcNow,
                Created_By = User.Identity.Name
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Customer");

            return Ok(new { message = "Customer account created successfully." });
        }

    }

    

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}