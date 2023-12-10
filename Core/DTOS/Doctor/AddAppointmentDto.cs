using System.ComponentModel.DataAnnotations;

namespace Core.DTOS.Doctor
{
    public class AddAppointmentDto
    {
        [Required]
        [DataType(DataType.Currency, ErrorMessage = "Please enter a valid Price ")]
        public decimal Price { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid ")]

        [Required]
        public DayOfWeek Day { get; set; }

        [Required]
        public List<int> Times { get; set; }


    }
}
