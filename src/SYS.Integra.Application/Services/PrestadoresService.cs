using SYS.Integra.src.SYS.Integra.Application.DTOs.Prestadores;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;
using SYS.Integra.src.SYS.Integra.Domain.Entities;
using SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories;

namespace SYS.Integra.src.SYS.Integra.Application.Services
{
    public class PrestadoresService : IPrestadoresService
    {
        private readonly IPrestadoresRepository _prestadoresRepository;

        public PrestadoresService(IPrestadoresRepository prestadoresRepository)
        {
            _prestadoresRepository = prestadoresRepository;
        }

        public async Task<IEnumerable<PrestadorDTO>> GetAllProvidersAsync()
        {
            var providers = await _prestadoresRepository.GetAllProvidersAsync();

            return providers.Select(MapToDTO);
        }

        public async Task<PrestadorDTO> GetProviderByCpfCnpjAsync(string providerCpfCnpj)
        {
                var provider = await _prestadoresRepository.GetProviderByCpfCnpjAsync(providerCpfCnpj) ?? throw new Exception("Prestador não encontrado");

                return MapToDTO(provider);
        }

        private PrestadorDTO MapToDTO(ItgPrestador prestador)
        {
            if (prestador == null)
            {
                throw new InvalidOperationException("Entidade nula");
            }

            return new PrestadorDTO
            {
                Id = prestador.Id,
                IdPrestadorERP = prestador.IdPrestadorERP,
                RazaoSocial = prestador.RazaoSocial,
                Nome = prestador.Nome,
                CpfCnpj = prestador.CpfCnpj,
                Ativo = prestador.Ativo,
                DataCriacao = prestador.DataCriacao,
                DataVerificacao = prestador.DataVerificacao,
                //Mapear outras propriedades, se necessário.
            };
        }
    }
}