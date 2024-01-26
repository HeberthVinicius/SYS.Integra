namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgFilial
{
    public int Id { get; set; }

    public int IdFilialERP { get; set; }

    public int IdEstado { get; set; }

    public string SiglaFilial { get; set; } = string.Empty;

    public virtual ItgEstado IdEstadoNavigation { get; set; } = null!;
}
