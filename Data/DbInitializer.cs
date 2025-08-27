using System;
using System.Linq;
using HopewellClinicApi.Models;           
using HopewellClinicApi.Data;      
using Microsoft.AspNetCore.Identity;

public static class DbInitializer
{
    public static void SeedRuntimeData(HopewellDbContext context)
    {
        var now = DateTime.UtcNow;

        foreach (var user in context.Users.Where(u => u.CreatedAt == default))
        {
            user.CreatedAt = now;
            user.UpdatedAt = now;
        }

        foreach (var service in context.Services.Where(s => s.CreatedAt == default))
        {
            service.CreatedAt = now;
            service.UpdatedAt = now;
        }

        // Seed Patients with dynamic DateOfBirth
        if (!context.Patients.Any())
        {
            // First, create a user for the patient if it doesn't exist
            var patientUserId = Guid.Parse("550e8400-e29b-41d4-a716-446655442010");
            var existingUser = context.Users.FirstOrDefault(u => u.Id == patientUserId);
            
            if (existingUser == null)
            {
                // Create the user first
                var patientUser = new ApplicationUser
                {
                    Id = patientUserId,
                    FirstName = "Nelson",
                    LastName = "Mandela",
                    Email = "nelson.mandela@example.com",
                    NormalizedEmail = "NELSON.MANDELA@EXAMPLE.COM",
                    UserName = "nelson.mandela@example.com",
                    NormalizedUserName = "NELSON.MANDELA@EXAMPLE.COM",
                    PhoneNumber = "+27821234567",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                
                context.Users.Add(patientUser);
            }

            // Now create the patient
            context.Patients.Add(new Patient
            {
                Id = Guid.Parse("550e8400-e29b-41d4-a716-446655442000"),
                UserId = patientUserId,
                PatientNumber = "PAT001",
                DateOfBirth = new DateTime(1985, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                Address = "123 Main Street, Qunu Village",
                EmergencyContactName = "Sizani Gcaba",
                EmergencyContactPhone = "+27821234568"
            });
        }

        context.SaveChanges();
    }
}
