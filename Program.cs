using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InternationalBankAPI.Data;
using InternationalBankAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text; // Added for Encoding
using Microsoft.AspNetCore.Authentication.JwtBearer; // Added for JwtBearerDefaults
using Microsoft.IdentityModel.Tokens; // Added for TokenValidationParameters and SymmetricSecurityKey


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => // Chained to AddAuthentication
{
    options.RequireHttpsMetadata = false; // true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(); // Separate call

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "InternationalBankAPI", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdmin(services);
}

app.Run();

static async Task SeedRolesAndAdmin(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var context = services.GetRequiredService<ApplicationDbContext>();


    string[] roles = { "Admin", "Employee", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminUser = await userManager.FindByEmailAsync("admin@bank.com");
    if (adminUser == null)
    {
        var user = new ApplicationUser
        {
            UserName = "admin@bank.com",
            Email = "admin@bank.com",
            Name = "System",
            Surname = "Administrator",
            IDNumber = "0000000000000",
            Created_Date = DateTime.UtcNow,
            Created_By = "Seeder",
            UsernameCustom = "Admin123"
        };
        await userManager.CreateAsync(user, "Admin@123");
        await userManager.AddToRoleAsync(user, "Admin");
    }

     // Seed Employee
            if (await userManager.FindByEmailAsync("employee@bank.com") is null)
            {
                var employee = new ApplicationUser
                {
                    UserName = "employee@bank.com",
                    Email = "employee@bank.com",
                    Name = "John",
                    Surname = "Employee",
                    IDNumber = "8001015009087",
                    Created_Date = DateTime.UtcNow,
                    Created_By = "Seeder",
                    UsernameCustom = "Employee123"
                };
                await userManager.CreateAsync(employee, "Employee@123");
                await userManager.AddToRoleAsync(employee, "Employee");
            }

            // Seed Customer
            if (await userManager.FindByEmailAsync("customer@bank.com") is null)
            {
                var newCustomer = new ApplicationUser
                {
                    UserName = "customer@bank.com",
                    Email = "customer@bank.com",
                    Name = "Mary",
                    Surname = "Customer",
                    IDNumber = "9002026009081",
                    AccountNumber = "ACC12345678",
                    Created_Date = DateTime.UtcNow,
                    Created_By = "Seeder",
                    UsernameCustom = "Customer123"
                };
                await userManager.CreateAsync(newCustomer, "Customer@123"); // Corrected variable name
                await userManager.AddToRoleAsync(newCustomer, "Customer");   // Corrected variable name
            }

            var newCustomer2 = await userManager.FindByEmailAsync("customer@bank.com");

            if (newCustomer2 != null && !context.Transactions.Any())
            {
                var transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Amount = 1500,
                        Currency = "USD",
                        SwiftCode = "BANKUS33",
                        RecipientAccountName = "Alice Smith",
                        RecipientAccountNumber = "1234567890",
                        RecipientSwiftCode = "ALICEUS22",
                        CreatedDate = DateTime.UtcNow.AddDays(-2),
                        IsVerified = false,
                        CustomerId = newCustomer2.Id // Corrected variable name
                    },
                    new Transaction
                    {
                        Amount = 850,
                        Currency = "EUR",
                        SwiftCode = "EURBANK99",
                        RecipientAccountName = "Bob Brown",
                        RecipientAccountNumber = "9988776655",
                        RecipientSwiftCode = "BOBBE22",
                        CreatedDate = DateTime.UtcNow.AddDays(-1),
                        IsVerified = false,
                        CustomerId = newCustomer2.Id // Corrected variable name
                    }
                };

                context.Transactions.AddRange(transactions);
                await context.SaveChangesAsync();
            }
}
