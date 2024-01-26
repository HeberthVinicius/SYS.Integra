namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgPrestador
{
    public int Id { get; set; }

    public int IdPrestadorERP { get; set; }

    public string RazaoSocial { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string CpfCnpj { get; set; } = string.Empty;

    public int Ativo { get; set; }

    public DateTime DataCriacao { get; set; }

    public DateTime DataVerificacao { get; set; }

    public virtual ICollection<ItgPrestadorEstado> ItgPrestadorEstados { get; set; } = new List<ItgPrestadorEstado>();

    public virtual ICollection<ItgPrestadorMunicipioExcecao> ItgPrestadorMunicipioExcecoes { get; set; } = new List<ItgPrestadorMunicipioExcecao>();

    public virtual ICollection<ItgPrestadorMunicipio> ItgPrestadorMunicipios { get; set; } = new List<ItgPrestadorMunicipio>();

    public virtual ICollection<ItgPrestadorRegra> ItgPrestadorRegras { get; set; } = new List<ItgPrestadorRegra>();

    public virtual ICollection<ItgUsuario> ItgUsuarios { get; set; } = new List<ItgUsuario>();
}
