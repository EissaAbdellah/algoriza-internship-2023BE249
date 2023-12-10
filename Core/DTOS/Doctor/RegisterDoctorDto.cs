using System.ComponentModel.DataAnnotations;
using Core.Attributes;
using Core.Enums;
using Core.Settings;
using Microsoft.AspNetCore.Http;

namespace Core.DTOS.Doctor
{
    public class RegisterDoctorDto
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email Address ")]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid Phone number ")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Name ")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Name ")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$",
          ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.")]
        public string Password { get; set; }


        [Required]
        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSize(FileSettings.MaxFileSizeInBytes, FileSettings.MaxFileSizeInMB)]
        public IFormFile ImageFile { get; set; }

        [Required]
        public Gender Gender { get; set; }


        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date in the format yyyy-mm-dd")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int SpecializationId { get; set; }


    }
}
