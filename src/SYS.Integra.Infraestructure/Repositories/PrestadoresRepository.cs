using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using SYS.Integra.src.SYS.Integra.Domain.Entities;
using SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories;

namespace SYS.Integra.src.SYS.Integra.Infraestructure.Repositories
{
    public class PrestadoresRepository : IPrestadoresRepository
    {
        private readonly ModelContext _modelContext;

        public PrestadoresRepository(ModelContext modelContext)
        {
            _modelContext = modelContext;
        }

        public async Task<IEnumerable<ItgPrestador>> GetAllProvidersAsync()
        {
            return await _modelContext.ItgPrestadores.ToListAsync();
        }

        public async Task<ItgPrestador> GetProviderByCpfCnpjAsync(string providerCpfCnpj)
        {
            try
            {
                var PKG = $"BEGIN PKG_ITG_PRESTADOR.SP_CADASTRA_PRESTADOR('{providerCpfCnpj}'); END;";

                await _modelContext.Database.ExecuteSqlRawAsync(PKG);
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20001)
                {
                    Console.WriteLine($"Usuário não encontrado na procedure: {ex.Number}");
                    throw;
                }

                Console.WriteLine($"Erro ao executar procedure de carga de prestadores: {ex.Number}");
                throw;
            }

            var provider = await _modelContext.ItgPrestadores.FirstOrDefaultAsync(p => p.CpfCnpj == providerCpfCnpj);

            return provider;
        }
    }
}