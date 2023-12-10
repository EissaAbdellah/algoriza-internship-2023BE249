namespace Core.DTOS.Patient
{
    public class DisplayAllAppointmentToPatientDto
    {

        public string Image { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string dateOfBirth { get; set; }

        public string Specialization { get; set; }

        public string Gender { get; set; }

        public decimal Price { get; set; }



        public string Day { get; set; }

        public List<DisplayTimeSlotDto> AppointmentTimeSlot { get; set; }


    }
}
