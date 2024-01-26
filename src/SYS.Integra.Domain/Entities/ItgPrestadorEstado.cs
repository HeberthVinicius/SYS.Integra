namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgPrestadorEstado
{
    public int Id { get; set; }

    public int IdPrestador { get; set; }

    public int IdEstado { get; set; }

    public int? AbrangenciaTotal { get; set; }

    public int Vigente { get; set; }

    public DateTime DataVerificacao { get; set; }

    public virtual ItgEstado IdEstadoNavigation { get; set; } = null!;

    public virtual ItgPrestador IdPrestadorNavigation { get; set; } = null!;
}
