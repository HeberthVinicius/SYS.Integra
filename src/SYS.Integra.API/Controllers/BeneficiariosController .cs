using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SYS.Integra.src.SYS.Integra.Application.Authentication;
using SYS.Integra.src.SYS.Integra.Application.Helpers;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;
using System.Security.Claims;

namespace SYS.Integra.src.SYS.Integra.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeneficiarioController : ControllerBase
    {
        private readonly IBeneficiariosService _beneficiarioService;
        private readonly TokenService _tokenService; 

        public BeneficiarioController(IBeneficiariosService beneficiarioService, TokenService tokenService)
        {
            _beneficiarioService = beneficiarioService;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet("getSegurados")]
        public IActionResult GetSegurados(string dtFinal)
        {
            if (!TokenHelper.TryGetValidatedToken(HttpContext, _tokenService, out var principal))
            {
                return Unauthorized();
            }

            var idPrestadorClaim = principal?.FindFirst(ClaimTypes.Sid);

            if (idPrestadorClaim == null || !int.TryParse(idPrestadorClaim.Value, out int idPrestador))
            {
                return Unauthorized("Reivindicação de idPrestador ausente ou inválida no token.");
            }

            if (!DateTime.TryParseExact(dtFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtFinal))
            {
                return BadRequest("Formato de data inválido. Use o formato dd/MM/yyyy.");
            }

            var beneficiarios = _beneficiarioService.GetSegurados(idPrestador, parsedDtFinal);
            return Ok(beneficiarios);
        }

        [Authorize]
        [HttpGet("getSeguradosComMenores")]
        public IActionResult GetSeguradosComMenores(string dtFinal)
        {
            if (!TokenHelper.TryGetValidatedToken(HttpContext, _tokenService, out _))
            {
                return Unauthorized();
            }

            if (!DateTime.TryParseExact(dtFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtFinal))
            {
                return BadRequest("Formato de data inválido. Use o formato dd/MM/yyyy.");
            }

            var beneficiarios = _beneficiarioService.GetSeguradosComMenores(parsedDtFinal);
            return Ok(beneficiarios);
        }

        [Authorize]
        [HttpGet("getMenores")]
        public IActionResult GetMenores(string dtInicial, string dtFinal)
        {
            if (!TokenHelper.TryGetValidatedToken(HttpContext, _tokenService, out _))
            {
                return Unauthorized();
            }

            if (!DateTime.TryParseExact(dtInicial, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtInicial))
            {
                return BadRequest("Formato de data inválido para dtInicial. Use o formato dd/MM/yyyy.");
            }

            if (!DateTime.TryParseExact(dtFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtFinal))
            {
                return BadRequest("Formato de data inválido para dtFinal. Use o formato dd/MM/yyyy.");
            }

            var beneficiarios = _beneficiarioService.GetMenores(parsedDtInicial, parsedDtFinal);
            return Ok(beneficiarios);
        }

        [Authorize]
        [HttpGet("getNovos")]
        public IActionResult GetNovos(string dtInicial, string dtFinal)
        {
            if (!TokenHelper.TryGetValidatedToken(HttpContext, _tokenService, out _))
            {
                return Unauthorized();
            }

            if (!DateTime.TryParseExact(dtInicial, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtInicial))
            {
                return BadRequest("Formato de data inválido para dtInicial. Use o formato dd/MM/yyyy.");
            }

            if (!DateTime.TryParseExact(dtFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtFinal))
            {
                return BadRequest("Formato de data inválido para dtFinal. Use o formato dd/MM/yyyy.");
            }

            var beneficiarios = _beneficiarioService.GetNovos(parsedDtInicial, parsedDtFinal);
            return Ok(beneficiarios);
        }

        [Authorize]
        [HttpGet("getBloqueados")]
        public IActionResult GetBloqueados(string dtInicial, string dtFinal)
        {
            if (!TokenHelper.TryGetValidatedToken(HttpContext, _tokenService, out _))
            {
                return Unauthorized();
            }

            if (!DateTime.TryParseExact(dtInicial, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtInicial))
            {
                return BadRequest("Formato de data inválido para dtInicial. Use o formato dd/MM/yyyy.");
            }

            if (!DateTime.TryParseExact(dtFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtFinal))
            {
                return BadRequest("Formato de data inválido para dtFinal. Use o formato dd/MM/yyyy.");
            }

            var beneficiarios = _beneficiarioService.GetBloqueados(parsedDtInicial, parsedDtFinal);
            return Ok(beneficiarios);
        }

        [Authorize]
        [HttpGet("getCancelados")]
        public IActionResult GetCancelados(string dtInicial, string dtFinal)
        {
            if (!TokenHelper.TryGetValidatedToken(HttpContext, _tokenService, out _))
            {
                return Unauthorized();
            }

            if (!DateTime.TryParseExact(dtInicial, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtInicial))
            {
                return BadRequest("Formato de data inválido para dtInicial. Use o formato dd/MM/yyyy.");
            }

            if (!DateTime.TryParseExact(dtFinal, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDtFinal))
            {
                return BadRequest("Formato de data inválido para dtFinal. Use o formato dd/MM/yyyy.");
            }

            var beneficiarios = _beneficiarioService.GetCancelados(parsedDtInicial, parsedDtFinal);

            return Ok(beneficiarios);
        }
    }
}
