﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YouPlay.Business.DTOs.TokenDTOs;
using YouPlay.Business.DTOs.UserDTOs;
using YouPlay.Business.Services.Interfaces;
using YouPlay.Business.Utilities;
using YouPlay.Core.Entities;

namespace YouPlay.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
        }
        public async Task<TokenResponseDto> Login(UserLoginDto userLoginDto)
        {
            AppUser user = await userManager.FindByNameAsync(userLoginDto.Username);
            if (user == null)
            {
                throw new NullReferenceException("Invalid Credentials");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid Credentials");
            }

            var roles = await userManager.GetRolesAsync(user);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Fullname", user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("ProfileImageUrl" , user.ProfileImageUrl),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            string secretKey = configuration.GetSection("JWT:secretkey").Value;
            DateTime expires = DateTime.UtcNow.AddDays(10);

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new(
                signingCredentials: signingCredentials,
                claims: claims,
                audience: configuration.GetSection("JWT:audience").Value,
                issuer: configuration.GetSection("JWT:issuer").Value,
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

            if (userRegisterDto.ProfileImage != null)
            {
                appUser.ProfileImageUrl = "https://localhost:7283/api/" + userRegisterDto.ProfileImage.SaveFile("wwwroot", "Uploads");
            }

            var result = await userManager.CreateAsync(appUser, userRegisterDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }

            if (!await roleManager.RoleExistsAsync("Member"))
            {
                await roleManager.CreateAsync(new IdentityRole("Member"));
            }

            await userManager.AddToRoleAsync(appUser, "Member");
        }
    }
}
