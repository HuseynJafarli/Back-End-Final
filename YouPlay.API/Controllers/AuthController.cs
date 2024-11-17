using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouPlay.API.ApiResponses;
using YouPlay.Business.DTOs.TokenDTOs;
using YouPlay.Business.DTOs.UserDTOs;
using YouPlay.Business.Services.Implementations;
using YouPlay.Business.Services.Interfaces;
using YouPlay.Business.Utilities;
using YouPlay.Core.Entities;

namespace YouPlay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.authService = authService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto userRegisterDto)
        {
            try
            {
               await authService.Register(userRegisterDto);

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Registration failed", error = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            try
            {
                var data = await authService.Login(dto);

                return Ok(new ApiResponse<TokenResponseDto>
                {
                    Data = data,
                    ErrorMessage = "Logged in successfully!",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Login failed", error = ex.Message });
            }
            catch (NullReferenceException ex)
            {
                return BadRequest(new { message = "Login failed", error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An unexpected error occurred", error = ex.Message });
            }
        }

    }
}
