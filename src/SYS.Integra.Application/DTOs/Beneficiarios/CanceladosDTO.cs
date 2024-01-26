namespace SYS.Integra.src.SYS.Integra.Application.DTOs.Beneficiarios
{
    public class CanceladosDTO
    {
        public string CPF { get; set; } = string.Empty;
        public int IdBeneficiario { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public DateTime? DataFalecimento { get; set; }
        public string StatusBeneficiario { get; set; } = string.Empty;
        public int IdMotivoCancelamento { get; set; }
        public string MotivoCancelamento { get; set; } = string.Empty;
    }
}