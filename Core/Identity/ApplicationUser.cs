using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core.Entities;
using Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace Core.Identity
{
    public class ApplicationUser : IdentityUser
    {

        [MaxLength(100)]
        [Required(ErrorMessage = "First Name is Required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Last Name is Required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date of Birth is Required")]
        public DateTime DateOfBirth { get; set; }


        [AllowNull]
        public string Image { get; set; }


        [Required(ErrorMessage = "Role is Required")]
        public RoleType RoleType { get; set; }


        [AllowNull]
        public int? SpecializationId { get; set; }

        public Specialization Specialization { get; set; } = default!;

        //for the doctor user
        [AllowNull]
        public ICollection<Appointment> Appointments { get; set; } = default!;

        [AllowNull]
        public ICollection<Booking> Bookings { get; set; } = default!;

        // Has Defaut value => 0 configueed in the DbContext
        public int NumOfRequests { get; set; }


    }
}
