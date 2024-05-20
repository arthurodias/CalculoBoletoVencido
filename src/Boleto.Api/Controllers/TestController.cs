using Microsoft.AspNetCore.Mvc;

namespace Boleto.Api.Controllers
{
    /// <summary>
    /// Controlador para testar o token de autenticação do middleware.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Obtém o token de autenticação do cabeçalho da requisição.
        /// </summary>
        /// <returns>
        /// Retorna um objeto contendo o token de autenticação do TokenMiddleware.
        /// </returns>
        /// <response code="200">Se o token foi recuperado com sucesso.</response>
        [HttpGet]
        public IActionResult GetToken()
        {
            var token = Request.Headers["Authorization"].ToString();
            return Ok(new { Token = token });
        }
    }
}