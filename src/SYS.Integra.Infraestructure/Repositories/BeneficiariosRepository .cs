using Dapper;
using Microsoft.EntityFrameworkCore;
using SYS.Integra.src.SYS.Integra.Application.DTOs.Beneficiarios;
using SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories;

namespace SYS.Integra.src.SYS.Integra.Infraestructure.Repositories
{
    public class BeneficiariosRepository : IBeneficiariosRepository
    {
        private readonly ERPContext _ERPContext;
        private readonly ModelContext _modelContext;

        public BeneficiariosRepository(ERPContext ERPContext, ModelContext modelContext)
        {
            _ERPContext = ERPContext;
            _modelContext = modelContext;
        }

        //TODO REGRAS ID SEGUNDA SPRINT
        /*
        1 - MAIOR IDADE
        2 - EXCLUSÃO DO TIPO DEPENDNETE PAIS tipodependente.cod != 9            
        */

        public IEnumerable<BeneficiariosDTO> GetSegurados(int idPrestador, DateTime dtFinal) 
        {
            var idPrestadorEstados = string.Join(",", (
                from itgPrestadorEstados in _modelContext.ItgPrestadorEstados
                join itgEstados in _modelContext.ItgEstados 
                on itgPrestadorEstados.IdEstado equals itgEstados.Id
                where itgPrestadorEstados.IdPrestador == idPrestador
                select itgEstados.IdERP)
                .ToList());

            var idPrestadorMunicipios = string.Join(",", (
               from itgPrestadorMunicipios in _modelContext.ItgPrestadorMunicipios
                join itgMunicipios in _modelContext.ItgMunicipios 
                on itgPrestadorMunicipios.IdMunicipio equals itgMunicipios.Id 
                where itgPrestadorMunicipios.IdPrestador == idPrestador
                select itgMunicipios.IdERP)
                .ToList());

            var sql = $@"
                                SELECT   beneficiario.nome                                          NomeBeneficiario
                                ,to_char(beneficiario.beneficiario)                                 CodigoBeneficiario
                                ,to_char(beneficiario.cod)                                       IdBeneficiario
		                        ,to_char(familia.familia)                                           Familia
                                ,to_char(familia.cod)                                            IdFamilia
                                ,beneficiario.dataadesao                                            DataAdesao
                                ,beneficiario.datacancelamento                                      DataCancelamento
                                ,beneficiario.databloqueio                                          DataBloqueio
                                ,matricula.datafalecimento                                          DataFalecimento
                                ,matricula.sexo                                                     Sexo
                                ,to_char(matricula.cpf)                                             CPF
                                ,matricula.rg                                                       RG
                                ,matricula.datanascimento                                           DataNascimento
                                ,TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) Idade
                                ,logradouros_tipo.descricao || ' ' || endereco.logradouro           Endereco
                                ,to_char(endereco.numero)                                           Nnumero
                                ,endereco.complemento                                               Complemento
                                ,endereco.bairro                                                    Bairro
                                ,municipios.nome                                                        Vidade
                                ,estados.nome                                                           Estado
                                ,estados.sigla                                                          UF
                                ,endereco.cep                                                       CEP
                                ,to_char(beneficiario.cartao)                                       Carteirinha
                                ,modulo.descricao                                                   Plano
                                ,COALESCE (endereco.telefone1,(select ender.telefone1 from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ),endereco.telefone1,null)             Telefone1
                                 ,COALESCE (endereco.telefone2,(select ender.telefone2 from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ),endereco.telefone2, null)            Telefone2     
                                 ,COALESCE (endereco.fax,(select ender.fax from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ),endereco.fax, null)                  Fax       
                                ,COALESCE (endereco_comercial.celular,
                                CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                     THEN
                                    (select ender.celular from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                     ELSE NULL END,endereco_comercial.celular, null)                    CelularComercial
                                ,COALESCE (endereco_comercial.fax,
                                 CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN
                                      (select ender.fax from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                      ELSE NULL END,endereco_comercial.fax, null)                       FaxComercial
                                ,COALESCE (endereco_comercial.telefone1,
                                 CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN
                                     (select ender.telefone1 from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                      ELSE NULL END,endereco_comercial.telefone1, null)                 TelefoneComercial1
                                ,COALESCE (endereco_comercial.telefone2,
                                 CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN
                                     (select ender.telefone2 from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                      ELSE NULL END,endereco_comercial.telefone2, null)                 TelefoneComercial2 
                                ,COALESCE (endereco.celular,(select ender.celular from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), endereco.celular, null)             Celular1
                                ,COALESCE (beneficiario.celular,(select ben.celular from beneficiario ben
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), beneficiario.celular, null)         Celular2
                                ,COALESCE (beneficiario.email,(select ben.email from beneficiario ben
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), beneficiario.email, null)           Email
                                ,CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN COALESCE (beneficiario.emailcorporativo,(select ben.emailcorporativo from beneficiario ben
                                             inner join familia fam on ben.familia = fam.cod
                                             where fam.cod = beneficiario.familia
                                             and ben.ehtitular = 'S' ), beneficiario.emailcorporativo, null) 
                                      ELSE NULL END                                                     EmailCorporativo
                                ,tipodependente.descricao                                           TipoDependente
                                ,CASE WHEN familia.titularresponsavel != beneficiario.cod
                                      THEN to_char(familia.titularresponsavel) ELSE NULL END        IdBeneficiarioTitular
                                ,matricula.nomepai                                                  NomePai 
                                ,matricula.nomemae                                                  NomeMae     
                                ,CASE  WHEN (   (beneficiario.databloqueio IS NOT NULL OR beneficiario.databloqueio <= :DT_FINAL))
                                            AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento >= :DT_FINAL)
		                        	   THEN 'BLOQUEADO'
		                        	   WHEN  (beneficiario_mod_historico.datacancelamento IS NOT NULL 
                                           AND beneficiario_mod_historico.datacancelamento <= :DT_FINAL)
		                        	   THEN 'CANCELADO' 
		                        	   WHEN (SELECT  MAX(1)
		                        			 FROM Empresa_PRD.beneficiario_suspensao
		                        			 WHERE :DT_FINAL  BETWEEN DATAINICIAL AND NVL(DATAFINAL, :DT_FINAL)
		                        			   AND (BENEFICIARIO = beneficiario.cod
		                        			    OR BENEFICIARIO = familia.titularresponsavel)) = 1
		                        	   THEN 'SUSPENSO'
		                        	   ELSE 'ATIVO' END                                                 StatusBeneficiario
                                FROM  Empresa_PRD.beneficiario
                                INNER JOIN Empresa_PRD.matricula
                                        ON beneficiario.matricula = matricula.cod
                                INNER JOIN Empresa_PRD.familia 
                                        ON beneficiario.familia =  familia.cod
                                LEFT  JOIN Empresa_PRD.endereco
                                        ON NVL(enderecoresidencial, 
                                            (SELECT enderecoresidencial 
                                               FROM Empresa_PRD.beneficiario 
                                              WHERE cod =  familia.titularresponsavel)) = endereco.cod
                                LEFT JOIN Empresa_PRD.logradouros_tipo
                                        ON endereco.tipologradouro = logradouros_tipo.cod
                                INNER JOIN Empresa_PRD.estados 
                                        ON endereco.estado = estados.cod
                                INNER JOIN Empresa_PRD.municipios
                                        ON endereco.municipio = municipios.cod
                                       AND estados.cod = municipios.estado
                                INNER JOIN Empresa_PRD.beneficiario_mod               
                                        ON beneficiario_mod.beneficiario = beneficiario.cod
                                INNER JOIN Empresa_PRD.beneficiario_mod_historico     
                                        ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                                INNER JOIN Empresa_PRD.contrato_mod                   
                                        ON contrato_mod.cod = beneficiario_mod.modulo
                                INNER JOIN Empresa_PRD.modulo                         
                                        ON modulo.cod = contrato_mod.modulo
                                LEFT  JOIN Empresa_PRD.endereco endereco_comercial
                                        ON NVL(enderecocomercial, 
                                            (SELECT enderecocomercial 
                                               FROM Empresa_PRD.beneficiario 
                                              WHERE cod =  familia.titularresponsavel)) = endereco_comercial.cod   
                                		      AND modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                INNER JOIN Empresa_PRD.CONTRATO_TPDEP
                                        ON BENEFICIARIO.TIPODEPENDENTE = CONTRATO_TPDEP.cod
                                INNER JOIN Empresa_PRD.TIPODEPENDENTE
                                        ON CONTRATO_TPDEP.TIPODEPENDENTE = TIPODEPENDENTE.cod
                                WHERE beneficiario.contrato IN (1,3,5) 
                                  AND (beneficiario_mod_historico.datacancelamento IS NULL 
                                        OR beneficiario_mod_historico.datacancelamento > :DT_FINAL
                                      )
                                  AND (beneficiario.databloqueio IS NULL 
                                        OR beneficiario.databloqueio > :DT_FINAL
                                      )
                                  AND (beneficiario.datacancelamento IS NULL 
                                        OR beneficiario.datacancelamento > :DT_FINAL
                                      )
                                  AND (beneficiario.dataadesao < :DT_FINAL)
                                  AND matricula.datafalecimento IS NULL
                                  --AND beneficiario.filialcusto IN (42)
                                  AND municipios.cod IN ({idPrestadorMunicipios})

                                  AND estados.cod IN ({idPrestadorEstados})

                                  --AND tipodependente.cod != 9                        
                       ";

            using var connection = _ERPContext.Database.GetDbConnection();
            connection.Open();

            var result = connection.Query<BeneficiariosDTO>(sql, new { DT_FINAL = dtFinal })
                                 .ToList();

            foreach (var beneficiario in result)
            {
                beneficiario.NomeBeneficiario = beneficiario.NomeBeneficiario.ToUpper();
                beneficiario.DataAdesao = (beneficiario.DataAdesao == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataAdesao;
                beneficiario.DataCancelamento = (beneficiario.DataCancelamento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataCancelamento;
                beneficiario.DataBloqueio = (beneficiario.DataBloqueio == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataBloqueio;
                beneficiario.DataFalecimento = (beneficiario.DataFalecimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataFalecimento;
                beneficiario.DataNascimento = (beneficiario.DataNascimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataNascimento;
                // Outros tratamentos...
            }

            return result;

        }

        public IEnumerable<BeneficiariosComMenoresDTO> GetSeguradosComMenores(DateTime dtFinal)
        {
            // Utilize o Dapper para executar a query e mapear os resultados para uma lista de BeneficiarioDTO
            var sql = @"
                        SELECT   beneficiario.nome                                          NomeBeneficiario
                        ,to_char(beneficiario.beneficiario)                                 CodigoBeneficiario
                        ,beneficiario.cod                                                IdBeneficiario
		                ,familia.familia                                                    Familia
                        ,familia.cod                                                     IdFamilia
                        ,beneficiario.dataadesao                                            DataAdesao
                        ,beneficiario.datacancelamento                                      DataCancelamento
                        ,beneficiario.databloqueio                                          DataBloqueio
                        ,matricula.datafalecimento                                          DataFalecimento
                        ,matricula.sexo                                                     Sexo
                        ,to_char(matricula.cpf)                                             CPF
                        ,matricula.rg                                                       RG
                        ,matricula.datanascimento                                           DataNascimento
                        ,TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) Idade
                        ,logradouros_tipo.descricao || ' ' || endereco.logradouro           Endereco
                        ,endereco.numero                                                    Numero
                        ,endereco.complemento                                               Complemento
                        ,endereco.bairro                                                    Bairro
                        ,municipios.nome                                                        Cidade
                        ,estados.nome                                                           Estado
                        ,estados.sigla                                                          UF
                        ,endereco.cep                                                       CEP
                        ,to_char(beneficiario.cartao)                                       Carteirinha
                        ,modulo.descricao                                                   Plano
                        ,COALESCE (endereco.telefone1,(select ender.telefone1 from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ),endereco.telefone1,null)             Telefone1
                         ,COALESCE (endereco.telefone2,(select ender.telefone2 from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ),endereco.telefone2, null)            Telefone2
                         ,COALESCE (endereco.fax,(select ender.fax from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ),endereco.fax, null)                  Fax
                        ,COALESCE (endereco_comercial.celular,
                        CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                             THEN
                            (select ender.celular from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                             ELSE NULL END,endereco_comercial.celular, null)                    CelularComercial
                        ,COALESCE (endereco_comercial.fax,
                         CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN
                              (select ender.fax from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                              ELSE NULL END,endereco_comercial.fax, null)                       FaxComercial
                        ,COALESCE (endereco_comercial.telefone1,
                         CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN
                             (select ender.telefone1 from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                              ELSE NULL END,endereco_comercial.telefone1, null)                 TelefoneComercial1
                        ,COALESCE (endereco_comercial.telefone2,
                         CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN
                             (select ender.telefone2 from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                              ELSE NULL END,endereco_comercial.telefone2, null)                 TelefoneComercial2
                        ,COALESCE (endereco.celular,(select ender.celular from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ), endereco.celular, null)             Celular1
                        ,COALESCE (beneficiario.celular,(select ben.celular from beneficiario ben
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ), beneficiario.celular, null)         Celular2
                        ,COALESCE (beneficiario.email,(select ben.email from beneficiario ben
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ), beneficiario.email, null)           Email
                        ,CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN COALESCE (beneficiario.emailcorporativo,(select ben.emailcorporativo from beneficiario ben
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), beneficiario.emailcorporativo, null)
                              ELSE NULL END                                                     EmailCorporativo
                        ,tipodependente.descricao                                           TipoDependente
                        ,CASE WHEN familia.titularresponsavel != beneficiario.cod
                              THEN to_char(familia.titularresponsavel) ELSE NULL END                 IdBeneficiarioTitular
                        ,matricula.nomepai                                                  NomePai
                        ,matricula.nomemae                                                  NomeMae
                        ,CASE  WHEN (   (beneficiario.databloqueio IS NOT NULL OR beneficiario.databloqueio <= :DT_FINAL))
                                    AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento >= :DT_FINAL)
		                	   THEN 'BLOQUEADO'
		                	   WHEN  (beneficiario_mod_historico.datacancelamento IS NOT NULL
                                   AND beneficiario_mod_historico.datacancelamento <= :DT_FINAL)
		                	   THEN 'CANCELADO'
		                	   WHEN (SELECT  MAX(1)
		                			 FROM Empresa_PRD.beneficiario_suspensao
		                			 WHERE :DT_FINAL  BETWEEN DATAINICIAL AND NVL(DATAFINAL, :DT_FINAL)
		                			   AND (BENEFICIARIO = beneficiario.cod
		                			    OR BENEFICIARIO = familia.titularresponsavel)) = 1
		                	   THEN 'SUSPENSO'
		                	   ELSE 'ATIVO' END                                                 StatusBeneficiario
                        ,NVL(total_menores.qt_menores, 0)                                       QtMenores
                        ,NVL(menores_autorizados.menores_autorizados, 'NAO')                    MenoresAutorizados

                        FROM  Empresa_PRD.beneficiario
                        INNER JOIN Empresa_PRD.matricula
                                ON beneficiario.matricula = matricula.cod
                        INNER JOIN Empresa_PRD.familia
                                ON beneficiario.familia =  familia.cod
                        LEFT  JOIN Empresa_PRD.endereco
                                ON NVL(enderecoresidencial,
                                    (SELECT enderecoresidencial
                                       FROM Empresa_PRD.beneficiario
                                      WHERE cod =  familia.titularresponsavel)) = endereco.cod
                        LEFT JOIN Empresa_PRD.logradouros_tipo
                                ON endereco.tipologradouro = logradouros_tipo.cod
                        INNER JOIN Empresa_PRD.estados
                                ON endereco.estado = estados.cod
                        INNER JOIN Empresa_PRD.municipios
                                ON endereco.municipio = municipios.cod
                               AND estados.cod = municipios.estado
                        INNER JOIN Empresa_PRD.beneficiario_mod
                                ON beneficiario_mod.beneficiario = beneficiario.cod
                        INNER JOIN Empresa_PRD.beneficiario_mod_historico
                                ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                        INNER JOIN Empresa_PRD.contrato_mod
                                ON contrato_mod.cod = beneficiario_mod.modulo
                        INNER JOIN Empresa_PRD.modulo
                                ON modulo.cod = contrato_mod.modulo
                        INNER JOIN Empresa_PRD.CONTRATO_TPDEP
                                ON BENEFICIARIO.TIPODEPENDENTE = CONTRATO_TPDEP.cod
                        INNER JOIN Empresa_PRD.TIPODEPENDENTE
                                ON CONTRATO_TPDEP.TIPODEPENDENTE = TIPODEPENDENTE.cod
                        LEFT  JOIN Empresa_PRD.endereco endereco_comercial
                                ON NVL(enderecocomercial,
                                    (SELECT enderecocomercial
                                       FROM Empresa_PRD.beneficiario
                                      WHERE cod =  familia.titularresponsavel)) = endereco_comercial.cod
                        		      AND modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                        /*TABELA DERIVADA PARA TRAZER A QUANTIDADE DE MENORES COM AUTORIZACAO*/
                        LEFT  JOIN (SELECT  CASE WHEN COUNT(beneficiario.cod) > 0
                                            THEN 'SIM' END                                   menores_autorizados,
                                                familia.titularresponsavel
                                        FROM beneficiario
                                        INNER JOIN familia
                                                ON beneficiario.familia = familia.cod
                                        INNER JOIN Empresa_PRD.matricula
                                                ON beneficiario.matricula = matricula.cod
                                        INNER JOIN Empresa_PRD.beneficiario_mod
                                                ON beneficiario_mod.beneficiario = beneficiario.cod
                                        INNER JOIN Empresa_PRD.beneficiario_mod_historico
                                                ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                                        LEFT  JOIN Empresa_PRD.endereco
                                                ON NVL(enderecoresidencial,
                                                    (SELECT enderecoresidencial
                                                       FROM Empresa_PRD.beneficiario
                                                      WHERE cod =  familia.titularresponsavel)) = endereco.cod
                                        INNER JOIN Empresa_PRD.estados
                                                ON endereco.estado = estados.cod
                                        INNER JOIN Empresa_PRD.municipios
                                                ON endereco.municipio = municipios.cod
                                               AND estados.cod = municipios.estado
                        
                                        WHERE beneficiario.ehtitular = 'N'
                                        AND TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) < 18
                        
                                        AND (beneficiario_mod_historico.datacancelamento IS NULL
                                            OR beneficiario_mod_historico.datacancelamento > :DT_FINAL
                                          )
                                        AND (beneficiario.databloqueio IS NULL
                                            OR beneficiario.databloqueio > :DT_FINAL
                                          )
                                        AND (beneficiario.datacancelamento IS NULL
                                            OR beneficiario.datacancelamento > :DT_FINAL
                                          )
                        
                                        AND familia.titularresponsavel IN (SELECT beneficiario
                                                                                 FROM beneficiario_anotadm
                                                                                 WHERE anotacao = 485
                                                                                 )
                                        AND matricula.datafalecimento IS NULL
                                        --PARAMETROS PARA CADA PRESTADOR
                        				AND beneficiario.filialcusto IN (42)
                        				AND municipios.cod IN (9760, 9944, 9954, 9960, 9964)
                        				--AND estados.cod = 8
                        
                                        GROUP BY titularresponsavel) menores_autorizados
                              ON beneficiario.cod = menores_autorizados.titularresponsavel
                        /*TABELA DERIVADA PARA TRAZER A QUANTIDADE TOTAL DE MENORES POR FAMILIA*/
                        INNER JOIN (SELECT  COUNT(beneficiario.cod) qt_menores,
                                        familia.titularresponsavel
                                FROM beneficiario
                                INNER JOIN familia
                                        ON beneficiario.familia = familia.cod
                                INNER JOIN Empresa_PRD.matricula
                                        ON beneficiario.matricula = matricula.cod
                                INNER JOIN Empresa_PRD.beneficiario_mod
                                        ON beneficiario_mod.beneficiario = beneficiario.cod
                                INNER JOIN Empresa_PRD.beneficiario_mod_historico
                                        ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                                LEFT  JOIN Empresa_PRD.endereco
                                        ON NVL(enderecoresidencial,
                                            (SELECT enderecoresidencial
                                               FROM Empresa_PRD.beneficiario
                                              WHERE cod =  familia.titularresponsavel)) = endereco.cod
                                INNER JOIN Empresa_PRD.estados
                                        ON endereco.estado = estados.cod
                                INNER JOIN Empresa_PRD.municipios
                                        ON endereco.municipio = municipios.cod
                                       AND estados.cod = municipios.estado
                        
                                WHERE beneficiario.ehtitular = 'N'
                                AND TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) < 18
                        
                                AND (beneficiario_mod_historico.datacancelamento IS NULL
                                    OR beneficiario_mod_historico.datacancelamento > :DT_FINAL
                                  )
                                AND (beneficiario.databloqueio IS NULL
                                    OR beneficiario.databloqueio > :DT_FINAL
                                  )
                                AND (beneficiario.datacancelamento IS NULL
                                    OR beneficiario.datacancelamento > :DT_FINAL
                                  )
                                AND matricula.datafalecimento IS NULL
                                --PARAMETROS PARA CADA PRESTADOR
                                AND beneficiario.filialcusto IN (42)
                        		AND municipios.cod IN (9760, 9944, 9954, 9960, 9964)
                        		--AND estados.cod = 8
                                GROUP BY titularresponsavel) total_menores
                            ON beneficiario.cod = total_menores.titularresponsavel
                        
                        WHERE beneficiario.contrato IN (1,3,5)
                          AND (beneficiario_mod_historico.datacancelamento IS NULL
                                OR beneficiario_mod_historico.datacancelamento > :DT_FINAL
                                )
                          AND (beneficiario.databloqueio IS NULL
                                OR beneficiario.databloqueio > :DT_FINAL
                                )
                          AND (beneficiario.datacancelamento IS NULL
                                OR beneficiario.datacancelamento > :DT_FINAL
                                )
                          AND TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) >= 18
                          AND (beneficiario.dataadesao < :DT_FINAL)
                          AND matricula.datafalecimento IS NULL
                          AND BENEFICIARIO.EHTITULAR = 'S'
                          AND beneficiario.filialcusto IN (42)
                          AND municipios.cod IN (9760, 9944, 9954, 9960, 9964)
                          --AND estados.cod = 8    
            ";

            using var connection = _ERPContext.Database.GetDbConnection();
            connection.Open();

            var result = connection.Query<BeneficiariosComMenoresDTO>(sql, new { DT_FINAL = dtFinal })
                                 .ToList();

            foreach (var beneficiario in result)
            {
                beneficiario.NomeBeneficiario = beneficiario.NomeBeneficiario.ToUpper();
                beneficiario.DataAdesao = (beneficiario.DataAdesao == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataAdesao;
                beneficiario.DataCancelamento = (beneficiario.DataCancelamento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataCancelamento;
                beneficiario.DataBloqueio = (beneficiario.DataBloqueio == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataBloqueio;
                beneficiario.DataFalecimento = (beneficiario.DataFalecimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataFalecimento;
                beneficiario.DataNascimento = (beneficiario.DataNascimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataNascimento;
                // Outros tratamentos...
            }

            return result;
        }

        public IEnumerable<BeneficiariosDTO> GetMenores(DateTime dtInicial, DateTime dtFinal)
        {
            var sql = @"
                        SELECT   beneficiario.nome                                          nome_beneficiario
                        ,to_char(beneficiario.beneficiario)                                 codigo_beneficiario
                        ,to_char(beneficiario.cod)                                       id_beneficiario 
		                ,to_char(familia.familia)                                           familia
                        ,to_char(familia.cod)                                            id_familia
                        ,beneficiario.dataadesao                                            data_adesao
                        ,beneficiario.datacancelamento                                      data_cancelamento
                        ,beneficiario.databloqueio                                          data_bloqueio
                        ,matricula.datafalecimento                                          data_falecimento
                        ,matricula.sexo                                                     sexo
                        ,to_char(matricula.cpf)                                             cpf
                        ,matricula.rg                                                       rg
                        ,matricula.datanascimento                                           data_nascimento
                        ,TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) idade
                        ,logradouros_tipo.descricao || ' ' || endereco.logradouro           endereco
                        ,to_char(endereco.numero)                                           numero
                        ,endereco.complemento                                               complemento
                        ,endereco.bairro                                                    bairro
                        ,municipios.nome                                                        cidade
                        ,estados.nome                                                           estado
                        ,estados.sigla                                                          uf
                        ,endereco.cep                                                       cep
                        ,to_char(beneficiario.cartao)                                       carteirinha
                        ,modulo.descricao                                                   plano
                        ,COALESCE (endereco.telefone1,(select ender.telefone1 from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ),endereco.telefone1,null)             telefone1
                         ,COALESCE (endereco.telefone2,(select ender.telefone2 from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ),endereco.telefone2, null)            telefone2     
                         ,COALESCE (endereco.fax,(select ender.fax from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ),endereco.fax, null)                  fax       
                        ,COALESCE (endereco_comercial.celular,
                        CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                             THEN
                            (select ender.celular from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                             ELSE NULL END,endereco_comercial.celular, null)                    celular_comercial
                        ,COALESCE (endereco_comercial.fax,
                         CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN
                              (select ender.fax from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                              ELSE NULL END,endereco_comercial.fax, null)                       fax_comercial
                        ,COALESCE (endereco_comercial.telefone1,
                         CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN
                             (select ender.telefone1 from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                              ELSE NULL END,endereco_comercial.telefone1, null)                 telefone_comercial1
                        ,COALESCE (endereco_comercial.telefone2,
                         CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN
                             (select ender.telefone2 from beneficiario ben
                                 inner join endereco ender on ben.enderecocomercial = ender.cod
                                 inner join familia fam on ben.familia = fam.cod
                                 where fam.cod = beneficiario.familia
                                 and ben.ehtitular = 'S' )
                              ELSE NULL END,endereco_comercial.telefone2, null)                 telefone_comercial2 
                        ,COALESCE (endereco.celular,(select ender.celular from beneficiario ben
                             inner join endereco ender on ben.enderecoresidencial = ender.cod
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ), endereco.celular, null)             celular1
                        ,COALESCE (beneficiario.celular,(select ben.celular from beneficiario ben
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ), beneficiario.celular, null)         celular2
                        ,COALESCE (beneficiario.email,(select ben.email from beneficiario ben
                             inner join familia fam on ben.familia = fam.cod
                             where fam.cod = beneficiario.familia
                             and ben.ehtitular = 'S' ), beneficiario.email, null)           email
                        ,CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                              THEN COALESCE (beneficiario.emailcorporativo,(select ben.emailcorporativo from beneficiario ben
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), beneficiario.emailcorporativo, null) 
                              ELSE NULL END                                                     email_corporativo
                        ,tipodependente.descricao                                           tipo_dependente
                        ,CASE WHEN familia.titularresponsavel != beneficiario.cod
                              THEN to_char(familia.titularresponsavel) ELSE NULL END                 id_beneficiario_titular
                        ,matricula.nomepai                                                  nome_pai 
                        ,matricula.nomemae                                                  nome_mae     
                        ,CASE  WHEN (   (beneficiario.databloqueio IS NOT NULL OR beneficiario.databloqueio <= :DT_FINAL))
                                    AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento >= :DT_FINAL)
		                	   THEN 'BLOQUEADO'
		                	   WHEN  (beneficiario_mod_historico.datacancelamento IS NOT NULL 
                                   AND beneficiario_mod_historico.datacancelamento <= :DT_FINAL)
		                	   THEN 'CANCELADO' 
		                	   WHEN (SELECT  MAX(1)
		                			 FROM Empresa_PRD.beneficiario_suspensao
		                			 WHERE :DT_FINAL  BETWEEN DATAINICIAL AND NVL(DATAFINAL, :DT_FINAL)
		                			   AND (BENEFICIARIO = beneficiario.cod
		                			    OR BENEFICIARIO = familia.titularresponsavel)) = 1
		                	   THEN 'SUSPENSO'
		                	   ELSE 'ATIVO' END                                                 status_beneficiario
                               FROM  Empresa_PRD.beneficiario
                               INNER JOIN Empresa_PRD.matricula
                                       ON beneficiario.matricula = matricula.cod
                               INNER JOIN Empresa_PRD.familia 
                                       ON beneficiario.familia =  familia.cod
                               LEFT  JOIN Empresa_PRD.endereco
                                       ON NVL(enderecoresidencial, 
                                           (SELECT enderecoresidencial 
                                              FROM Empresa_PRD.beneficiario 
                                             WHERE cod =  familia.titularresponsavel)) = endereco.cod
                               LEFT JOIN Empresa_PRD.logradouros_tipo
                                       ON endereco.tipologradouro = logradouros_tipo.cod
                               INNER JOIN Empresa_PRD.estados 
                                       ON endereco.estado = estados.cod
                               INNER JOIN Empresa_PRD.municipios
                                       ON endereco.municipio = municipios.cod
                                      AND estados.cod = municipios.estado
                               INNER JOIN Empresa_PRD.beneficiario_mod               
                                       ON beneficiario_mod.beneficiario = beneficiario.cod
                               INNER JOIN Empresa_PRD.beneficiario_mod_historico     
                                       ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                               INNER JOIN Empresa_PRD.contrato_mod                   
                                       ON contrato_mod.cod = beneficiario_mod.modulo
                               INNER JOIN Empresa_PRD.modulo                         
                                       ON modulo.cod = contrato_mod.modulo
                               INNER JOIN Empresa_PRD.CONTRATO_TPDEP
                                       ON BENEFICIARIO.TIPODEPENDENTE = CONTRATO_TPDEP.cod
                               INNER JOIN Empresa_PRD.TIPODEPENDENTE
                                       ON CONTRATO_TPDEP.TIPODEPENDENTE = TIPODEPENDENTE.cod
                               LEFT  JOIN beneficiario_anotadm
                                       ON beneficiario.cod = beneficiario_anotadm.beneficiario 
                                      AND beneficiario_anotadm.anotacao = 485
                               LEFT  JOIN Empresa_PRD.endereco endereco_comercial
                                       ON NVL(enderecocomercial, 
                                           (SELECT enderecocomercial 
                                              FROM Empresa_PRD.beneficiario 
                                             WHERE cod =  familia.titularresponsavel)) = endereco_comercial.cod   
                               		      AND modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                               WHERE beneficiario.contrato IN (1,3,5) 
                                 AND (beneficiario_mod_historico.datacancelamento IS NULL 
                                       OR beneficiario_mod_historico.datacancelamento > :DT_FINAL
                               )
                                 AND (beneficiario.databloqueio IS NULL 
                                       OR beneficiario.databloqueio > :DT_FINAL)
                                 AND (beneficiario.datacancelamento IS NULL 
                                       OR beneficiario.datacancelamento > :DT_FINAL
                                     )                        
                                 AND TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) < 18
                                 AND matricula.datafalecimento IS NULL
                               
                               --   AND familia.titularresponsavel IN (SELECT beneficiario
                               --                                          FROM beneficiario_anotadm
                               --                                          WHERE anotacao = 485
                               --                                          AND DATA = :DT_INICIAL
                               --                                          )
                               AND beneficiario.filialcusto IN (42)
                               AND municipios.cod IN (9760, 9944, 9954, 9960, 9964)
                               --AND estados.cod = 8
                       ";

            using var connection = _ERPContext.Database.GetDbConnection();
            connection.Open();

            var result = connection.Query<BeneficiariosDTO>(sql, new { DT_FINAL = dtFinal })
                                 .ToList();

            foreach (var beneficiario in result)
            {
                beneficiario.NomeBeneficiario = beneficiario.NomeBeneficiario.ToUpper();
                beneficiario.DataAdesao = (beneficiario.DataAdesao == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataAdesao;
                beneficiario.DataCancelamento = (beneficiario.DataCancelamento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataCancelamento;
                beneficiario.DataBloqueio = (beneficiario.DataBloqueio == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataBloqueio;
                beneficiario.DataFalecimento = (beneficiario.DataFalecimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataFalecimento;
                beneficiario.DataNascimento = (beneficiario.DataNascimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataNascimento;
                // Outros tratamentos...
            }

            return result;
        }

        public IEnumerable<BeneficiariosDTO> GetNovos(DateTime dtInicial, DateTime dtFinal)
        {
            var sql = @"
                                SELECT   beneficiario.nome                                          NomeBeneficiario
                                ,to_char(beneficiario.beneficiario)                                 CodigoBeneficiario
                                ,to_char(beneficiario.cod)                                       IdBeneficiario
		                        ,to_char(familia.familia)                                           Familia
                                ,to_char(familia.cod)                                            IdFamilia
                                ,beneficiario.dataadesao                                            DataAdesao
                                ,beneficiario.datacancelamento                                      DataCancelamento
                                ,beneficiario.databloqueio                                          DataBloqueio
                                ,matricula.datafalecimento                                          DataFalecimento
                                ,matricula.sexo                                                     Sexo
                                ,to_char(matricula.cpf)                                             CPF
                                ,matricula.rg                                                       RG
                                ,matricula.datanascimento                                           DataNascimento
                                ,TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) Idade
                                ,logradouros_tipo.descricao || ' ' || endereco.logradouro           Endereco
                                ,to_char(endereco.numero)                                           Nnumero
                                ,endereco.complemento                                               Complemento
                                ,endereco.bairro                                                    Bairro
                                ,municipios.nome                                                        Vidade
                                ,estados.nome                                                           Estado
                                ,estados.sigla                                                          UF
                                ,endereco.cep                                                       CEP
                                ,to_char(beneficiario.cartao)                                       Carteirinha
                                ,modulo.descricao                                                   Plano
                                ,COALESCE (endereco.telefone1,(select ender.telefone1 from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ),endereco.telefone1,null)             Telefone1
                                 ,COALESCE (endereco.telefone2,(select ender.telefone2 from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ),endereco.telefone2, null)            Telefone2     
                                 ,COALESCE (endereco.fax,(select ender.fax from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ),endereco.fax, null)                  Fax       
                                ,COALESCE (endereco_comercial.celular,
                                CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                     THEN
                                    (select ender.celular from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                     ELSE NULL END,endereco_comercial.celular, null)                    CelularComercial
                                ,COALESCE (endereco_comercial.fax,
                                 CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN
                                      (select ender.fax from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                      ELSE NULL END,endereco_comercial.fax, null)                       FaxComercial
                                ,COALESCE (endereco_comercial.telefone1,
                                 CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN
                                     (select ender.telefone1 from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                      ELSE NULL END,endereco_comercial.telefone1, null)                 TelefoneComercial1
                                ,COALESCE (endereco_comercial.telefone2,
                                 CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN
                                     (select ender.telefone2 from beneficiario ben
                                         inner join endereco ender on ben.enderecocomercial = ender.cod
                                         inner join familia fam on ben.familia = fam.cod
                                         where fam.cod = beneficiario.familia
                                         and ben.ehtitular = 'S' )
                                      ELSE NULL END,endereco_comercial.telefone2, null)                 TelefoneComercial2 
                                ,COALESCE (endereco.celular,(select ender.celular from beneficiario ben
                                     inner join endereco ender on ben.enderecoresidencial = ender.cod
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), endereco.celular, null)             Celular1
                                ,COALESCE (beneficiario.celular,(select ben.celular from beneficiario ben
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), beneficiario.celular, null)         Celular2
                                ,COALESCE (beneficiario.email,(select ben.email from beneficiario ben
                                     inner join familia fam on ben.familia = fam.cod
                                     where fam.cod = beneficiario.familia
                                     and ben.ehtitular = 'S' ), beneficiario.email, null)           Email
                                ,CASE WHEN modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                      THEN COALESCE (beneficiario.emailcorporativo,(select ben.emailcorporativo from beneficiario ben
                                             inner join familia fam on ben.familia = fam.cod
                                             where fam.cod = beneficiario.familia
                                             and ben.ehtitular = 'S' ), beneficiario.emailcorporativo, null) 
                                      ELSE NULL END                                                     EmailCorporativo
                                ,tipodependente.descricao                                           TipoDependente
                                ,CASE WHEN familia.titularresponsavel != beneficiario.cod
                                      THEN to_char(familia.titularresponsavel) ELSE NULL END        IdBeneficiarioTitular
                                ,matricula.nomepai                                                  NomePai 
                                ,matricula.nomemae                                                  NomeMae     
                                ,CASE  WHEN (   (beneficiario.databloqueio IS NOT NULL OR beneficiario.databloqueio <= :DT_FINAL))
                                            AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento >= :DT_FINAL)
		                        	   THEN 'BLOQUEADO'
		                        	   WHEN  (beneficiario_mod_historico.datacancelamento IS NOT NULL 
                                           AND beneficiario_mod_historico.datacancelamento <= :DT_FINAL)
		                        	   THEN 'CANCELADO' 
		                        	   WHEN (SELECT  MAX(1)
		                        			 FROM Empresa_PRD.beneficiario_suspensao
		                        			 WHERE :DT_FINAL  BETWEEN DATAINICIAL AND NVL(DATAFINAL, :DT_FINAL)
		                        			   AND (BENEFICIARIO = beneficiario.cod
		                        			    OR BENEFICIARIO = familia.titularresponsavel)) = 1
		                        	   THEN 'SUSPENSO'
		                        	   ELSE 'ATIVO' END                                                 StatusBeneficiario
                                FROM  Empresa_PRD.beneficiario
                                INNER JOIN Empresa_PRD.matricula
                                        ON beneficiario.matricula = matricula.cod
                                INNER JOIN Empresa_PRD.familia 
                                        ON beneficiario.familia =  familia.cod
                                LEFT  JOIN Empresa_PRD.endereco
                                        ON NVL(enderecoresidencial, 
                                            (SELECT enderecoresidencial 
                                               FROM Empresa_PRD.beneficiario 
                                              WHERE cod =  familia.titularresponsavel)) = endereco.cod
                                LEFT JOIN Empresa_PRD.logradouros_tipo
                                        ON endereco.tipologradouro = logradouros_tipo.cod
                                INNER JOIN Empresa_PRD.estados 
                                        ON endereco.estado = estados.cod
                                INNER JOIN Empresa_PRD.municipios
                                        ON endereco.municipio = municipios.cod
                                       AND estados.cod = municipios.estado
                                INNER JOIN Empresa_PRD.beneficiario_mod               
                                        ON beneficiario_mod.beneficiario = beneficiario.cod
                                INNER JOIN Empresa_PRD.beneficiario_mod_historico     
                                        ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                                INNER JOIN Empresa_PRD.contrato_mod                   
                                        ON contrato_mod.cod = beneficiario_mod.modulo
                                INNER JOIN Empresa_PRD.modulo                         
                                        ON modulo.cod = contrato_mod.modulo
                                LEFT  JOIN Empresa_PRD.endereco endereco_comercial
                                        ON NVL(enderecocomercial, 
                                            (SELECT enderecocomercial 
                                               FROM Empresa_PRD.beneficiario 
                                              WHERE cod =  familia.titularresponsavel)) = endereco_comercial.cod   
                                		      AND modulo.cod NOT IN (SELECT cod FROM modulo WHERE UPPER(descricao) LIKE '%APOSENT%')
                                INNER JOIN Empresa_PRD.CONTRATO_TPDEP
                                        ON BENEFICIARIO.TIPODEPENDENTE = CONTRATO_TPDEP.cod
                                INNER JOIN Empresa_PRD.TIPODEPENDENTE
                                        ON CONTRATO_TPDEP.TIPODEPENDENTE = TIPODEPENDENTE.cod
                                
                                WHERE beneficiario.contrato IN (1,3,5) 
                                  AND (beneficiario_mod_historico.datacancelamento IS NULL 
                                        OR beneficiario_mod_historico.datacancelamento > :DT_FINAL
                                      )
                                  AND (beneficiario.databloqueio IS NULL 
                                        OR beneficiario.databloqueio > :DT_FINAL
                                      )
                                  AND (beneficiario.datacancelamento IS NULL 
                                        OR beneficiario.datacancelamento > :DT_FINAL
                                      )
                                
                                  AND (TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) >= 18
                                       OR (
                                		    TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) < 18
                                			AND familia.titularresponsavel IN (SELECT beneficiario 
                                											 FROM beneficiario_anotadm 
                                											 WHERE anotacao = 485)
                                	       )
                                	   )
                                  AND (beneficiario.dataadesao < :DT_FINAL)
                                  AND matricula.datafalecimento IS NULL
                                  AND beneficiario.filialcusto IN (42)
                                  AND municipios.cod IN (9760, 9944, 9954, 9960, 9964)
                                  --AND estados.cod = 8
                                  AND tipodependente.cod != 9                        
                       ";

            using var connection = _ERPContext.Database.GetDbConnection();
            connection.Open();

            var result = connection.Query<BeneficiariosDTO>(sql, new { DT_FINAL = dtFinal })
                                 .ToList();

            foreach (var beneficiario in result)
            {
                beneficiario.NomeBeneficiario = beneficiario.NomeBeneficiario.ToUpper();
                beneficiario.DataAdesao = (beneficiario.DataAdesao == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataAdesao;
                beneficiario.DataCancelamento = (beneficiario.DataCancelamento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataCancelamento;
                beneficiario.DataBloqueio = (beneficiario.DataBloqueio == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataBloqueio;
                beneficiario.DataFalecimento = (beneficiario.DataFalecimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataFalecimento;
                beneficiario.DataNascimento = (beneficiario.DataNascimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataNascimento;
                // Outros tratamentos...
            }

            return result;
        }

        public IEnumerable<BloqueadosDTO> GetBloqueados(DateTime dtInicial, DateTime dtFinal)
        {
            var sql = @"
                        SELECT   matricula.cpf                                                      CPF
                        ,beneficiario.cod                                                IdBeneficiario
                        ,beneficiario.datacancelamento                                      DataCancelamento
                        ,beneficiario.databloqueio                                          DataBloqueio
                        ,matricula.datafalecimento                                          DataFalecimento
                        ,CASE  WHEN (   (beneficiario.databloqueio IS NOT NULL OR beneficiario.databloqueio <= :DT_FINAL))
                                    AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento >= :DT_FINAL)
                			   THEN 'BLOQUEADO'
                			   WHEN  (beneficiario_mod_historico.datacancelamento IS NOT NULL 
                                   AND beneficiario_mod_historico.datacancelamento <= :DT_FINAL)
                			   THEN 'CANCELADO' 
                			   WHEN (SELECT  MAX(1)
                					 FROM Empresa_PRD.beneficiario_suspensao
                					 WHERE :DT_FINAL  BETWEEN DATAINICIAL AND NVL(DATAFINAL, :DT_FINAL)
                					   AND (BENEFICIARIO = beneficiario.cod
                					    OR BENEFICIARIO = familia.titularresponsavel)) = 1
                			   THEN 'SUSPENSO'
                			   ELSE 'ATIVO' END                                                 StatusBeneficiario
                        FROM  Empresa_PRD.beneficiario
                        INNER JOIN Empresa_PRD.matricula
                                ON beneficiario.matricula = matricula.cod
                        INNER JOIN Empresa_PRD.familia 
                                ON beneficiario.familia =  familia.cod
                        LEFT  JOIN Empresa_PRD.endereco
                                ON NVL(enderecoresidencial, 
                                    (SELECT enderecoresidencial 
                                       FROM Empresa_PRD.beneficiario 
                                      WHERE cod =  familia.titularresponsavel)) = endereco.cod
                        LEFT  JOIN Empresa_PRD.logradouros_tipo
                                ON endereco.tipologradouro = logradouros_tipo.cod
                        INNER JOIN Empresa_PRD.estados
                                ON endereco.estado = estados.cod
                        INNER JOIN Empresa_PRD.municipios
                                ON endereco.municipio = municipios.cod
                               AND estados.cod = municipios.estado
                        INNER JOIN Empresa_PRD.beneficiario_mod               
                                ON beneficiario_mod.beneficiario = beneficiario.cod
                        INNER JOIN Empresa_PRD.beneficiario_mod_historico     
                                ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                        INNER JOIN Empresa_PRD.contrato_mod                   
                                ON contrato_mod.cod = beneficiario_mod.modulo
                        INNER JOIN Empresa_PRD.modulo                         
                                ON modulo.cod = contrato_mod.modulo
                        INNER JOIN Empresa_PRD.CONTRATO_TPDEP
                                ON BENEFICIARIO.TIPODEPENDENTE = CONTRATO_TPDEP.cod
                        INNER JOIN Empresa_PRD.TIPODEPENDENTE
                                ON CONTRATO_TPDEP.TIPODEPENDENTE = TIPODEPENDENTE.cod
                        LEFT JOIN Empresa_PRD.beneficiario_suspensao
                                ON beneficiario.cod = beneficiario_suspensao.beneficiario
                        WHERE beneficiario.contrato IN (1, 3, 5)                                                  
                          AND TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) >= 18
                          AND (((beneficiario.databloqueio >= :DT_INICIAL AND beneficiario.databloqueio < :DT_FINAL) 
                                 AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento > :DT_FINAL))
                              OR (:DT_FINAL BETWEEN beneficiario_suspensao.DATAINICIAL AND NVL(beneficiario_suspensao.DATAFINAL, :DT_FINAL) AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento < :DT_FINAL)))
                          AND (beneficiario.liminar = 'N' OR matricula.datafalecimento IS NULL)
                          AND beneficiario.filialcusto IN (42)
                          AND municipios.cod IN (9760, 9944, 9954, 9960, 9964)
                          --AND estados.cod = 8
                          AND tipodependente.cod != 9
                       ";

            using var connection = _ERPContext.Database.GetDbConnection();
            connection.Open();

            var result = connection.Query<BloqueadosDTO>(sql, new { DT_INICIAL = dtInicial, DT_FINAL = dtFinal })
                                 .ToList();

            foreach (var beneficiario in result)
            {
                beneficiario.DataCancelamento = (beneficiario.DataCancelamento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataCancelamento;
                beneficiario.DataBloqueio = (beneficiario.DataBloqueio == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataBloqueio;
                beneficiario.DataFalecimento = (beneficiario.DataFalecimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataFalecimento;
                // Outros tratamentos...
            }

            return result;
        }

        public IEnumerable<CanceladosDTO> GetCancelados(DateTime dtInicial, DateTime dtFinal)
        {
            var sql = @"
                        SELECT  matricula.cpf                                               CPF
                        ,beneficiario.cod                                                IdBeneficiario
                        ,beneficiario.datacancelamento                                      DataCancelamento
                        ,beneficiario.databloqueio                                          DataBloqueio
                        ,matricula.datafalecimento                                          DataFalecimento
                        ,MOTIVOCANCELAMENTO.cod                                          IdMotivoCancelamento
                        ,MOTIVOCANCELAMENTO.DESCRICAO                                       MotivoCancelamento          
                        ,CASE  WHEN (   (beneficiario.databloqueio IS NOT NULL OR beneficiario.databloqueio <= :DT_FINAL))
                                            AND (beneficiario.datacancelamento IS NULL OR beneficiario.datacancelamento >= :DT_FINAL)
                        			   THEN 'BLOQUEADO'
                        			   WHEN  (beneficiario_mod_historico.datacancelamento IS NOT NULL 
                                           AND beneficiario_mod_historico.datacancelamento <= :DT_FINAL)
                        			   THEN 'CANCELADO' 
                        			   WHEN (SELECT  MAX(1)
                        					 FROM Empresa_PRD.beneficiario_suspensao
                        					 WHERE :DT_FINAL  BETWEEN DATAINICIAL AND NVL(DATAFINAL, :DT_FINAL)
                        					   AND (BENEFICIARIO = beneficiario.cod
                        					    OR BENEFICIARIO = familia.titularresponsavel)) = 1
                        			   THEN 'SUSPENSO'
                        			   ELSE 'ATIVO' END                                                 StatusBeneficiario
                        FROM  Empresa_PRD.beneficiario
                        INNER JOIN Empresa_PRD.matricula
                                ON beneficiario.matricula = matricula.cod
                        INNER JOIN Empresa_PRD.familia 
                                ON beneficiario.familia =  familia.cod
                        LEFT  JOIN Empresa_PRD.endereco
                                ON NVL(enderecoresidencial, 
                                    (SELECT enderecoresidencial 
                                       FROM Empresa_PRD.beneficiario 
                                      WHERE cod =  familia.titularresponsavel)) = endereco.cod
                        LEFT  JOIN Empresa_PRD.logradouros_tipo
                                ON endereco.tipologradouro = logradouros_tipo.cod
                        INNER JOIN Empresa_PRD.estados
                                ON endereco.estado = estados.cod
                        INNER JOIN Empresa_PRD.municipios
                                ON endereco.municipio = municipios.cod
                               AND estados.cod = municipios.estado
                        INNER JOIN Empresa_PRD.beneficiario_mod               
                                ON beneficiario_mod.beneficiario = beneficiario.cod
                        INNER JOIN Empresa_PRD.beneficiario_mod_historico     
                                ON beneficiario_mod_historico.beneficiariomod = beneficiario_mod.cod
                        INNER JOIN Empresa_PRD.contrato_mod                   
                                ON contrato_mod.cod = beneficiario_mod.modulo
                        INNER JOIN Empresa_PRD.modulo                         
                                ON modulo.cod = contrato_mod.modulo
                        INNER JOIN Empresa_PRD.CONTRATO_TPDEP
                                ON BENEFICIARIO.TIPODEPENDENTE = CONTRATO_TPDEP.cod
                        INNER JOIN Empresa_PRD.TIPODEPENDENTE
                                ON CONTRATO_TPDEP.TIPODEPENDENTE = TIPODEPENDENTE.cod
                        LEFT  JOIN Empresa_PRD.MOTIVOCANCELAMENTO
                                ON beneficiario_mod_historico.motivocancelamento = MOTIVOCANCELAMENTO.cod
                        WHERE beneficiario.contrato IN (1, 3, 5) 
                          AND TO_NUMBER(TRUNC((Months_Between(:DT_FINAL, matricula.datanascimento)/12))) >= 18
                          AND (    (beneficiario.datacancelamento >= :DT_INICIAL AND beneficiario.datacancelamento < :DT_FINAL
                              AND  (beneficiario_mod_historico.datacancelamento >= :DT_INICIAL AND beneficiario_mod_historico.datacancelamento < :DT_FINAL)
                          AND (beneficiario.liminar = 'N'  OR (beneficiario.liminar = 'S' AND matricula.datafalecimento IS NOT NULL))))
                          AND beneficiario.filialcusto IN (42)
                          AND municipios.cod IN (9760, 9944, 9954, 9960, 9964)
                          --AND estados.cod = 8
                          AND tipodependente.cod != 9
                       ";

            using var connection = _ERPContext.Database.GetDbConnection();
            connection.Open();

            var result = connection.Query<CanceladosDTO>(sql, new { DT_INICIAL = dtInicial, DT_FINAL = dtFinal })
                                 .ToList();

            foreach (var beneficiario in result)
            {
                beneficiario.DataCancelamento = (beneficiario.DataCancelamento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataCancelamento;
                beneficiario.DataBloqueio = (beneficiario.DataBloqueio == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataBloqueio;
                beneficiario.DataFalecimento = (beneficiario.DataFalecimento == DateTime.MinValue) ? (DateTime?)null : beneficiario.DataFalecimento;
                // Outros tratamentos...
            }

            return result;
        }
    }
}
