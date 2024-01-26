namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgPrestadorMunicipio
{
    public int Id { get; set; }

    public int IdPrestador { get; set; }

    public int IdMunicipio { get; set; }

    public int? Vigente { get; set; }

    public DateTime DataVerificacao { get; set; }

    public virtual ItgMunicipio IdMunicipioNavigation { get; set; } = null!;

    public virtual ItgPrestador IdPrestadorNavigation { get; set; } = null!;
}
