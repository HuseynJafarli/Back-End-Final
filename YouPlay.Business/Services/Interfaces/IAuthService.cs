using YouPlay.Business.DTOs.TokenDTOs;
using YouPlay.Business.DTOs.UserDTOs;

namespace YouPlay.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task Register(UserRegisterDto userRegisterDto);
        Task<TokenResponseDto> Login(UserLoginDto userLoginDto);

    }
}
