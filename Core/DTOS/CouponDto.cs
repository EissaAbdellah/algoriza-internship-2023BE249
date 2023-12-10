using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.DTOS
{
    public class CouponDto
    {
        [Required]
        public string DiscoundCode { get; set; }

        [Required]
        public int NumOfRequests { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Required(ErrorMessage = "Value is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than zero")]
        public decimal Value { get; set; }


    }
}
