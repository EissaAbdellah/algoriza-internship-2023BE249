namespace Core.DTOS.Patient
{
    public class PatientDisplayBookingDto
    {
        public string Image { get; set; }

        public string DoctorFullName { get; set; }

        public string Specialization { get; set; }

        public string Day { get; set; }

        public string Time { get; set; }

        public decimal Price { get; set; }

        public string DiscoundCode { get; set; }

        public decimal FinalPrice { get; set; }


        public string Status { get; set; }



    }
}
