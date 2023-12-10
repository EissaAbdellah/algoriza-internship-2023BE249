using Core.DTOS.Doctor;

namespace Core.Interfaces
{
    public interface IDoctorRepository
    {


        public Task<List<DisplayAllAppointmentDto>> GetAll(int page, int pageSize, string doctorId, DayOfWeek day);

        public Task<bool> ConfirmCheckUp(int bookingId);

        public Task<bool> AddAppointment(AddAppointmentDto appointmentDto, string doctorId);

        public Task<bool> UpdateAppontment(int AppointmentTimeId, UpdateAppointmnetTimeDto timeDto)
            ;
        public Task<bool> DeleteAppontment(int AppointmentTimeId);


    }
}
