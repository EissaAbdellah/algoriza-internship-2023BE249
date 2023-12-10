using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class AppointmentTimeSlot
    {
        [Key]
        public int Id { get; set; }

        public bool IsBooked { get; set; } = false;

        [Required]
        public TimeSpan Time { get; set; }

        // Foreign key to link with Appointment
        public int AppointmentId { get; set; }
        // Navigation property to represent the appointment
        public Appointment Appointment { get; set; }


        // Foreign key to link with Booking , Navigation property to represent the Booking
        public int BookingId { get; set; }

        // Navigation property to represent the Bookings associated with this time slot
        public ICollection<Booking> Bookings { get; set; }


    }
}
