namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgMunicipio
{
    public int Id { get; set; }

    public int IdERP { get; set; }

    public int? Regiao { get; set; }

    public int? RegiaoDeSaude { get; set; }

    public int IdEstado { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int CodigoIbge { get; set; }

    public virtual ICollection<ItgPrestadorMunicipioExcecao> ItgPrestadorMunicipioExcecoes { get; set; } = new List<ItgPrestadorMunicipioExcecao>();

    public virtual ICollection<ItgPrestadorMunicipio> ItgPrestadorMunicipios { get; set; } = new List<ItgPrestadorMunicipio>();
}
