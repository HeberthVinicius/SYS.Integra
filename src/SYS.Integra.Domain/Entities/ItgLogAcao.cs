namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgLogAcao
{
    public int Id { get; set; }

    public int? IdUsuario { get; set; }

    public string Metodo { get; set; } = string.Empty;

    public string Registro { get; set; } = string.Empty;

    public DateTime DataOperacao { get; set; }

    public virtual ItgUsuario? IdUsuarioNavigation { get; set; }
}
