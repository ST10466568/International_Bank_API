using Microsoft.EntityFrameworkCore;
using HopewellClinicApi.Data;
using HopewellClinicApi.Models;
using HopewellClinicApi.DTOs;

namespace HopewellClinicApi.Services
{
    public class DoctorScheduleService
    {
        private readonly HopewellDbContext _context;
        private readonly ILogger<DoctorScheduleService> _logger;

        public DoctorScheduleService(HopewellDbContext context, ILogger<DoctorScheduleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get doctor's weekly schedule
        public async Task<DoctorScheduleManagementResponse> GetDoctorScheduleAsync(Guid doctorId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var doctor = await _context.Staff
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == doctorId);

            if (doctor == null)
            {
                throw new ArgumentException("Doctor not found");
            }

            var query = _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId);

            if (startDate.HasValue)
            {
                query = query.Where(ds => ds.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(ds => ds.Date <= endDate.Value.Date);
            }

            var schedules = await query
                .OrderBy(ds => ds.DayOfWeek)
                .ThenBy(ds => ds.Date)
                .Select(ds => new DoctorScheduleDto
                {
                    Id = ds.Id,
                    DoctorId = ds.DoctorId,
                    DayOfWeek = ds.DayOfWeek,
                    IsActive = ds.IsActive,
                    ShiftStart = ds.ShiftStart,
                    ShiftEnd = ds.ShiftEnd,
                    BreakStart = ds.BreakStart,
                    BreakEnd = ds.BreakEnd,
                    CreatedAt = ds.CreatedAt,
                    UpdatedAt = ds.UpdatedAt
                })
                .ToListAsync();

            return new DoctorScheduleManagementResponse
            {
                DoctorId = doctorId,
                DoctorName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                WeeklySchedule = schedules
            };
        }

        // Update doctor's schedule
        public async Task<DoctorScheduleManagementResponse> UpdateDoctorScheduleAsync(Guid doctorId, UpdateDoctorScheduleRequest request)
        {
            var doctor = await _context.Staff
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == doctorId);

            if (doctor == null)
            {
                throw new ArgumentException("Doctor not found");
            }

            // Remove existing schedules for this doctor
            var existingSchedules = await _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId)
                .ToListAsync();

            _context.DoctorSchedules.RemoveRange(existingSchedules);

            // Add new schedules
            var newSchedules = new List<DoctorSchedule>();
            var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            foreach (var dayOfWeek in daysOfWeek)
            {
                var scheduleItem = request.Schedule.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);
                
                if (scheduleItem != null)
                {
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
                                DoctorId = doctorId,
                                Date = date,
                                DayOfWeek = dayOfWeek,
                                IsActive = scheduleItem.IsActive,
                                ShiftStart = scheduleItem.ShiftStart,
                                ShiftEnd = scheduleItem.ShiftEnd,
                                BreakStart = scheduleItem.BreakStart,
                                BreakEnd = scheduleItem.BreakEnd,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };

                            newSchedules.Add(schedule);
                        }
                    }
                }
            }

            _context.DoctorSchedules.AddRange(newSchedules);
            await _context.SaveChangesAsync();

            return await GetDoctorScheduleAsync(doctorId);
        }

        // Check if doctor is available at specific time
        public async Task<DoctorAvailabilityManagementResponse> CheckDoctorAvailabilityAsync(Guid doctorId, DateTime date, TimeSpan? time = null)
        {
            var dayOfWeek = date.DayOfWeek.ToString();
            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(ds => ds.DoctorId == doctorId && 
                                         ds.DayOfWeek == dayOfWeek && 
                                         ds.Date == date.Date && 
                                         ds.IsActive);

            if (schedule == null)
            {
                return new DoctorAvailabilityManagementResponse
                {
                    IsAvailable = false,
                    Reason = "Doctor is not scheduled to work on this day"
                };
            }

            if (time.HasValue)
            {
                // Check if time is within shift hours
                if (time.Value < schedule.ShiftStart || time.Value >= schedule.ShiftEnd)
                {
                    return new DoctorAvailabilityManagementResponse
                    {
                        IsAvailable = false,
                        Reason = "Time is outside doctor's shift hours",
                        NextAvailableTime = schedule.ShiftStart
                    };
                }

                // Check if time is during break
                if (schedule.BreakStart.HasValue && schedule.BreakEnd.HasValue)
                {
                    if (time.Value >= schedule.BreakStart.Value && time.Value < schedule.BreakEnd.Value)
                    {
                        return new DoctorAvailabilityManagementResponse
                        {
                            IsAvailable = false,
                            Reason = "Doctor is on break at this time",
                            NextAvailableTime = schedule.BreakEnd.Value
                        };
                    }
                }

                // Check for conflicting appointments
                var conflictingAppointment = await _context.Appointments
                    .Include(a => a.Patient)
                        .ThenInclude(p => p.User)
                    .Include(a => a.Service)
                    .FirstOrDefaultAsync(a => a.StaffId == doctorId &&
                                             a.AppointmentDate == date.Date &&
                                             a.StartTime <= TimeOnly.FromTimeSpan(time.Value) &&
                                             a.EndTime > TimeOnly.FromTimeSpan(time.Value));

                if (conflictingAppointment != null)
                {
                    return new DoctorAvailabilityManagementResponse
                    {
                        IsAvailable = false,
                        Reason = $"Doctor has an appointment with {conflictingAppointment.Patient.User.FirstName} {conflictingAppointment.Patient.User.LastName}",
                        NextAvailableTime = conflictingAppointment.EndTime.ToTimeSpan()
                    };
                }
            }

            // Generate available slots for the day
            var availableSlots = await GenerateAvailableSlotsAsync(doctorId, date, 30);

            return new DoctorAvailabilityManagementResponse
            {
                IsAvailable = true,
                Reason = "Doctor is available",
                AvailableSlots = availableSlots.Select(s => s.StartTime).ToList()
            };
        }

        // Generate available time slots for a doctor on a specific date
        public async Task<List<TimeSlotEnhancedDto>> GenerateAvailableSlotsAsync(Guid doctorId, DateTime date, int serviceDuration = 30)
        {
            var dayOfWeek = date.DayOfWeek.ToString();
            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(ds => ds.DoctorId == doctorId && 
                                         ds.DayOfWeek == dayOfWeek && 
                                         ds.Date == date.Date && 
                                         ds.IsActive);

            if (schedule == null)
            {
                return new List<TimeSlotEnhancedDto>();
            }

            // Get existing appointments for this doctor on this date
            var existingAppointments = await _context.Appointments
                .Where(a => a.StaffId == doctorId && a.AppointmentDate == date.Date)
                .ToListAsync();

            var slots = new List<TimeSlotEnhancedDto>();
            var currentTime = schedule.ShiftStart;

            while (currentTime.Add(TimeSpan.FromMinutes(serviceDuration)) <= schedule.ShiftEnd)
            {
                var endTime = currentTime.Add(TimeSpan.FromMinutes(serviceDuration));
                var isDuringBreak = false;
                var conflictReason = string.Empty;

                // Check if slot is during break time
                if (schedule.BreakStart.HasValue && schedule.BreakEnd.HasValue)
                {
                    if (currentTime < schedule.BreakEnd.Value && endTime > schedule.BreakStart.Value)
                    {
                        isDuringBreak = true;
                        conflictReason = "During break time";
                    }
                }

                // Check for conflicting appointments
                var isAvailable = true;
                if (!isDuringBreak)
                {
                    var hasConflict = existingAppointments.Any(a =>
                        a.StartTime < TimeOnly.FromTimeSpan(endTime) && 
                        a.EndTime > TimeOnly.FromTimeSpan(currentTime));

                    if (hasConflict)
                    {
                        isAvailable = false;
                        conflictReason = "Appointment conflict";
                    }
                }

                slots.Add(new TimeSlotEnhancedDto
                {
                    Id = Guid.NewGuid(),
                    DoctorId = doctorId,
                    Date = date,
                    StartTime = currentTime,
                    EndTime = endTime,
                    Duration = serviceDuration,
                    IsAvailable = isAvailable && !isDuringBreak,
                    DayOfWeek = dayOfWeek,
                    IsDuringBreak = isDuringBreak,
                    ConflictReason = conflictReason
                });

                currentTime = currentTime.Add(TimeSpan.FromMinutes(serviceDuration));
            }

            return slots;
        }

        // Get doctors on duty for a specific date
        public async Task<List<DoctorOnDutyEnhancedDto>> GetDoctorsOnDutyAsync(DateTime date, Guid? serviceId = null)
        {
            var dayOfWeek = date.DayOfWeek.ToString();
            
            var doctors = await _context.Staff
                .Include(s => s.User)
                .Where(s => s.IsActive)
                .Join(_context.DoctorSchedules,
                    s => s.Id,
                    ds => ds.DoctorId,
                    (s, ds) => new { Staff = s, Schedule = ds })
                .Where(x => x.Schedule.DayOfWeek == dayOfWeek && 
                           x.Schedule.Date == date.Date && 
                           x.Schedule.IsActive)
                .Select(x => new DoctorOnDutyEnhancedDto
                {
                    Id = x.Staff.Id,
                    FirstName = x.Staff.User.FirstName,
                    LastName = x.Staff.User.LastName,
                    Specialty = "General Practice", // Default specialty
                    Rating = 4.5, // Default rating
                    ShiftStart = x.Schedule.ShiftStart,
                    ShiftEnd = x.Schedule.ShiftEnd,
                    IsAvailable = true,
                    Services = new List<string> { "consultation", "follow-up" },
                    DayOfWeek = x.Schedule.DayOfWeek,
                    BreakStart = x.Schedule.BreakStart,
                    BreakEnd = x.Schedule.BreakEnd
                })
                .ToListAsync();

            return doctors;
        }

        // Get doctor schedule summary
        public async Task<DoctorScheduleSummaryDto> GetDoctorScheduleSummaryAsync(Guid doctorId)
        {
            var doctor = await _context.Staff
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == doctorId);

            if (doctor == null)
            {
                throw new ArgumentException("Doctor not found");
            }

            var schedules = await _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId && ds.IsActive)
                .GroupBy(ds => ds.DayOfWeek)
                .Select(g => new
                {
                    DayOfWeek = g.Key,
                    Schedule = g.First()
                })
                .ToListAsync();

            var workingDays = schedules.Select(s => s.DayOfWeek).ToList();
            var allDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var offDays = allDays.Except(workingDays).ToList();

            var totalWeeklyHours = schedules.Sum(s => 
                s.Schedule.ShiftEnd.Subtract(s.Schedule.ShiftStart).TotalHours);

            var lastUpdated = schedules.Any() ? schedules.Max(s => s.Schedule.UpdatedAt) : DateTime.UtcNow;

            return new DoctorScheduleSummaryDto
            {
                DoctorId = doctorId,
                DoctorName = $"{doctor.User.FirstName} {doctor.User.LastName}",
                Specialty = "General Practice",
                ActiveDays = workingDays.Count,
                TotalWeeklyHours = TimeSpan.FromHours(totalWeeklyHours),
                WorkingDays = workingDays,
                OffDays = offDays,
                LastUpdated = lastUpdated
            };
        }

        // Initialize default schedules for a doctor
        public async Task InitializeDefaultScheduleAsync(Guid doctorId)
        {
            var existingSchedules = await _context.DoctorSchedules
                .Where(ds => ds.DoctorId == doctorId)
                .ToListAsync();

            if (existingSchedules.Any())
            {
                return; // Doctor already has schedules
            }

            var daysOfWeek = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var newSchedules = new List<DoctorSchedule>();

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
                            DoctorId = doctorId,
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

                        newSchedules.Add(schedule);
                    }
                }
            }

            _context.DoctorSchedules.AddRange(newSchedules);
            await _context.SaveChangesAsync();
        }
    }
}
