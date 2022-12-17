using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.models;
using Microsoft.EntityFrameworkCore;

namespace MagicalVilla_CoponAPI.Repository.CouponRepository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        public CouponRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task CreateAsync(Coupon coupon)
        {
            await _db.AddAsync(coupon);
        }

        public async Task<ICollection<Coupon>> GetAllAsycn()
        {
            return await _db.Coupons.ToListAsync();
        }

        public async Task<Coupon> GetAsycn(int id)
        {
            return await _db.Coupons.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Coupon> GetAsycn(string name)
        {
            return await _db.Coupons.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task RemoveAsync(Coupon coupon)
        {
            _db.Remove(coupon);

            await Task.FromResult(coupon);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Coupon coupon)
        {
            _db.Update(coupon);

            await Task.FromResult(coupon);
        }
    }
}
