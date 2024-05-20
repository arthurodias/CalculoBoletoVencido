using Boleto.Api.Models;
using Boleto.Domain.Intefaces.Services;
using Boleto.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.Api.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações relacionadas aos boletos.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BoletoController : ControllerBase
    {
        private readonly IBoletoService _boletoService;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="BoletoController"/>.
        /// </summary>
        /// <param name="boletoService">Serviço de boletos.</param>
        public BoletoController(IBoletoService boletoService)
        {
            _boletoService = boletoService;
        }

        /// <summary>
        /// Calcula o valor total de um boleto vencido.
        /// </summary>
        /// <param name="request">Objeto contendo o código de barras do boleto e a data de pagamento.</param>
        /// <returns>
        /// Retorna um objeto <see cref="BoletoResponse"/> contendo o valor original do boleto, valor calculado com juros e multa, data de vencimento, data de pagamento, juros calculados e multa calculada.
        /// Se ocorrer algum erro, retorna um objeto contendo a mensagem de erro.
        /// </returns>
        /// <response code="200">Retorna os dados do boleto calculado.</response>
        /// <response code="400">Se ocorrer algum erro durante o processamento do boleto.</response>
        [HttpPost]
        public async Task<IActionResult> CalculateBoleto([FromBody] BoletoRequest request)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString();
                var result = await _boletoService.CalcularValorBoletoAsync(request.BarCode, DateTime.Parse(request.PaymentDate), token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}