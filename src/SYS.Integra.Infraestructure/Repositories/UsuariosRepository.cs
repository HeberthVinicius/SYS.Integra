using Microsoft.EntityFrameworkCore;
using SYS.Integra.src.SYS.Integra.Application.Exceptions;
using SYS.Integra.src.SYS.Integra.Domain.Entities;
using SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories;

namespace SYS.Integra.src.SYS.Integra.Infraestructure.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly ModelContext _modelContext;

        public UsuariosRepository(ModelContext modelContext)
        {
            _modelContext = modelContext;
        }

        public async Task<IEnumerable<ItgUsuario>> GetAllUsersAsync()
        {
            try
            {
                return await _modelContext.ItgUsuarios.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar todos os usuários: {ex.Message}");
                throw;
            }
        }

        public async Task<ItgUsuario> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _modelContext.ItgUsuarios.FindAsync(userId);

                return user ?? throw new NotFoundException($"Usuário com ID {userId} não encontrado!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao consultar usuário por ID: {ex.Message}");
                throw;
            }
        }

        public async Task<ItgUsuario> AddUserAsync(ItgUsuario user)
        {
            try
            {
                _modelContext.ItgUsuarios.Add(user);
                await _modelContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar usuário: {ex.Message}");
                throw;
            }
        }

        public async Task<ItgUsuario> UpdateUserAsync(ItgUsuario user)
        {
            try
            {
                _modelContext.Entry(user).State = EntityState.Modified;
                await _modelContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o usuário: {ex.Message}");
                throw;
            }
        }

        //public async Task<bool> DeleteUserAsync(int userId)
        //{
        //    var user = await _modelContext.ItgUsuarios.FindAsync(userId);
        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    _modelContext.ItgUsuarios.Remove(user);
        //    await _modelContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<ItgUsuario> GetUserByLoginAsync(string login)
        {
            try
            {
                var user = await _modelContext.ItgUsuarios.FirstOrDefaultAsync(u => u.Login == login);

                return user ?? throw new NotFoundException($"User with login {login} not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by login: {ex.Message}");
                throw;
            }
        }

        public async Task<ItgUsuario> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _modelContext.ItgUsuarios.FirstOrDefaultAsync(u => u.Email == email);

                return user ?? throw new NotFoundException($"User with email {email} not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user by email: {ex.Message}");
                throw;
            }
        }
    }

}
