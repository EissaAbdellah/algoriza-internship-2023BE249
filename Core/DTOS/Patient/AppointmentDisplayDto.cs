namespace Core.DTOS.Patient
{
    public class AppointmentDisplayDto
    {

        public decimal Price { get; set; }

        public bool IsBooked { get; set; }

        public DayOfWeek Day { get; set; }
    }
}
