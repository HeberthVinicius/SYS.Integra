namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgUsuario
{
    public int Id { get; set; }

    public int IdPrestador { get; set; }

    public int IdUsuarioCadastro { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string SenhaHash { get; set; } = string.Empty;

    public int PrimeiroAcesso { get; set; }

    public int ResetarSenha { get; set; }

    public int Ativo { get; set; }

    public int GerenciarUsuario { get; set; }

    public int UsuarioGetec { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime DataAtualizacao { get; set; }

    public virtual ItgPrestador? IdPrestadorNavigation { get; set; }

    public virtual ItgUsuario IdUsuarioCadastroNavigation { get; set; } = null!;

    public virtual ICollection<ItgUsuario> InverseIdUsuarioCadastroNavigation { get; set; } = new List<ItgUsuario>();

    public virtual ICollection<ItgLogAcao> ItgLogAcoes { get; set; } = new List<ItgLogAcao>();

    public virtual ICollection<ItgLogAcesso> ItgLogAcessos { get; set; } = new List<ItgLogAcesso>();
}
