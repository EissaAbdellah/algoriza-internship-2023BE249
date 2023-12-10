using System.ComponentModel.DataAnnotations;
using Core.Identity;

namespace Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than or equal to $")]
        public decimal Price { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        // Navigation property to represent the associated doctor
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<AppointmentTimeSlot> AppointmentTimeSlots { get; set; } = new List<AppointmentTimeSlot>();



    }
}
