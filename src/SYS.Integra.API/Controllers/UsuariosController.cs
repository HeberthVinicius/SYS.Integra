using EmailService.src.EmailService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using SYS.Integra.src.SYS.Integra.Application.DTOs.Usuarios;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;

namespace SYS.Integra.src.SYS.Integra.API.Controllers
{
    [ApiController]
    [Route("api/itgUsuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetAllUsers()
        {
            var users = await _usuariosService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UsuarioDTO>> GetUserById(int userId)
        {
            var user = await _usuariosService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(); // Retorna 404 se o usuário não for encontrado
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<object>> CreateUser([FromBody] UsuarioDTO userDTO)
        {
            try
            {
                var result = await _usuariosService.CreateUserAsync(userDTO);

                if ((bool)result.GetType().GetProperty("Success").GetValue(result))
                {
                    var createdUser = result.GetType().GetProperty("User").GetValue(result);
                    return CreatedAtAction(nameof(GetUserById), new { userId = ((UsuarioDTO)createdUser).Id }, createdUser);
                }
                else
                {
                    var errorMessage = result.GetType().GetProperty("ErrorMessage").GetValue(result);
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult<UsuarioDTO>> UpdateUser(int userId, [FromBody] UsuarioDTO userDTO)
        {
            var updatedUser = await _usuariosService.UpdateUserAsync(userId, userDTO);

            if (updatedUser == null)
            {
                return NotFound(); // Retorna 404 se o usuário não for encontrado
            }

            return Ok(updatedUser);
        }

        [HttpPut("{userId}/update-password")]
        public async Task<ActionResult<UsuarioDTO>> UpdateUserPassword(int userId, [FromBody] UpdateUserPasswordDTO passwordDTO)
        {
            var updatedUser = await _usuariosService.UpdateUserPasswordAsync(userId, passwordDTO.NewHashPassword);

            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpPost("recover-password")]
        public async Task<ActionResult<object>> RecoverUserPassword([FromBody] RecoverUserPasswordDTO recoverUserPasswordDTO)
        {
            try
            {
                var result = await _usuariosService.RecoverUserPasswordAsync(recoverUserPasswordDTO.Email);

                if ((bool)result.GetType().GetProperty("Success").GetValue(result))
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, ErrorMessage = $"Internal server error: {ex.Message}" });
            }
        }

        //[HttpDelete("{userId}")]
        //public async Task<ActionResult> DeleteUser(int userId)
        //{
        //    var result = await _itgUsuarioService.DeleteUserAsync(userId);
        //    if (!result)
        //    {
        //        return NotFound(); // Retorna 404 se o usuário não for encontrado
        //    }

        //    return NoContent(); // Retorna 204 No Content em caso de sucesso
        //}
    }
}
