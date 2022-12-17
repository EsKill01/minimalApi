using MagicalVilla_CoponAPI.Models.DTO.LocalUser;

namespace MagicalVilla_CoponAPI.Repository.AuthRepository
{
    public interface IAuthRepository
    {
        Task<bool> IsUniqueUserAsync(string username);

        Task<LocalUserLoginResponseDTO> AuthenticateAsync(LocalUserLoginResponseDTO localUserLoginResponseDTO);

        Task<LocalUserDTO> RegisterAsync(LocalUserDTO localUserDTO);
    }
}
