namespace MagicalVilla_CoponAPI.Models.DTO
{
    public class CouponUpDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Percent { get; set; }

        public bool IsActive { get; set; }

        public DateTime? Update { get; set; }
    }
}
