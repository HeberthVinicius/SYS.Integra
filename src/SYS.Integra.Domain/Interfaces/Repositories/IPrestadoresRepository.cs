using SYS.Integra.src.SYS.Integra.Domain.Entities;

namespace SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories
{
    public interface IPrestadoresRepository
    {
        Task<IEnumerable<ItgPrestador>> GetAllProvidersAsync();
        Task<ItgPrestador> GetProviderByCpfCnpjAsync(string providerCpfCnpj);
    }
}