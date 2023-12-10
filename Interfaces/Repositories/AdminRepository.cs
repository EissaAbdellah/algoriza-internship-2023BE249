using Core.DTOS;
using Core.DTOS.Admin;
using Core.DTOS.Doctor;
using Core.DTOS.Patient;
using Core.Entities;
using Core.Enums;
using Core.Identity;
using Core.Interfaces;
using Core.Servicses;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IImageServicse _imageServicse;


        public AdminRepository(ApplicationDbContext context, IImageServicse imageServicse)
        {
            _context = context;
            _imageServicse = imageServicse;
        }


        public async Task<int> NumOfDoctors()
        {

            int num = 0;
            num = await _context.Users
                .AsNoTracking()
                 .CountAsync(d => d.RoleType == RoleType.Doctor);

            return num;

        }


        public async Task<int> NumOfPatients()
        {
            int num = 0;
            num = await _context.Users
                .AsNoTracking()
                 .CountAsync(d => d.RoleType == RoleType.Patient);

            return num;

        }



        public async Task<NumOfRequestsDto> NumOfRequests()
        {
            int numOfrequests = await _context.Bookings.CountAsync();
            int numOfPendingrequests = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);
            int numOCompletedrequests = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Completed);
            int numOCancelledrequests = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.Cancelled);

            NumOfRequestsDto numOfRequestsDto = new NumOfRequestsDto()
            {
                NumOfRequests = numOfrequests,
                NumOfPendingRequests = numOfPendingrequests,
                NumOfCompletedRequests = numOCompletedrequests,
                NumOfCancelledRequests = numOCancelledrequests
            };

            return numOfRequestsDto;


        }


        public async Task<List<TopSpecializations>> TopFiveSpecializations()
        {


            var topSpecializations = await _context.Specializations
                                      .OrderByDescending(s => s.Doctors.Sum(u => u.NumOfRequests))
                                      .Take(5)
                                      .Select(s => new TopSpecializations
                                      {
                                          Name = s.Name,
                                          NumberOfRequests = s.Doctors.Sum(u => u.NumOfRequests)
                                      })
                                      .ToListAsync();


            if (topSpecializations is not null)
                return topSpecializations;

            return new List<TopSpecializations>() { };

        }


        public async Task<List<TopDotorsDto>> TopTenDotors()
        {

            var topDoctroRequested = await _context.ApplicationUsers
                .AsNoTracking()
                .Include(s => s.Specialization)
                .Where(d => d.RoleType == RoleType.Doctor)
                .OrderByDescending(a => a.NumOfRequests)
                .Take(10)
                .ToListAsync();



            List<TopDotorsDto> topDotorsDto = topDoctroRequested.Select(d => new TopDotorsDto

            {
                Image = d.Image,
                FullName = $"{d.FirstName} {d.LastName}",
                NumOfRequests = d.NumOfRequests,
                specialize = d.Specialization.Name


            }).ToList();




            return topDotorsDto;
        }


        public async Task<List<DisplayAllDoctorsDto>> GetAllDoctors(int page, int pageSize, string search)
        {

            IQueryable<ApplicationUser> query = _context.Users
                                      .Where(d => d.RoleType == RoleType.Doctor)
                                       .Include(d => d.Specialization);
            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d =>
                    d.FirstName.Contains(search) ||
                    d.LastName.Contains(search) ||
                    d.Email.Contains(search)
                );
            }

            List<DisplayAllDoctorsDto> doctorsDtoList = await query
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .Select(doctor => new DisplayAllDoctorsDto
                         {
                             Image = doctor.Image,
                             FullName = $"{doctor.FirstName} {doctor.LastName}",
                             Email = doctor.Email,
                             PhoneNumber = doctor.PhoneNumber,
                             Specialization = doctor.Specialization.Name,
                             dateOfBirth = doctor.DateOfBirth.ToShortDateString(),
                             Gender = doctor.Gender.ToString()
                         })
                            .AsNoTracking()
                            .ToListAsync();

            return doctorsDtoList;


        }

        public async Task<DisplayAllDoctorsDto> GetDoctorById(string Id)
        {
            DisplayAllDoctorsDto doctorDto =
                await _context.Users
                .Where(d => d.Id == Id && d.RoleType == RoleType.Doctor)
                .Include(d => d.Specialization)
                .Select(doctor => new DisplayAllDoctorsDto
                {

                    Image = doctor.Image,
                    FullName = $"{doctor.FirstName} {doctor.LastName}",
                    Email = doctor.Email,
                    PhoneNumber = doctor.PhoneNumber,
                    Specialization = doctor.Specialization.Name,
                    dateOfBirth = doctor.DateOfBirth.ToShortDateString(),
                    Gender = doctor.Gender.ToString()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();



            return doctorDto;

        }



        public async Task<List<DisplayAllPatientsDto>> GetAllPatients(int page, int pageSize, string search)
        {

            IQueryable<ApplicationUser> query = _context.Users
                                      .Where(d => d.RoleType == RoleType.Patient)
                                       .Include(d => d.Specialization);
            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d =>
                    d.FirstName.Contains(search) ||
                    d.LastName.Contains(search) ||
                    d.Email.Contains(search)
                );
            }

            List<DisplayAllPatientsDto> patientsDtoList = await query
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .Select(doctor => new DisplayAllPatientsDto
                         {
                             Image = doctor.Image,
                             FullName = $"{doctor.FirstName} {doctor.LastName}",
                             Email = doctor.Email,
                             PhoneNumber = doctor.PhoneNumber,
                             Gender = doctor.Gender.ToString(),
                             dateOfBirth = doctor.DateOfBirth.ToShortDateString()
                         })
                            .AsNoTracking()
                            .ToListAsync();

            return patientsDtoList;


        }

        public async Task<DisplayAllPatientsDto> GetPatientById(string Id)
        {
            DisplayAllPatientsDto patientDto =
                await _context.Users
                .Where(d => d.Id == Id && d.RoleType == RoleType.Patient)
                .Include(d => d.Specialization)
                .Select(doctor => new DisplayAllPatientsDto
                {

                    Image = doctor.Image,
                    FullName = $"{doctor.FirstName} {doctor.LastName}",
                    Email = doctor.Email,
                    PhoneNumber = doctor.PhoneNumber,
                    dateOfBirth = doctor.DateOfBirth.ToShortDateString(),
                    Gender = doctor.Gender.ToString()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return patientDto;


        }




        public async Task<bool> EditDoctor(EditDoctotDto editDoctotDto, string doctorId)
        {

            var Doctor = await _context.Users
                 .Where(d => d.RoleType == RoleType.Doctor)
                .FirstOrDefaultAsync(d => d.Id == doctorId);
            if (Doctor is not null)
            {

                Doctor.FirstName = editDoctotDto.FirstName;
                Doctor.LastName = editDoctotDto.LastName;
                Doctor.Email = editDoctotDto.Email;
                Doctor.UserName = editDoctotDto.Email;
                Doctor.PhoneNumber = editDoctotDto.PhoneNumber;
                Doctor.Gender = editDoctotDto.Gender;
                Doctor.DateOfBirth = editDoctotDto.dateOfBirth;
                Doctor.SpecializationId = Doctor.SpecializationId;

                // save image
                var imagePath = await _imageServicse.saveImage(editDoctotDto.ImageFile, "doctors");

                Doctor.Image = imagePath;




                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }

            }
            return false;

        }


        public async Task<bool> DeleteDoctor(string doctorId)
        {
            //check if doctor have any reuest (booking)
            var haveBookings = _context.Bookings.Any(d => d.AppointmentTimeSlot.Appointment.ApplicationUserId == doctorId);

            //check if the doctor is not null
            var Doctor = await _context.Users
                 .Where(d => d.RoleType == RoleType.Doctor)
                .FirstOrDefaultAsync(d => d.Id == doctorId);

            if (Doctor is not null && !haveBookings)
            {
                _context.Users.Remove(Doctor);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return true;
                }

            }
            return false;

        }





        public async Task<bool> AddCoupon(CouponDto couponDto)
        {
            Coupon Newcoupon = new Coupon()
            {
                DiscoundCode = couponDto.DiscoundCode,
                NumOfRequests = couponDto.NumOfRequests,
                DiscountType = couponDto.DiscountType,
                Value = couponDto.Value
            };

            await _context.Coupons.AddAsync(Newcoupon);
            if (await _context.SaveChangesAsync() > 0)
                return true;

            return false;

        }

        public async Task<bool> UpdateCoupon(int couponId, CouponDto couponDto)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Id == couponId);
            if (!coupon.isApplied)
            {
                coupon.DiscoundCode = couponDto.DiscoundCode;
                coupon.NumOfRequests = couponDto.NumOfRequests;
                coupon.DiscountType = couponDto.DiscountType;
                coupon.Value = couponDto.Value;
                _context.Coupons.Update(coupon);

                if (await _context.SaveChangesAsync() > 0)
                    return true;
            }
            return false;
        }

        public async Task<bool> DeleteCoupon(int couponId)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Id == couponId);
            if (coupon is not null && coupon.isApplied == false)
            {
                _context.Coupons.Remove(coupon);
                if (await _context.SaveChangesAsync() > 0)
                    return true;
            }
            return false;

        }

        public async Task<bool> DeactiveCoupon(int couponId)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Id == couponId);
            if (coupon is not null)
            {
                coupon.isActived = false;
                _context.Coupons.Update(coupon);
                if (await _context.SaveChangesAsync() > 0)
                    return true;
            }
            return false;
        }



    }
}
