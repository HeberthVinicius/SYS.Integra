namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgLogAcesso
{
    public int Id { get; set; }

    public int? IdUsuario { get; set; }

    public string IpAcesso { get; set; } = string.Empty;

    public int LoginAceito { get; set; }

    public string Registro { get; set; } = string.Empty;

    public DateTime DataAcesso { get; set; }

    public DateTime DataExpiracao { get; set; }

    public virtual ItgUsuario? IdUsuarioNavigation { get; set; }
}
