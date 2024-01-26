namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgCadastroAlterado
{
    public int Id { get; set; }

    public int IdBeneficiario { get; set; }

    public DateTime? DataAlteracao { get; set; }
}
