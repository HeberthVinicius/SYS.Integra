using SYS.Integra.src.SYS.Integra.Application.DTOs.Beneficiarios;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;
using SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories;
using SYS.Integra.src.SYS.Integra.Infraestructure.Repositories;

namespace SYS.Integra.src.SYS.Integra.Application.Services
{
    public class BeneficiariosService : IBeneficiariosService
    {
        private readonly IBeneficiariosRepository _beneficiariosRepository;

        public BeneficiariosService(IBeneficiariosRepository beneficiarioRepository)
        {
            _beneficiariosRepository = beneficiarioRepository;
        }

        public IEnumerable<BeneficiariosDTO> GetSegurados(int idPrestador, DateTime dtFinal)
        {
            return _beneficiariosRepository.GetSegurados(idPrestador, dtFinal);
        }

        public IEnumerable<BeneficiariosComMenoresDTO> GetSeguradosComMenores(DateTime dtFinal)
        {
            return _beneficiariosRepository.GetSeguradosComMenores(dtFinal);
        }

        public IEnumerable<BeneficiariosDTO> GetMenores(DateTime dtInicial, DateTime dtFinal)
        {
            return _beneficiariosRepository.GetMenores(dtInicial, dtFinal);
        }

        public IEnumerable<BeneficiariosDTO> GetNovos(DateTime dtInicial, DateTime dtFinal)
        {
            return _beneficiariosRepository.GetNovos(dtInicial, dtFinal);
        }

        public IEnumerable<BloqueadosDTO> GetBloqueados(DateTime dtInicial, DateTime dtFinal)
        {
            return _beneficiariosRepository.GetBloqueados(dtInicial, dtFinal);
        }

        public IEnumerable<CanceladosDTO> GetCancelados(DateTime dtInicial, DateTime dtFinal)
        {
            return _beneficiariosRepository.GetCancelados(dtInicial, dtFinal);
        }
    }

}
