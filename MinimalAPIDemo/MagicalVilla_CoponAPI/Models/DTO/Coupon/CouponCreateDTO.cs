namespace MagicalVilla_CoponAPI.Models.DTO.Coupon
{
    public class CouponCreateDTO
    {
        public string Name { get; set; }

        public int Percent { get; set; }

        public bool IsActive { get; set; }
    }
}
