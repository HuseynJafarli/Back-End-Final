using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YouPlay.API.ApiResponses;
using YouPlay.Business.DTOs.PurchaseDTOs;
using YouPlay.Business.Exceptions.Common;
using YouPlay.Business.Services.Interfaces;

namespace YouPlay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        // Endpoint to create a new purchase
        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody] PurchaseCreateDto purchaseCreateDto)
        {
            if (purchaseCreateDto == null)
            {
                return BadRequest(new ApiResponse<PurchaseCreateDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid purchase data.",
                    Data = null
                });
            }

            try
            {
                var purchaseDto = await _purchaseService.CreateAsync(purchaseCreateDto);
                return CreatedAtAction(nameof(GetPurchaseById), new { id = purchaseDto.Id }, new ApiResponse<PurchaseCreateDto>
                {
                    StatusCode = StatusCodes.Status201Created,
                    ErrorMessage = null,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PurchaseCreateDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        // Endpoint to retrieve a purchase by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseById(int id)
        {
            if (id < 1)
            {
                return BadRequest(new ApiResponse<PurchaseGetDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid purchase ID.",
                    Data = null
                });
            }

            try
            {
                var purchaseDto = await _purchaseService.GetByIdAsync(id);
                if (purchaseDto == null)
                {
                    return NotFound(new ApiResponse<PurchaseGetDto>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "Purchase not found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<PurchaseGetDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    ErrorMessage = null,
                    Data = purchaseDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PurchaseGetDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        // Endpoint to get all purchases
        [HttpGet]
        public async Task<IActionResult> GetAllPurchases()
        {
            try
            {
                var purchaseDtos = await _purchaseService.GetByExpressionAsync();
                if (purchaseDtos == null || !purchaseDtos.Any())
                {
                    return NotFound(new ApiResponse<ICollection<PurchaseGetDto>>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "No purchases found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<ICollection<PurchaseGetDto>>
                {
                    StatusCode = StatusCodes.Status200OK,
                    ErrorMessage = null,
                    Data = purchaseDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<ICollection<PurchaseGetDto>>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        // Endpoint to delete a purchase
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            if (id < 1)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid purchase ID.",
                    Data = null
                });
            }

            try
            {
                await _purchaseService.DeleteAsync(id);
                return Ok(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status200OK,
                    ErrorMessage = null,
                    Data = null
                });
            }
            catch (InvalidIdException)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid ID.",
                    Data = null
                });
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
    }
}
