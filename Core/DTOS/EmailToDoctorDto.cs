using System.ComponentModel.DataAnnotations;

namespace Core.DTOS
{
    public class EmailToDoctorDto
    {

        [Required]
        public string ToEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }


    }
}
