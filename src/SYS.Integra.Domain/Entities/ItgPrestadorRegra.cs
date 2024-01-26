namespace SYS.Integra.src.SYS.Integra.Domain.Entities;

public partial class ItgPrestadorRegra
{
    public int Id { get; set; }

    public int IdPrestador { get; set; }

    public int RestricaoPaiMae { get; set; }

    public int RestricaoMenorIdade { get; set; }

    public virtual ItgPrestador IdPrestadorNavigation { get; set; } = null!;
}
