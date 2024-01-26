using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using SYS.Integra.src.SYS.Integra.Application.DTOs.Prestadores;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;

namespace SYS.Integra.src.SYS.Integra.API.Controllers
{
    [ApiController]
    [Route("api/itgPrestadores")]
    public class PrestadoresController : ControllerBase
    {
        private readonly IPrestadoresService _prestadoresService;

        public PrestadoresController(IPrestadoresService prestadoresService)
        {
            _prestadoresService = prestadoresService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestadorDTO>>> GetAllProviders()
        {
            var users = await _prestadoresService.GetAllProvidersAsync();
            return Ok(users);
        }

        [HttpGet("{providerCpfCnpj}")]
        public async Task<ActionResult<PrestadorDTO>> GetProviderByCpfCnpj(string providerCpfCnpj)
        {
            try
            {
                var provider = await _prestadoresService.GetProviderByCpfCnpjAsync(providerCpfCnpj);

                if (provider == null)
                {
                    return NotFound(new { code = "404", message = "Prestador não encontrado" });
                }

                return Ok(provider);
            }
            catch (OracleException ex)
            {
                // Captura exceções específicas do Oracle
                if (ex.Number == 20001)
                {
                    return NotFound(new { code = "404", message = "Usuário não encontrado na procedure", error = ex.Message });
                }

                Console.WriteLine($"Erro ao buscar prestador: {ex.Message}");
                return StatusCode(500, new { code = "500", message = "Erro interno do servidor", error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar prestador: {ex.Message}");
                return StatusCode(500, new { code = "500", message = "Erro interno do servidor" , error = ex.Message});
            }
        }
    }
}
