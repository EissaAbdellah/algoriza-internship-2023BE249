namespace Core.DTOS.Patient
{
    public class BookingToPatientDto
    {

        public string Image { get; set; }

        public string FullName { get; set; }

        public string Specialization { get; set; }

        public DayOfWeek Day { get; set; }

        public decimal Price { get; set; }

        public decimal FinalPrice { get; set; }

        public string DiscountCode { get; set; }

        public string Status { get; set; }

        public List<DisplayTimeSlotDto> AppointmentTimeSlot { get; set; }


    }
}
