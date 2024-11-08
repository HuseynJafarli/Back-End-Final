using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YouPlay.Business.DTOs.TokenDTOs;
using YouPlay.Business.DTOs.UserDTOs;
using YouPlay.Business.Services.Interfaces;
using YouPlay.Core.Entities;

namespace YouPlay.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<TokenResponseDto> Login(UserLoginDto userLoginDto)
        {
            AppUser user = null;
            user = await _userManager.FindByNameAsync(userLoginDto.Username);
            if (user == null)
            {
                throw new NullReferenceException("Invalid Credentials");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe);
            var roles = await _userManager.GetRolesAsync(user);

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Fullname", user.FullName),
                .. roles.Select(role=>new Claim(ClaimTypes.Role, role))
            ];

            string secretKey = _configuration.GetSection("JWT:secretkey").Value;
            DateTime expires = DateTime.UtcNow.AddDays(1);

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new(
                signingCredentials: signingCredentials,
                claims: claims,
                audience: _configuration.GetSection("JWT:audience").Value,
                issuer: _configuration.GetSection("JWT:issuer").Value,
                expires: expires,
                notBefore: DateTime.UtcNow
                );

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return new TokenResponseDto(token, expires);
        }

        public async Task Register(UserRegisterDto userRegisterDto)
        {
            AppUser appUser = new AppUser()
            {
                Email = userRegisterDto.Email,
                FullName = userRegisterDto.Fullname,
                UserName = userRegisterDto.Username
            };

            await _userManager.CreateAsync(appUser, userRegisterDto.Password);
        }
    }
}
