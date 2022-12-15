using AutoMapper;
using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models.DTO;

namespace MagicalVilla_CoponAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();
            CreateMap<Coupon, CouponUpdateDTO>().ReverseMap();
            CreateMap<Coupon, CouponUpDTO>().ReverseMap();
        }
    }
}
