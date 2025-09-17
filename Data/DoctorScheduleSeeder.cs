using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Models;

namespace HopewellClinicApi.Data
{
    public static class DoctorScheduleSeeder
    {
        public static async Task SeedDoctorSchedulesAsync(HopewellDbContext context)
        {
            // Get all active staff members who are doctors
            var doctors = await context.Staff
                .Where(s => s.IsActive)
                .ToListAsync();

            if (!doctors.Any())
            {
                return; // No doctors to seed schedules for
            }

            var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var schedules = new List<DoctorSchedule>();

            foreach (var doctor in doctors)
            {
                // Check if doctor already has schedules
                var existingSchedules = await context.DoctorSchedules
                    .Where(ds => ds.DoctorId == doctor.Id)
                    .AnyAsync();

                if (existingSchedules)
                {
                    continue; // Doctor already has schedules
                }

                foreach (var dayOfWeek in daysOfWeek)
                {
                    var isWeekend = dayOfWeek == "Saturday" || dayOfWeek == "Sunday";
                    
                    // Create schedule for the next 30 days for this day of week
                    var startDate = DateTime.Today;
                    var endDate = startDate.AddDays(30);

                    for (var date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        if (date.DayOfWeek.ToString() == dayOfWeek)
                        {
                            var schedule = new DoctorSchedule
                            {
                                Id = Guid.NewGuid(),
                                DoctorId = doctor.Id,
                                Date = date,
                                DayOfWeek = dayOfWeek,
                                IsActive = !isWeekend, // Weekends are inactive by default
                                ShiftStart = isWeekend ? TimeSpan.Zero : new TimeSpan(9, 0, 0), // 9:00 AM
                                ShiftEnd = isWeekend ? TimeSpan.Zero : new TimeSpan(17, 0, 0), // 5:00 PM
                                BreakStart = isWeekend ? null : new TimeSpan(12, 0, 0), // 12:00 PM
                                BreakEnd = isWeekend ? null : new TimeSpan(13, 0, 0), // 1:00 PM
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            schedules.Add(schedule);
                        }
                    }
                }
            }

            if (schedules.Any())
            {
                context.DoctorSchedules.AddRange(schedules);
                await context.SaveChangesAsync();
            }
        }
    }
}

