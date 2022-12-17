using AutoMapper;
using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.Models.DTO.LocalUser;

namespace MagicalVilla_CoponAPI.Repository.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public AuthRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public Task<LocalUserLoginResponseDTO> Authenticate(LocalUserLoginResponseDTO localUserLoginResponseDTO)
        {
            throw new NotImplementedException();
        }

        public bool IsUniqueUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<LocalUserDTO> Register(LocalUserDTO localUserDTO)
        {
            throw new NotImplementedException();
        }
    }
}
