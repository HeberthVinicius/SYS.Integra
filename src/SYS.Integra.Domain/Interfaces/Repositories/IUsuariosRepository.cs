using SYS.Integra.src.SYS.Integra.Domain.Entities;

namespace SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories
{
    public interface IUsuariosRepository
    {
        Task<IEnumerable<ItgUsuario>> GetAllUsersAsync();
        Task<ItgUsuario> GetUserByIdAsync(int userId);
        Task<ItgUsuario> AddUserAsync(ItgUsuario user);
        Task<ItgUsuario> UpdateUserAsync(ItgUsuario user);
        //Task<bool> DeleteUserAsync(int userId);
        Task<ItgUsuario> GetUserByLoginAsync(string login);
        Task<ItgUsuario> GetUserByEmailAsync(string email);
    }
}
