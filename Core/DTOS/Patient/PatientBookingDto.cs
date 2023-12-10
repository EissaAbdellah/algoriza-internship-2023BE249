using System.ComponentModel.DataAnnotations;

namespace Core.DTOS.Patient
{
    public class PatientBookingDto
    {


        [Required]
        public int AppointmentTimeId { get; set; }


    }
}
