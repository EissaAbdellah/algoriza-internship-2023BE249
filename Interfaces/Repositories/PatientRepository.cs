using Core.DTOS;
using Core.DTOS.Patient;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Servicses;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {



        private readonly ApplicationDbContext _context;

        private readonly ICouponServicse _couponServicse;

        private readonly IStringLocalizer<PatientRepository> _localization;

        public PatientRepository(ApplicationDbContext context, ICouponServicse couponServicse, IStringLocalizer<PatientRepository> localization)
        {
            _context = context;
            _couponServicse = couponServicse;
            _localization = localization;
        }



        public async Task<List<DisplayAllAppointmentToPatientDto>> DisplayAllAppointmentToPatient(int page, int pageSize, string search)
        {

            IQueryable<Appointment> query = _context.Appointments
                                     .Where(d => d.ApplicationUser.RoleType == RoleType.Doctor)
                                      .Include(d => d.AppointmentTimeSlots)
                                      .Include(p => p.ApplicationUser);


            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d =>
                    d.ApplicationUser.FirstName.Contains(search) ||
                    d.ApplicationUser.LastName.Contains(search) ||
                    d.ApplicationUser.Email.Contains(search)
                );
            }


            List<DisplayAllAppointmentToPatientDto> diaplayDto = await query
                  .Skip((page - 1) * pageSize)
                   .Take(pageSize)
                .Select(doctorWithAppontment => new DisplayAllAppointmentToPatientDto
                {

                    Image = doctorWithAppontment.ApplicationUser.Image,
                    FullName = $"{doctorWithAppontment.ApplicationUser.FirstName} {doctorWithAppontment.ApplicationUser.LastName}",
                    Email = doctorWithAppontment.ApplicationUser.Email,
                    PhoneNumber = doctorWithAppontment.ApplicationUser.PhoneNumber,

                    //Localization En/ Ar
                    Specialization = _localization[doctorWithAppontment.ApplicationUser.Specialization.Name],
                    dateOfBirth = doctorWithAppontment.ApplicationUser.DateOfBirth.ToShortDateString(),

                    Gender = _localization[doctorWithAppontment.ApplicationUser.Gender.ToString()],

                    Price = doctorWithAppontment.Price,
                    Day = _localization[doctorWithAppontment.Day.ToString()],

                    AppointmentTimeSlot = doctorWithAppontment.AppointmentTimeSlots
                    .Select(t => new DisplayTimeSlotDto
                    {

                        Time = DateTime.Today.Add(t.Time).ToString("hh:mm tt"),
                        IsBooked = t.IsBooked
                    }).ToList()

                })
                .AsNoTracking()
                .ToListAsync();


            return diaplayDto;


        }



        public async Task<bool> BookAppointment(string PatientId, PatientBookingDto bookingDto, string discountCode)
        {
            //get the appointment Time
            var appointmentTimeSlots = _context.AppointmentTimeSlots
                                        .Include(a => a.Appointment)
                                        .SingleOrDefault(d => d.Id == bookingDto.AppointmentTimeId);




            if (appointmentTimeSlots is not null && appointmentTimeSlots.IsBooked == false)
            {


                int numOfRequests = _context.Bookings.Count(c => c.ApplicationUserId == PatientId);

                decimal actualPrice = appointmentTimeSlots.Appointment.Price;

                // Aplly the coupon

                decimal priceAfterDiscount = await _couponServicse.ApplyCoupon(PatientId, discountCode, numOfRequests, actualPrice);

                if (priceAfterDiscount == actualPrice)
                    discountCode = "";

                Booking booking = new Booking()
                {
                    IsConfirmed = false,
                    ApplicationUserId = PatientId,
                    AppointmentTimeSlotId = bookingDto.AppointmentTimeId,
                    Price = actualPrice,
                    FinalPrice = priceAfterDiscount,
                    DiscountCode = discountCode


                };


                await _context.Bookings.AddAsync(booking);
                if (await _context.SaveChangesAsync() > 0)
                {

                    // chaneg the state of the booking booked or not
                    var Timeslot = await _context.AppointmentTimeSlots.FirstOrDefaultAsync(x => x.Id == bookingDto.AppointmentTimeId);
                    Timeslot.IsBooked = true;
                    Timeslot.BookingId = booking.Id;


                    // update number of requests for the doctor


                    var doctorUser = _context.ApplicationUsers
                        .SingleOrDefault(s => s.Id == appointmentTimeSlots.Appointment.ApplicationUserId);

                    doctorUser.NumOfRequests += 1;
                    _context.Update(doctorUser);


                    _context.AppointmentTimeSlots.Update(Timeslot);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            return false;



        }


        public async Task<List<PatientDisplayBookingDto>> GetAllBooking(string id)
        {

            var bookings = await _context.Bookings
                         .AsNoTracking()
                         .Include(t => t.AppointmentTimeSlot)
                         .ThenInclude(a => a.Appointment)
                         .ThenInclude(u => u.ApplicationUser)
                         .ThenInclude(s => s.Specialization)
                         .Where(b => b.ApplicationUserId == id)
                         .ToListAsync();


            List<PatientDisplayBookingDto> bookingsDto = bookings
                 .Select(booking => new PatientDisplayBookingDto
                 {

                     Image = booking.AppointmentTimeSlot.Appointment.ApplicationUser.Image,
                     DoctorFullName = $"{booking.AppointmentTimeSlot.Appointment.ApplicationUser.FirstName} {booking.AppointmentTimeSlot.Appointment.ApplicationUser.LastName}",

                     Specialization = _localization[booking.AppointmentTimeSlot.Appointment.ApplicationUser.Specialization.Name],
                     Day = _localization[booking.AppointmentTimeSlot.Appointment.Day.ToString()],

                     Time = DateTime.Today.Add(booking.AppointmentTimeSlot.Time).ToString("hh:mm tt"),
                     Price = booking.Price,
                     FinalPrice = booking.FinalPrice,
                     Status = booking.Status.ToString(),
                     DiscoundCode = booking.DiscountCode


                 }
                 ).ToList();


            return bookingsDto;



        }


        public async Task<bool> CancelBooking(int bookingId)
        {

            Booking booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
            // check the state of the booking
            if (booking is not null && booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Cancelled;


                // Update timeslot to be available

                var time = await _context.AppointmentTimeSlots.FirstOrDefaultAsync(t => t.BookingId == bookingId);

                time.BookingId = 0;
                time.IsBooked = false;

                _context.Bookings.Update(booking);
                _context.AppointmentTimeSlots.Update(time);

                if (await _context.SaveChangesAsync() > 0)
                    return true;


            }

            return false;

        }

    }
}
