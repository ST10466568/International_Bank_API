// In Controllers/UserManagementController.cs
using InternationalBankAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Employee,Admin")] // Only Employees or Admins can edit roles
public class UserManagementController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("users/{id}/edit-role")]
    public async Task<IActionResult> EditUserRole(string id, [FromBody] RoleUpdateDto model)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found." });

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeRolesResult.Succeeded)
            return BadRequest(new { message = "Failed to remove old roles." });

        if (!await _roleManager.RoleExistsAsync(model.Role))
            return BadRequest(new { message = "Invalid role specified." });

        var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
        if (!addRoleResult.Succeeded)
            return BadRequest(new { message = "Failed to add new role." });

        return Ok(new { message = $"Role updated to {model.Role}." });
    }

    [HttpPost("users/{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found." });

        // Generate a new random password
        var newPassword = "Temp" + Guid.NewGuid().ToString("N").Substring(0, 8) + "!"; // e.g., Temp4a9b2...!

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            return BadRequest(new { message = "Failed to reset password." });

        // Optionally, email the user the new password (not implemented here)
        return Ok(new { message = "Password reset successful.", newPassword });
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers() // Changed to async Task<IActionResult>
    {
        var usersFromDb = _userManager.Users.ToList(); 
        var userListResult = new List<UserListDto>(); // Use the DTO

        foreach (var user in usersFromDb)
        {
            var roles = await _userManager.GetRolesAsync(user); // Await the async call
            userListResult.Add(new UserListDto // Populate the DTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname, 
                AccountNumber = user.AccountNumber,
                IDNumber = user.IDNumber, 
                Role = roles.FirstOrDefault(),
                Username = user.UsernameCustom
            });
        }

        return Ok(userListResult);
    }


    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            return BadRequest(new { message = "A user with this email already exists." });

        var newUser = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Name = model.Name,
            Surname = model.Surname,
            IDNumber = model.IDNumber,
            Created_Date = DateTime.UtcNow,
            Created_By = User.Identity?.Name ?? "System",
            UsernameCustom = model.Username,
            AccountNumber = model.Role == "Customer" ? GenerateAccountNumber() : null
        };

        var createResult = await _userManager.CreateAsync(newUser, model.Password);
        if (!createResult.Succeeded)
            return BadRequest(new { message = "Failed to create user.", errors = createResult.Errors });

        if (!await _roleManager.RoleExistsAsync(model.Role))
            return BadRequest(new { message = "Invalid role specified." });

        var roleResult = await _userManager.AddToRoleAsync(newUser, model.Role);
        if (!roleResult.Succeeded)
            return BadRequest(new { message = "Failed to assign role." });

        return Ok(new { message = $"User {model.Email} created with role {model.Role}." });
    }

    [Authorize(Roles = "Employee,Admin")]
    [HttpPut("users/{id}")]
    public async Task<IActionResult> EditUser(string id, [FromBody] EditUserDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found." });

        // Update fields
        user.Name = model.Name ?? user.Name;
        user.Surname = model.Surname ?? user.Surname;
        user.Email = model.Email ?? user.Email;
        user.UserName = model.Email ?? user.UserName;
        user.IDNumber = model.IDNumber ?? user.IDNumber;
        user.ModifiedBy = User.Identity?.Name ?? "System"; // Corrected property name
        user.ModifiedDate = DateTime.UtcNow; // Corrected property name

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return BadRequest(new { message = "Failed to update user.", errors = updateResult.Errors });

        return Ok(new { message = "User updated successfully." });
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found." });

        // Optionally, prevent deletion of self
        if (User.Identity?.Name == user.UserName)
            return BadRequest(new { message = "You cannot delete your own account." });

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return BadRequest(new { message = "Failed to delete user.", errors = result.Errors });

        return Ok(new { message = "User deleted successfully." });
    }

    private string GenerateAccountNumber()
    {
        var random = new Random();
        string bankCode = "INTB"; // Bank prefix
        string uniquePart = random.Next(10000000, 99999999).ToString(); // 8-digit unique number
        return $"{bankCode}{uniquePart}";
    }

    public class RoleUpdateDto
    {
        public string Role { get; set; }
    }
}