using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YouPlay.API.ApiResponses;
using YouPlay.Business.DTOs.GameDTOs;
using YouPlay.Business.DTOs.PurchaseDTOs;
using YouPlay.Business.Exceptions.Common;
using YouPlay.Business.Services.Implementations;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseCreateDto dto)
        {
            try
            {
                await _purchaseService.CreateAsync(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<PurchaseCreateDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponse<PurchaseCreateDto>
            {
                Data = null,
                StatusCode = StatusCodes.Status200OK,
                ErrorMessage = null
            });
        }


    }
}
