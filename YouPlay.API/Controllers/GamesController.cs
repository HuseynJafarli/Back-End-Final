using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YouPlay.API.ApiResponses;
using YouPlay.Business.DTOs.GameDTOs;
using YouPlay.Business.Exceptions.Common;
using YouPlay.Business.Services.Interfaces;

namespace YouPlay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService gameService ;

        public GamesController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ICollection<GameGetDto> data;
            try
            {
                data = await gameService.GetByExpessionAsync(true, null, "GameImages");
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ApiResponse<GameGetDto>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
            
            return Ok(new ApiResponse<ICollection<GameGetDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                ErrorMessage = null,
                Data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            GameGetDto dto = null;
            try
            {
                dto = await gameService.GetByIdAsync(id);
            }
            catch (InvalidIdException ex)
            {
                return BadRequest(new ApiResponse<GameGetDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Id is invalid!",
                    Data = null
                });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ApiResponse<GameGetDto>
                {
                    StatusCode = ex.StatusCode,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<GameGetDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
            return Ok(new ApiResponse<GameGetDto>
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK,
                ErrorMessage = null
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] GameCreateDto dto)
        {
            try
            {
                var imageFiles = Request.Form.Files; 
                                                     
                await gameService.CreateAsync(dto, imageFiles);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<GameCreateDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponse<GameCreateDto>
            {
                Data = null,
                StatusCode = StatusCodes.Status200OK,
                ErrorMessage = null
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] GameUpdateDto dto, int id)
        {
            GameGetDto res = null;
            try
            {
                var imageFiles = Request.Form.Files;

                res = await gameService.UpdateAsync(id,dto,imageFiles);
            }
            catch (InvalidIdException)
            {
                return BadRequest(new ApiResponse<GameUpdateDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Id yanlisdir",
                    Data = null
                });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ApiResponse<GameUpdateDto>
                {
                    StatusCode = ex.StatusCode,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<GameUpdateDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
            return Ok(new ApiResponse<GameGetDto>
            {
                Data = res,
                StatusCode = StatusCodes.Status200OK,
                ErrorMessage = null
            });

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await gameService.DeleteAsync(id);
            }
            catch (InvalidIdException)
            {
                return BadRequest(new ApiResponse<GameGetDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Id is invalid!",
                    Data = null
                });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ApiResponse<GameGetDto>
                {
                    StatusCode = ex.StatusCode,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<GameGetDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                StatusCode = StatusCodes.Status200OK,
                Data = null,
                ErrorMessage = null
            });
        }

    }
}
