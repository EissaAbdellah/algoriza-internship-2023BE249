using Core.Enums;
using Core.Servicses;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CouponServicse : ICouponServicse
    {
        private readonly ApplicationDbContext _context;
        public CouponServicse(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> ApplyCoupon(string patientId, string discountCode, int numRequests, decimal price)
        {



            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.DiscoundCode == discountCode);
            decimal discountValue = 0;

            decimal finalPrice = 0;

            // check if the user used this coupon before !
            var lastUserBookings = _context.Bookings.Where(u => u.ApplicationUserId == patientId);

            bool isUsedCoupon = lastUserBookings.Any(x => x.DiscountCode == discountCode);

            // check the coupon Validation
            if (coupon is not null && coupon.NumOfRequests <= numRequests && coupon.isActived == true && isUsedCoupon == false)
            {
                // check the type of the coupon
                if (coupon.DiscountType == DiscountType.Percentage)
                {
                    discountValue = (coupon.Value / 100) * price;

                    finalPrice = price - discountValue;

                    coupon.isApplied = true;
                    _context.Coupons.Update(coupon);
                    await _context.SaveChangesAsync();
                    return finalPrice;

                }
                if (coupon.DiscountType == DiscountType.Value)
                {
                    discountValue = coupon.Value;

                    finalPrice = price - discountValue;
                    coupon.isApplied = true;
                    _context.Coupons.Update(coupon);
                    return finalPrice;

                }

            }


            return price;
        }
    }
}
