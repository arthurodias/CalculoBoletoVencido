using Boleto.Api.Models;
using Boleto.Domain.Intefaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoletoController : ControllerBase
    {
        private readonly IBoletoService _boletoService;

        public BoletoController(IBoletoService boletoService)
        {
            _boletoService = boletoService;
        }

        [HttpPost]
        public async Task<IActionResult> CalculateBoleto([FromBody] BoletoRequest request)
        {
            try
            {
                var result = await _boletoService.CalcularValorBoletoAsync(request.BarCode, DateTime.Parse(request.PaymentDate));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}