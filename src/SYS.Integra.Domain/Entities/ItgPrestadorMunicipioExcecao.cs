namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgPrestadorMunicipioExcecao
{
    public int Id { get; set; }

    public int IdPrestador { get; set; }

    public int IdMunicipio { get; set; }

    public virtual ItgMunicipio IdMunicipioNavigation { get; set; } = null!;

    public virtual ItgPrestador IdPrestadorNavigation { get; set; } = null!;
}
