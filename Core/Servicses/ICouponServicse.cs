namespace Core.Servicses
{
    public interface ICouponServicse
    {

        public Task<decimal> ApplyCoupon(string patientId, string discountCode, int numRequests, decimal price);


    }
}
