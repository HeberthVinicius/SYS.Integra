using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SYS.Integra.src.SYS.Integra.Domain.Entities;

namespace SYS.Integra.src.SYS.Integra.Infraestructure;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ItgCadastroAlterado> ItgCadastroAlterados { get; set; }

    public virtual DbSet<ItgCadastroBloqueado> ItgCadastroBloqueados { get; set; }

    public virtual DbSet<ItgEstado> ItgEstados { get; set; }

    public virtual DbSet<ItgFilial> ItgFilials { get; set; }

    public virtual DbSet<ItgLogAcao> ItgLogAcoes { get; set; }

    public virtual DbSet<ItgLogAcesso> ItgLogAcessos { get; set; }

    public virtual DbSet<ItgMunicipio> ItgMunicipios { get; set; }

    public virtual DbSet<ItgPrestador> ItgPrestadores { get; set; }

    public virtual DbSet<ItgPrestadorEstado> ItgPrestadorEstados { get; set; }

    public virtual DbSet<ItgPrestadorMunicipio> ItgPrestadorMunicipios { get; set; }

    public virtual DbSet<ItgPrestadorMunicipioExcecao> ItgPrestadorMunicipioExcecoes { get; set; }

    public virtual DbSet<ItgPrestadorRegra> ItgPrestadorRegras { get; set; }

    public virtual DbSet<ItgUsuario> ItgUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("USREmpresa")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<ItgCadastroAlterado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_CADASTRO_ALTERADO_PK");

            entity.ToTable("ITG_CADASTRO_ALTERADO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.DataAlteracao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_ALTERACAO");
            entity.Property(e => e.IdBeneficiario)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_BENEFICIARIO");
        });

        modelBuilder.Entity<ItgCadastroBloqueado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_BLOQUEADOS_PK");

            entity.ToTable("ITG_CADASTRO_BLOQUEADO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.DataBloqueio)
                .HasColumnType("DATE")
                .HasColumnName("DATA_BLOQUEIO");
            entity.Property(e => e.DataDesbloqueio)
                .HasColumnType("DATE")
                .HasColumnName("DATA_DESBLOQUEIO");
            entity.Property(e => e.IdBeneficiario)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_BENEFICIARIO");
        });

        modelBuilder.Entity<ItgEstado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_ESTADO_PK");

            entity.ToTable("ITG_ESTADO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.CodigoIbge)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CODIGO_IBGE");
            entity.Property(e => e.IdERP)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_ERP");
            entity.Property(e => e.Nome)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("NOME");
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("UF");
        });

        modelBuilder.Entity<ItgFilial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_FILIAL_PK");

            entity.ToTable("ITG_FILIAL");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.IdEstado)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_ESTADO");
            entity.Property(e => e.IdFilialERP)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_FILIAL_ERP");
            entity.Property(e => e.SiglaFilial)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("SIGLA_FILIAL");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.ItgFilials)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_FILIAL_ITG_ESTADO_FK");
        });

        modelBuilder.Entity<ItgLogAcao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_LOG_ACAO_PK");

            entity.ToTable("ITG_LOG_ACAO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.DataOperacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_OPERACAO");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.Metodo)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("METODO");
            entity.Property(e => e.Registro)
                .HasColumnType("CLOB")
                .HasColumnName("REGISTRO");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ItgLogAcoes)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("ITG_LOG_ACAO_ITG_USUARIO_FK");
        });

        modelBuilder.Entity<ItgLogAcesso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_LOG_ACESSO_PK");

            entity.ToTable("ITG_LOG_ACESSO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.DataAcesso)
                .HasColumnType("DATE")
                .HasColumnName("DATA_ACESSO");
            entity.Property(e => e.DataExpiracao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_EXPIRACAO");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_USUARIO");
            entity.Property(e => e.IpAcesso)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("IP_ACESSO");
            entity.Property(e => e.LoginAceito)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("LOGIN_ACEITO");
            entity.Property(e => e.Registro)
                .HasColumnType("CLOB")
                .HasColumnName("REGISTRO");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ItgLogAcessos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("ITG_LOG_ACESSO_ITG_USUARIO_FK");
        });

        modelBuilder.Entity<ItgMunicipio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_MUNICIPIO_PK");

            entity.ToTable("ITG_MUNICIPIO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.CodigoIbge)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CODIGO_IBGE");
            entity.Property(e => e.IdERP)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_ERP");
            entity.Property(e => e.IdEstado)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_ESTADO");
            entity.Property(e => e.Nome)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("NOME");
            entity.Property(e => e.Regiao)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("REGIAO");
            entity.Property(e => e.RegiaoDeSaude)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("REGIAO_DE_SAUDE");
        });

        modelBuilder.Entity<ItgPrestador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_PRESTADOR_PK");

            entity.ToTable("ITG_PRESTADOR");

            entity.HasIndex(e => e.CpfCnpj, "ITG_PRESTADOR__UN").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Ativo)
                .HasDefaultValueSql("1 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ATIVO");
            entity.Property(e => e.CpfCnpj)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("CPF_CNPJ");
            entity.Property(e => e.DataCriacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_CRIACAO");
            entity.Property(e => e.DataVerificacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_VERIFICACAO");
            entity.Property(e => e.IdPrestadorERP)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_PRESTADOR_ERP");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NOME");
            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("RAZAO_SOCIAL");
        });

        modelBuilder.Entity<ItgPrestadorEstado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_PRESTADOR_ESTADO_PK");

            entity.ToTable("ITG_PRESTADOR_ESTADO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.AbrangenciaTotal)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ABRANGENCIA_TOTAL");
            entity.Property(e => e.DataVerificacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_VERIFICACAO");
            entity.Property(e => e.IdEstado)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_ESTADO");
            entity.Property(e => e.IdPrestador)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_PRESTADOR");
            entity.Property(e => e.Vigente)
                //.HasPrecision(1)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("VIGENTE");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.ItgPrestadorEstados)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_PRESTADOR_ESTADO_ITG_ESTADO_FK");

            entity.HasOne(d => d.IdPrestadorNavigation).WithMany(p => p.ItgPrestadorEstados)
                .HasForeignKey(d => d.IdPrestador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_PRESTADOR_ESTADO_ITG_PRESTADOR_FK");
        });

        modelBuilder.Entity<ItgPrestadorMunicipio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_PRESTADOR_MUNICIPIO_PK");

            entity.ToTable("ITG_PRESTADOR_MUNICIPIO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.DataVerificacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_VERIFICACAO");
            entity.Property(e => e.IdMunicipio)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_MUNICIPIO");
            entity.Property(e => e.IdPrestador)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_PRESTADOR");
            entity.Property(e => e.Vigente)
                .IsRequired()
                //.HasPrecision(1)
                .HasColumnType("NUMBER(38)")
                .HasDefaultValueSql("1")
                .HasColumnName("VIGENTE");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.ItgPrestadorMunicipios)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_PRESTADOR_MUNICIPIO_ITG_MUNICIPIO_FK");

            entity.HasOne(d => d.IdPrestadorNavigation).WithMany(p => p.ItgPrestadorMunicipios)
                .HasForeignKey(d => d.IdPrestador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_PRESTADOR_MUNICIPIO_ITG_PRESTADOR_FK");
        });

        modelBuilder.Entity<ItgPrestadorMunicipioExcecao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_PRESTADOR_MUNICIPIO_EXCECAO_PK");

            entity.ToTable("ITG_PRESTADOR_MUNICIPIO_EXCECAO");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.IdMunicipio)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_MUNICIPIO");
            entity.Property(e => e.IdPrestador)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_PRESTADOR");

            entity.HasOne(d => d.IdMunicipioNavigation).WithMany(p => p.ItgPrestadorMunicipioExcecoes)
                .HasForeignKey(d => d.IdMunicipio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_PRESTADOR_MUNICIPIO_EXCECAO_ITG_MUNICIPIO_FK");

            entity.HasOne(d => d.IdPrestadorNavigation).WithMany(p => p.ItgPrestadorMunicipioExcecoes)
                .HasForeignKey(d => d.IdPrestador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_PRESTADOR_MUNICIPIO_EXCECAO_ITG_PRESTADOR_FK");
        });

        modelBuilder.Entity<ItgPrestadorRegra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_PRESTADOR_REGRA_PK");

            entity.ToTable("ITG_PRESTADOR_REGRA");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.IdPrestador)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_PRESTADOR");
            entity.Property(e => e.RestricaoMenorIdade)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RESTRICAO_MENOR_IDADE");
            entity.Property(e => e.RestricaoPaiMae)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RESTRICAO_PAI_MAE");

            entity.HasOne(d => d.IdPrestadorNavigation).WithMany(p => p.ItgPrestadorRegras)
                .HasForeignKey(d => d.IdPrestador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_PRESTADOR_REGRA_ITG_PRESTADOR_FK");
        });

        modelBuilder.Entity<ItgUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ITG_USUARIO_PK");

            entity.ToTable("ITG_USUARIO");

            entity.HasIndex(e => new { e.Login, e.Email }, "ITG_USUARIO__UN").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Ativo)
                .HasDefaultValueSql("1 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ATIVO");
            entity.Property(e => e.DataAtualizacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_ATUALIZACAO");
            entity.Property(e => e.DataCriacao)
                .HasColumnType("DATE")
                .HasColumnName("DATA_CRIACAO");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.GerenciarUsuario)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("GERENCIAR_USUARIO");
            entity.Property(e => e.IdPrestador)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_PRESTADOR");
            entity.Property(e => e.IdUsuarioCadastro)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID_USUARIO_CADASTRO");
            entity.Property(e => e.Login)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("LOGIN");
            entity.Property(e => e.Nome)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("NOME");
            entity.Property(e => e.PrimeiroAcesso)
                .HasDefaultValueSql("1 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PRIMEIRO_ACESSO");
            entity.Property(e => e.ResetarSenha)
                .HasDefaultValueSql("1 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("RESETAR_SENHA");
            entity.Property(e => e.SenhaHash)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("SENHA_HASH");
            entity.Property(e => e.UsuarioGetec)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USUARIO_GETEC");

            entity.HasOne(d => d.IdPrestadorNavigation).WithMany(p => p.ItgUsuarios)
                .HasForeignKey(d => d.IdPrestador)
                .HasConstraintName("ITG_USUARIO_ITG_PRESTADOR_FK");

            entity.HasOne(d => d.IdUsuarioCadastroNavigation).WithMany(p => p.InverseIdUsuarioCadastroNavigation)
                .HasForeignKey(d => d.IdUsuarioCadastro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ITG_USUARIO_ITG_USUARIO_FK");
        });
        modelBuilder.HasSequence("ITG_BLOQUEADOS_ID_SEQ");
        modelBuilder.HasSequence("ITG_LOG_ACAO_ID_SEQ");
        modelBuilder.HasSequence("ITG_LOG_ACESSO_ID_SEQ");
        modelBuilder.HasSequence("ITG_PRESTADOR_ID_SEQ");
        modelBuilder.HasSequence("ITG_USUARIO_ID_SEQ");
        modelBuilder.HasSequence("SEQ_ITG_CADASTRO_ALTERADO_ID");
        modelBuilder.HasSequence("SEQ_ITG_CADASTRO_BLOQUEADO_ID");
        modelBuilder.HasSequence("SEQ_ITG_ESTADO_ID");
        modelBuilder.HasSequence("SEQ_ITG_FILIAL_ID");
        modelBuilder.HasSequence("SEQ_ITG_LOG_ACAO_ID");
        modelBuilder.HasSequence("SEQ_ITG_LOG_ACESSO_ID");
        modelBuilder.HasSequence("SEQ_ITG_MUNICIPIO_ID");
        modelBuilder.HasSequence("SEQ_ITG_PRESTADOR_ESTADO_ID");
        modelBuilder.HasSequence("SEQ_ITG_PRESTADOR_ID");
        modelBuilder.HasSequence("SEQ_ITG_PRESTADOR_MUNICIPIO_EXCECAO_ID");
        modelBuilder.HasSequence("SEQ_ITG_PRESTADOR_MUNICIPIO_ID");
        modelBuilder.HasSequence("SEQ_ITG_PRESTADOR_REGRA_ID");
        modelBuilder.HasSequence("SEQ_ITG_USUARIO_ID");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
