using SYS.Integra.src.SYS.Integra.Application.DTOs.Beneficiarios;

namespace SYS.Integra.src.SYS.Integra.Application.Interfaces
{
    public interface IBeneficiariosService
    {
        //IEnumerable<BeneficiariosDTO> GetSegurados(DateTime dtFinal);
        IEnumerable<BeneficiariosDTO> GetSegurados(int idPrestador,DateTime dtFinal);
        IEnumerable<BeneficiariosComMenoresDTO> GetSeguradosComMenores(DateTime dtFinal);
        IEnumerable<BeneficiariosDTO> GetMenores(DateTime dtInicial, DateTime dtFinal);
        IEnumerable<BeneficiariosDTO> GetNovos(DateTime dtInicial, DateTime dtFinal);
        IEnumerable<BloqueadosDTO> GetBloqueados(DateTime dtInicial, DateTime dtFinal);
        IEnumerable<CanceladosDTO> GetCancelados(DateTime dtInicial, DateTime dtFinal);
    }

}
