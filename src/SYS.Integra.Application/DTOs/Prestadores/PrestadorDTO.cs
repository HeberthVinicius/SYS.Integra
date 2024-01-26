using SYS.Integra.src.SYS.Integra.Application.DTOs.Prestadores;
using SYS.Integra.src.SYS.Integra.Domain.Entities;

namespace SYS.Integra.src.SYS.Integra.Application.DTOs.Prestadores
{
    public class PrestadorDTO
    {
        public int Id { get; set; }
        public int IdPrestadorERP { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string CpfCnpj { get; set; } = string.Empty;
        public int Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataVerificacao { get; set; }

        // Método estático para mapear de uma entidade para DTO
        public static PrestadorDTO FromEntity(ItgPrestador itgPrestador)
        {
            return new PrestadorDTO
            {
                Id = itgPrestador.Id,
                IdPrestadorERP = itgPrestador.IdPrestadorERP,
                RazaoSocial = itgPrestador.RazaoSocial,
                Nome = itgPrestador.Nome,
                CpfCnpj = itgPrestador.CpfCnpj,
                Ativo = itgPrestador.Ativo,
                DataCriacao = itgPrestador.DataCriacao,
                DataVerificacao = itgPrestador.DataVerificacao,
                //Mapear outras propriedades, se necessário.
            };
        }
    }
}