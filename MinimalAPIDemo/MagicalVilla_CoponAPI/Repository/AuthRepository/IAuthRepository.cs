using MagicalVilla_CoponAPI.Models.DTO.LocalUser;

namespace MagicalVilla_CoponAPI.Repository.AuthRepository
{
    public interface IAuthRepository
    {
        Task<bool> IsUniqueUserAsync(string username);

        Task<LocalUserLoginResponseDTO> AuthenticateAsync(LocalUserLoginDTO localUserLoginResponseDTO);

        Task<LocalUserDTO> RegisterAsync(LocalUserRegistrationDTO localUserDTO);
    }
}
