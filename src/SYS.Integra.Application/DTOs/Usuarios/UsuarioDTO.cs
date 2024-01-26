using SYS.Integra.src.SYS.Integra.Domain.Entities;
using System.Text.Json.Serialization;

namespace SYS.Integra.src.SYS.Integra.Application.DTOs.Usuarios
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public int IdPrestador { get; set; }
        public int IdUsuarioCadastro { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PrimeiroAcesso { get; set; }
        public int ResetarSenha { get; set; } 
        public int Ativo { get; set; }
        public int GerenciarUsuario { get; set; }
        public int UsuarioGetec { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        [JsonIgnore]
        public string Senha { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;

        // Método estático para mapear de uma entidade para DTO
        public static UsuarioDTO FromEntity(ItgUsuario itgUsuario)
        {
            return new UsuarioDTO
            {
                Id = itgUsuario.Id,
                IdPrestador = itgUsuario.IdPrestador,
                IdUsuarioCadastro = itgUsuario.IdUsuarioCadastro,
                Nome = itgUsuario.Nome,
                Login = itgUsuario.Login,
                Email = itgUsuario.Email,
                PrimeiroAcesso = itgUsuario.PrimeiroAcesso,
                ResetarSenha = itgUsuario.ResetarSenha,
                Ativo = itgUsuario.Ativo,
                GerenciarUsuario = itgUsuario.GerenciarUsuario,
                UsuarioGetec = itgUsuario.UsuarioGetec,
                DataCriacao = itgUsuario.DataCriacao,
                DataAtualizacao = itgUsuario.DataAtualizacao,
                //Mapear outras propriedades, se necessário.
            };
        }
    }

}

