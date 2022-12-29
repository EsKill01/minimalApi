using AutoMapper;
using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Models.DTO.Coupon;
using MagicalVilla_CoponAPI.Models.DTO.LocalUser;

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

            #region auth

            CreateMap<LocalUser, LocalUserDTO>().ReverseMap();
            CreateMap<LocalUserLoginDTO, LocalUserLoginResponseDTO>().ReverseMap();
            CreateMap<LocalUser, LocalUserRegistrationDTO>().ReverseMap();
            CreateMap<LocalUserDTO, LocalUserRegistrationDTO>().ReverseMap();

            #endregion


        }
    }
}
