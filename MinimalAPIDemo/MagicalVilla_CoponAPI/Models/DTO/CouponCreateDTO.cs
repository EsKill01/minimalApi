namespace MagicalVilla_CoponAPI.Models.DTO
{
    public class CouponCreateDTO
    {
        public string Name { get; set; }

        public int  Percent { get; set; }

        public bool IsActive { get; set; }
    }
}
