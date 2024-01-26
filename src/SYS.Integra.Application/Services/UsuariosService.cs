using SYS.Integra.src.SYS.Integra.Application.DTOs.Usuarios;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;
using SYS.Integra.src.SYS.Integra.Domain.Entities;
using SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories;
using EmailService.src.EmailService.Domain.Interfaces;
using EmailService.src.EmailService.Domain.Entities;
using System.Text;
using SYS.Integra.src.SYS.Integra.Application.Exceptions;

namespace SYS.Integra.src.SYS.Integra.Application.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IEmailSenderService _emailSenderService;

        public UsuariosService(IUsuariosRepository usuariosRepository, IEmailSenderService emailSenderService)
            {
            _usuariosRepository = usuariosRepository;
            _emailSenderService = emailSenderService;
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllUsersAsync()
        {
            var users = await _usuariosRepository.GetAllUsersAsync();

            return users.Select(MapToDTO);
        }

        public async Task<UsuarioDTO> GetUserByIdAsync(int userId)
        {
            var user = await _usuariosRepository.GetUserByIdAsync(userId);

            return MapToDTO(user);
        }

        public async Task<object> CreateUserAsync(UsuarioDTO userDTO)
        {
            try
            {
                userDTO.Senha = GenerateRandomPassword(8);
                userDTO.SenhaHash = CreateHashPassword(userDTO.Senha);
                userDTO.ResetarSenha = 1; // Marcar como recuper senha (ou alguma lógica adequada)

                var user = MapToEntity(userDTO);
                var emailSubject = "Bem-vindo!";
                var emailStatement = $"Cadastro efetuado! Seu usuario é: <strong>{userDTO.Login}</strong> <br /> E-mail cadastrado: {userDTO.Email}";
                var welcomeEmail = new Email
                {
                    ToEmail = user.Email,
                    ToName = user.Nome,
                    Subject = emailSubject,
                    Body = userDTO.Senha,
                    Statement = emailStatement,
                };

                var emailResult = await _emailSenderService.SendEmailAsync(welcomeEmail);

                if (emailResult)
                {
                    user = await _usuariosRepository.AddUserAsync(user);
                    userDTO.Senha = string.Empty;

                    return new { Success = true, User = MapToDTO(user) };
                }
                else
                {
                    return new { Success = false, ErrorMessage = "Não foi possível criar o usuário!" };
                }
            }
            catch (Exception ex)
            {
                return new { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<UsuarioDTO> UpdateUserAsync(int userId, UsuarioDTO userDTO)
        {
            var existingUser = await _usuariosRepository.GetUserByIdAsync(userId) ?? throw new Exception("Usuário não encontrado");

            existingUser.Nome = userDTO.Nome;
            existingUser.Email = userDTO.Email;
            existingUser.Ativo = userDTO.Ativo;
            existingUser.DataAtualizacao = userDTO.DataAtualizacao = DateTime.Now;
            existingUser.GerenciarUsuario = userDTO.GerenciarUsuario;
            existingUser.UsuarioGetec = userDTO.UsuarioGetec;
            existingUser.ResetarSenha = userDTO.ResetarSenha;
            // Atualizar outras propriedades...

            existingUser = await _usuariosRepository.UpdateUserAsync(existingUser);
            return MapToDTO(existingUser);
        }

        public async Task<UsuarioDTO> UpdateUserPasswordAsync(int userId, string newPassword)
        {
            var existingUser = await _usuariosRepository.GetUserByIdAsync(userId) ?? throw new NotFoundException($"Usuário com ID {userId} não encontrado!");

            existingUser.SenhaHash = CreateHashPassword(newPassword);
            existingUser.DataAtualizacao = DateTime.Now;
            existingUser.ResetarSenha = 0;
            existingUser.PrimeiroAcesso = 0;
            existingUser.Ativo = 1;

            existingUser = await _usuariosRepository.UpdateUserAsync(existingUser);
            return MapToDTO(existingUser);
        }

        public async Task<object> RecoverUserPasswordAsync(string email)
        {
            try
            {
                var user = await _usuariosRepository.GetUserByEmailAsync(email) ?? throw new NotFoundException($"Email {email} não cadastrado!"); 
                var novaSenha = GenerateRandomPassword(8);

                user.SenhaHash = CreateHashPassword(novaSenha);
                user.ResetarSenha = 1; // Marcar como recuper senha (ou alguma lógica adequada)

                var emailSubject = "Recuperação de Senha";
                var emailStatement = "Conforme sua solicitação, segue abaixo a nova senha de acesso para sua conta:";
                var recoverEmail = new Email
                {
                    ToEmail = user.Email,
                    ToName = user.Nome,
                    Subject = emailSubject,
                    Body = novaSenha,
                    Statement = emailStatement,
                };

                var emailResult = await _emailSenderService.SendEmailAsync(recoverEmail);

                if (emailResult)
                {
                    user = await _usuariosRepository.UpdateUserAsync(user);
                    return new { Success = true, Message = "E-mail de recuperação de senha enviado com sucesso." };
                }
                else
                {
                    return new { Success = false, ErrorMessage = "Não foi possível enviar o e-mail de recuperação de senha." };
                }
            }
            catch (Exception ex)
            {
                return new { Success = false, ErrorMessage = ex.Message };
            }
        }

        //public async Task<bool> DeleteUserAsync(int userId)
        //{
        //    return await _itgUsuarioRepository.DeleteUserAsync(userId);
        //}

        public async Task<UsuarioDTO> LoginAsync(string login, string password)
        {
            try
            {
                var user = await _usuariosRepository.GetUserByLoginAsync(login);

                if (user != null && ValidateHashPassword(user, password))
                {
                    return MapToDTO(user);
                }

                throw new NotFoundException($"Usuário com Login {login} não encontrado!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro durante o login: {ex.Message}");
                throw;
            }
        }

        private static string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        private static bool ValidateHashPassword(ItgUsuario usuario, string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);
        }

        private static string CreateHashPassword(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        private UsuarioDTO MapToDTO(ItgUsuario usuario)
        {
            if (usuario == null)
            {
                throw new InvalidOperationException("Entidade nula");
            }

            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                IdPrestador = usuario.IdPrestador,
                IdUsuarioCadastro = usuario.IdUsuarioCadastro,
                Login = usuario.Login,
                SenhaHash = usuario.SenhaHash,
                Email = usuario.Email,
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                ResetarSenha = usuario.ResetarSenha,
                Ativo = usuario.Ativo,
                GerenciarUsuario = usuario.GerenciarUsuario,
                UsuarioGetec = usuario.UsuarioGetec,
                DataCriacao = usuario.DataCriacao,
                DataAtualizacao = usuario.DataAtualizacao,
                //Mapear outras propriedades...
            };
        }

        private static ItgUsuario MapToEntity(UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
            {
                throw new InvalidOperationException("DTO nula");
            }

            return new ItgUsuario
            {
                Id = usuarioDTO.Id,
                Nome = usuarioDTO.Nome,
                IdPrestador = usuarioDTO.IdPrestador,
                IdUsuarioCadastro = usuarioDTO.IdUsuarioCadastro,
                Login = usuarioDTO.Login,
                SenhaHash = usuarioDTO.SenhaHash,
                Email = usuarioDTO.Email,
                PrimeiroAcesso = usuarioDTO.PrimeiroAcesso,
                ResetarSenha = usuarioDTO.ResetarSenha,
                Ativo = usuarioDTO.Ativo,
                GerenciarUsuario = usuarioDTO.GerenciarUsuario,
                UsuarioGetec = usuarioDTO.UsuarioGetec,
                DataCriacao = usuarioDTO.DataCriacao,
                DataAtualizacao = usuarioDTO.DataAtualizacao,
                // Mapear outras propriedades...
            };
        }

    }
}
