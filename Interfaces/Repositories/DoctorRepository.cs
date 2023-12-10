using Core.DTOS.Doctor;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;



        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;

        }



        public async Task<List<DisplayAllAppointmentDto>> GetAll(int page, int pageSize, string doctorId, System.DayOfWeek day)
        {
            // get the booking for the docotro in the day and if it status pending only
            var appointments = _context.Bookings
                             .Where(d => d.AppointmentTimeSlot.Appointment.Day == day)
                             .Where(d => d.Status == BookingStatus.Pending)
                             .Where(d => d.AppointmentTimeSlot.Appointment.ApplicationUserId == doctorId)
                              .Include(d => d.AppointmentTimeSlot)
                              .ThenInclude(d => d.Appointment);


            List<DisplayAllAppointmentDto> displayAllAppointmentDto = await appointments
                  .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                .Select(appointment => new DisplayAllAppointmentDto
                {
                    PatientName = $"{appointment.ApplicationUser.FirstName} {appointment.ApplicationUser.LastName}",
                    Image = appointment.ApplicationUser.Image,
                    Age = CalculateAge(appointment.ApplicationUser.DateOfBirth),
                    Gender = appointment.ApplicationUser.Gender.ToString(),
                    Email = appointment.ApplicationUser.Email,
                    Phone = appointment.ApplicationUser.PhoneNumber,
                    Appointment = DateTime.Today.Add(appointment.AppointmentTimeSlot.Time).ToString("hh:mm tt")


                })
                 .AsNoTracking()
                 .ToListAsync();


            return displayAllAppointmentDto;


        }


        public async Task<bool> ConfirmCheckUp(int bookingId)
        {

            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            booking.Status = BookingStatus.Completed;
            booking.IsConfirmed = true;


            var timeSlot = await _context.AppointmentTimeSlots.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            timeSlot.IsBooked = false;

            _context.Bookings.Update(booking);
            _context.AppointmentTimeSlots.Update(timeSlot);

            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;
        }



        public async Task<bool> AddAppointment(AddAppointmentDto appointmentDto, string doctorId)
        {

            Appointment appointment = new()
            {

                Price = appointmentDto.Price,
                Day = appointmentDto.Day,
                ApplicationUserId = doctorId

            };


            await _context.Appointments.AddAsync(appointment);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {

                foreach (var item in appointmentDto.Times)
                {
                    AppointmentTimeSlot timeSlot = new()
                    {
                        AppointmentId = appointment.Id,
                        Time = new TimeSpan(item, 0, 0)


                    };
                    await _context.AppointmentTimeSlots.AddAsync(timeSlot);
                }
                await _context.SaveChangesAsync();


                return true;
            }


            return false;

        }


        public async Task<bool> UpdateAppontment(int AppointmentTimeId, UpdateAppointmnetTimeDto timeDto)
        {
            var AppointmentTime = await _context.AppointmentTimeSlots.FirstOrDefaultAsync(t => t.Id == AppointmentTimeId);

            // check if the time not booked
            if (AppointmentTime is not null && AppointmentTime.IsBooked == false)
            {
                AppointmentTime.Time = new TimeSpan(timeDto.AppointmentTime, 0, 0);
                _context.Update(AppointmentTime);
                if (await _context.SaveChangesAsync() > 0)
                    return true;

            }

            return false;
        }

        public async Task<bool> DeleteAppontment(int AppointmentTimeId)
        {
            var AppointmentTime = await _context.AppointmentTimeSlots.FirstOrDefaultAsync(t => t.Id == AppointmentTimeId);

            // check if the time not booked
            if (AppointmentTime is not null && AppointmentTime.IsBooked == false)
            {

                _context.Remove(AppointmentTime);
                if (await _context.SaveChangesAsync() > 0)
                    return true;

            }

            return false;
        }

        private static int CalculateAge(DateTime birthdate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthdate.Year;

            // Check if the birthday has occurred this year
            if (birthdate.Date > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }


    }
}
