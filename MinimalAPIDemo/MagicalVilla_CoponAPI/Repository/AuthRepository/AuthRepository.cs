using AutoMapper;
using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Models.DTO.LocalUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicalVilla_CoponAPI.Repository.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string _secretKey;
        public AuthRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;

            _secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
        }
        public async Task<LocalUserLoginResponseDTO> AuthenticateAsync(LocalUserLoginDTO localUserLoginDTO)
        {
            var userAutincate = await _db.LocalUsers.SingleOrDefaultAsync(c => 
            c.UserName.ToLower() == localUserLoginDTO.UserName.ToLower() 
            && c.Password == localUserLoginDTO.Password);

            if(userAutincate != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userAutincate.Name),
                        new Claim(ClaimTypes.Role, userAutincate.Role)

                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescription);
                var writenToken = new JwtSecurityTokenHandler().WriteToken(token);

                LocalUserLoginResponseDTO reponse = new LocalUserLoginResponseDTO
                {
                    Token = writenToken,
                    User = _mapper.Map<LocalUserDTO>(userAutincate)
                };

                return await Task.FromResult(reponse);
            }

            return null;
        }

        public async Task<bool> IsUniqueUserAsync(string username)
        {
            var user = await _db.LocalUsers.FirstOrDefaultAsync(c => c.UserName == username);

            if (user == null)
            {
                return await Task.FromResult(false);
            }
            else
            {
                return await Task.FromResult(true);
            }
        }

        public async Task<LocalUserDTO> RegisterAsync(LocalUserRegistrationDTO localUserDTO)
        {

            var localUser = _mapper.Map<LocalUser>(localUserDTO);
            //localUser.Role = "Admin";
            localUser.Role = "Customer";

            await _db.LocalUsers.AddAsync(localUser);
            await _db.SaveChangesAsync();

            var user = await _db.LocalUsers.FirstOrDefaultAsync(c => c.UserName == localUserDTO.UserName);

            return _mapper.Map<LocalUserDTO>(user);
        }
    }
}
