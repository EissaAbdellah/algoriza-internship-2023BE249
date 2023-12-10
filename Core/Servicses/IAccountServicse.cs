using Core.DTOS;
using Core.DTOS.Doctor;
using Core.DTOS.Patient;

namespace Core.Servicses
{
    public interface IAccountServicse
    {

        public Task<tokenDto> Login(LoginDto loginDto);

        public Task<bool> AddDoctor(RegisterDoctorDto registerDoctorDto);

        public Task<bool> Register(RegisterPatientDto registerPatientDto);


        //public Task<bool> AddRole(string roleName);

        //public Task<bool> AddAdmin(AdminDto adminDto);

    }
}
