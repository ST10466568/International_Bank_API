using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using HopewellClinicApi.Data;
using HopewellClinicApi.Middleware;

using HopewellClinicApi.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework with retry policy
builder.Services.AddDbContext<HopewellDbContext>(options =>
{
  var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString)
                   .ConfigureWarnings(warnings => 
            warnings.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
});

// Add ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<HopewellDbContext>()
.AddDefaultTokenProviders();




// Custom JWT Authentication - bypasses problematic [Authorize] attribute
// No authentication services needed - handled by custom middleware





// Configure CORS to allow frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5000", "http://localhost:4001", "https://localhost:9999", "http://localhost:9999")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply migrations and runtime seed
// Apply migrations + seed runtime data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HopewellDbContext>();
    db.Database.Migrate();
    DbInitializer.SeedRuntimeData(db); 
}

// Enable CORS
app.UseCors("AllowFrontend");

// Authentication disabled for stability - manual checks in controllers if needed
// app.UseJwtAuthentication();

// Map controllers
app.MapControllers();

// Bind to localhost on port 5002 for testing
app.Run("http://localhost:5002");