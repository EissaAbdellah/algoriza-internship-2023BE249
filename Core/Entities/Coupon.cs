using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Entities
{
    public class Coupon
    {

        public int Id { get; set; }

        [Required]
        public string DiscoundCode { get; set; }

        public int NumOfRequests { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Required]
        public decimal Value { get; set; }

        //Haveing defaul value 
        public bool isApplied { get; set; }

        public bool isActived { get; set; }





    }
}
