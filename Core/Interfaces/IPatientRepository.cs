using Core.DTOS.Patient;

namespace Core.Interfaces
{
    public interface IPatientRepository
    {
        public Task<List<DisplayAllAppointmentToPatientDto>> DisplayAllAppointmentToPatient(int page, int pageSize, string search);

        public Task<bool> BookAppointment(string PatientId, PatientBookingDto bookingDto, string discountCode);

        public Task<List<PatientDisplayBookingDto>> GetAllBooking(string id);

        public Task<bool> CancelBooking(int bookingId);

    }
}
