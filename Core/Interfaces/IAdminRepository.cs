using Core.DTOS;
using Core.DTOS.Admin;
using Core.DTOS.Doctor;
using Core.DTOS.Patient;

namespace Core.Interfaces
{
    public interface IAdminRepository
    {



        //Page/size/Search
        public Task<List<DisplayAllDoctorsDto>> GetAllDoctors(int page, int pageSize, string search);

        public Task<DisplayAllDoctorsDto> GetDoctorById(string Id);

        public Task<bool> EditDoctor(EditDoctotDto editDoctotDto, string Id);

        public Task<bool> DeleteDoctor(string Id);

        public Task<int> NumOfDoctors();

        public Task<int> NumOfPatients();

        public Task<NumOfRequestsDto> NumOfRequests();

        public Task<List<TopSpecializations>> TopFiveSpecializations();

        public Task<List<TopDotorsDto>> TopTenDotors();



        public Task<List<DisplayAllPatientsDto>> GetAllPatients(int page, int pageSize, string search);
        public Task<DisplayAllPatientsDto> GetPatientById(string Id);


        public Task<bool> AddCoupon(CouponDto couponDto);
        public Task<bool> UpdateCoupon(int couponId, CouponDto couponDto);
        public Task<bool> DeleteCoupon(int couponId);
        public Task<bool> DeactiveCoupon(int couponId);







    }
}
