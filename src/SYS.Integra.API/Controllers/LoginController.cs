using Microsoft.AspNetCore.Mvc;
using SYS.Integra.src.SYS.Integra.Application.Authentication;
using SYS.Integra.src.SYS.Integra.Application.DTOs.Login;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;

namespace SYS.Integra.src.SYS.Integra.API.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        private readonly TokenService _tokenService;

        public LoginController(IUsuariosService usuariosService, TokenService tokenService)
        {
            _usuariosService = usuariosService;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            // Validação básica dos dados de entrada
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Login) || string.IsNullOrEmpty(loginRequest.Senha))
            {
                return BadRequest("Favor informar os dados de login!");
            }

            // Chama o serviço de aplicação para realizar a lógica de login
            var loggedInUser = await _usuariosService.LoginAsync(loginRequest.Login, loginRequest.Senha);

            // Verifica se o login foi bem-sucedido
            if (loggedInUser == null)
            {
                return Unauthorized("Credenciais inválidas");
            }

            // Gera o token
            var token = await _tokenService.GenerateTokenAsync(loggedInUser);

            // Retorna o usuário logado (ou DTO) junto com outras informações, se necessário
            return Ok(new { Usuario = loggedInUser, Token = token });
        }
    }

}
