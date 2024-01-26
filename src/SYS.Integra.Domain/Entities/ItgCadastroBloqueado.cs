namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgCadastroBloqueado
{
    public int Id { get; set; }

    public int IdBeneficiario { get; set; }

    public DateTime? DataBloqueio { get; set; }

    public DateTime? DataDesbloqueio { get; set; }
}
