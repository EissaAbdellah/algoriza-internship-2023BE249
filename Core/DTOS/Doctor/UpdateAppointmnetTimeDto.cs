using System.ComponentModel.DataAnnotations;

namespace Core.DTOS.Doctor
{
    public class UpdateAppointmnetTimeDto
    {

        [Required]
        public int AppointmentTime { get; set; }
    }
}
