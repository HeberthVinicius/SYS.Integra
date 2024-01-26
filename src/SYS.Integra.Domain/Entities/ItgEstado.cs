namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgEstado
{
    public int Id { get; set; }

    public int IdERP { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Uf { get; set; } = string.Empty;

    public int CodigoIbge { get; set; }

    public virtual ICollection<ItgFilial> ItgFilials { get; set; } = new List<ItgFilial>();

    public virtual ICollection<ItgPrestadorEstado> ItgPrestadorEstados { get; set; } = new List<ItgPrestadorEstado>();
}
