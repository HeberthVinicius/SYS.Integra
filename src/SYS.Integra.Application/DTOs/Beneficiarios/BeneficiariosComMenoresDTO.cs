namespace SYS.Integra.src.SYS.Integra.Application.DTOs.Beneficiarios
{
    public class BeneficiariosComMenoresDTO
    {
        public string NomeBeneficiario { get; set; } = string.Empty;
        public string CodigoBeneficiario { get; set; } = string.Empty;
        public int IdBeneficiario { get; set; }
        public int Familia { get; set; }
        public int IdFamilia { get; set; }
        public DateTime? DataAdesao { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public DateTime? DataFalecimento { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string RG { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public int Idade { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Carteirinha { get; set; } = string.Empty;
        public string Plano { get; set; } = string.Empty;
        public string Telefone1 { get; set; } = string.Empty;
        public string Telefone2 { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string CelularComercial { get; set; } = string.Empty;
        public string FaxComercial { get; set; } = string.Empty;
        public string TelefoneComercial1 { get; set; } = string.Empty;
        public string TelefoneComercial2 { get; set; } = string.Empty;
        public string Celular1 { get; set; } = string.Empty;
        public string Celular2 { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmailCorporativo { get; set; } = string.Empty;
        public string TipoDependente { get; set; } = string.Empty;
        public string IdBeneficiarioTitular { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public string StatusBeneficiario { get; set; } = string.Empty;
        public int QtMenores { get; set; }
        public string MenoresAutorizados { get; set; } = string.Empty;
    }
}
