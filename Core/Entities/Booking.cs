using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core.Enums;
using Core.Identity;

namespace Core.Entities
{
    public class Booking
    {


        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal FinalPrice { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        [AllowNull]
        public string DiscountCode { get; set; }

        [Required]
        public bool IsConfirmed { get; set; }

        // Navigation property to represent the associated patient
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        // Foreign key to link with Appointment, Navigation property to represent the appointment
        public int AppointmentTimeSlotId { get; set; }
        public AppointmentTimeSlot AppointmentTimeSlot { get; set; }



    }
}
