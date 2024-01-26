namespace SYS.Integra.src.SYS.Integra.Application.DTOs.Beneficiarios
{
    public class BloqueadosDTO
    {
        public string CPF { get; set; } = string.Empty;
        public int IdBeneficiario { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public DateTime? DataFalecimento { get; set; }
        public string StatusBeneficiario { get; set; } = string.Empty;
    }
}