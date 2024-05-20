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
        public async Task<IActionResult> CalcularBoleto([FromBody] BoletoRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { error = "A solicitação é inválida." });
            }

            if (string.IsNullOrEmpty(request.BarCode) || string.IsNullOrEmpty(request.PaymentDate))
            {
                return BadRequest(new { error = "Código de barras e data de pagamento são obrigatórios." });
            }

            try
            {
                string token = Request.Headers.Authorization.ToString();

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { error = "Token de autorização é obrigatório." });
                }

                if (!DateTime.TryParse(request.PaymentDate, out DateTime paymentDate))
                {
                    return BadRequest(new { error = "Data de pagamento inválida." });
                }

                var result = await _boletoService.CalcularValorBoletoAsync(request.BarCode, paymentDate, token);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." });
            }
        }
    }
}
