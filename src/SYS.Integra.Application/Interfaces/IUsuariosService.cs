using SYS.Integra.src.SYS.Integra.Application.DTOs.Usuarios;

namespace SYS.Integra.src.SYS.Integra.Application.Interfaces
{
    public interface IUsuariosService
    {
        Task<IEnumerable<UsuarioDTO>> GetAllUsersAsync();
        Task<UsuarioDTO> GetUserByIdAsync(int userId);
        Task<object> CreateUserAsync(UsuarioDTO userDTO);
        Task<UsuarioDTO> UpdateUserAsync(int userId, UsuarioDTO userDTO);
        //Task<bool> DeleteUserAsync(int userId);
        Task<UsuarioDTO> LoginAsync(string login, string senha);
        Task<object> RecoverUserPasswordAsync(string email);
        Task<UsuarioDTO> UpdateUserPasswordAsync(int userId, string novaSenha);
    }
}
