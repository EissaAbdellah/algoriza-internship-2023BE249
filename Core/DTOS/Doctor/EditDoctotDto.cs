using System.ComponentModel.DataAnnotations;
using Core.Attributes;
using Core.Enums;
using Core.Settings;
using Microsoft.AspNetCore.Http;

namespace Core.DTOS.Doctor
{
    public class EditDoctotDto
    {

        [Required]
        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSize(FileSettings.MaxFileSizeInBytes, FileSettings.MaxFileSizeInMB)]
        public IFormFile ImageFile { get; set; }

        [Required]
        [DataType(DataType.Text, ErrorMessage = "Please enter a valid Name ")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text, ErrorMessage = "Please enter a valid Name ")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid  Email Adress")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid Phone number ")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date in the format yyyy-mm-dd")]
        [Required]
        public DateTime dateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public int SpecializationId { get; set; }


    }
}
