using MagicalVilla_CoponAPI.models;

namespace MagicalVilla_CoponAPI.Data
{
    public class CouponStore
    {
        public static List<Coupon> coupons= new List<Coupon>
        {
            new Coupon{Id =1, Name="1000f", Percent=10, IsActive=true},
            new Coupon{Id =2, Name="2000f", Percent=20, IsActive=false},
        };
    }
}
