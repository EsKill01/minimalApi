using MagicalVilla_CoponAPI.models;

namespace MagicalVilla_CoponAPI.Repository.CouponRepository.CouponRepository
{
    public interface ICouponRepository
    {
        Task<ICollection<Coupon>> GetAllAsycn();

        Task<Coupon> GetAsycn(int id);

        Task<Coupon> GetAsycn(string name);

        Task CreateAsync(Coupon coupon);

        Task UpdateAsync(Coupon coupon);

        Task RemoveAsync(Coupon coupon);

        Task SaveAsync();
    }
}
