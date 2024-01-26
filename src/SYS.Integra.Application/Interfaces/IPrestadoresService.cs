using SYS.Integra.src.SYS.Integra.Application.DTOs.Prestadores;

namespace SYS.Integra.src.SYS.Integra.Application.Interfaces
{
    public interface IPrestadoresService
    {
        Task<IEnumerable<PrestadorDTO>> GetAllProvidersAsync();
        Task<PrestadorDTO> GetProviderByCpfCnpjAsync(string providerCpfCnpj);
    }
}