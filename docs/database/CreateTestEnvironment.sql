/* =====================================================================
   ApiCampaignDash - Script de criacao de AMBIENTE DE TESTE completo
   =====================================================================
   Cria os 3 bancos usados pela API (GS300GP, GS300ERP, GS300BI), todas as
   tabelas necessarias (reconstruidas a partir do AppDbContext, das
   EntityTypeConfiguration e das queries SQL cruas dos repositorios) e
   popula com dados de exemplo suficientes para exercitar TODAS as rotas
   da API atualmente implementadas.

   - GS300GP  -> banco principal da aplicacao (Initial Catalog do
                 appsettings.json). Contem as tabelas tblCampanhaTelevendas*.
   - GS300ERP -> em producao contem VIEWS (uvwPessoaFisicaJuridica, vwProduto,
                 tblProduto, tblProdutoLinha, uvwClienteOutrasInformacoes).
                 Aqui sao recriadas como TABELAS apenas para possibilitar
                 testes locais.
   - GS300BI  -> em producao contem a tabela de consolidacao de vendas
                 (tblConsolidacaoVendas) usada no relatorio de sell-out.

   O script e idempotente: pode ser executado mais de uma vez sem duplicar
   estrutura nem dados (usa IF OBJECT_ID / IF NOT EXISTS em todo lugar).

   Ajuste o nome do servidor/instancia no seu client SQL antes de rodar.
   Requer permissao para CREATE DATABASE.
   ===================================================================== */

SET NOCOUNT ON;
GO

/* =====================================================================
   0) CRIACAO DOS BANCOS
   ===================================================================== */

IF DB_ID('GS300GP') IS NULL CREATE DATABASE GS300GP;
GO
IF DB_ID('GS300ERP') IS NULL CREATE DATABASE GS300ERP;
GO
IF DB_ID('GS300BI') IS NULL CREATE DATABASE GS300BI;
GO


/* =====================================================================
   1) GS300GP - TABELAS DO MODULO DE CAMPANHA
   ===================================================================== */

USE GS300GP;
GO

/* ---------- Tabelas de dominio / lookup ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendasTipoApuracao', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasTipoApuracao
(
    IDCampanhaTelevendasTipoApuracao INT NOT NULL PRIMARY KEY,
    DescricaoTipoApuracao            VARCHAR(200) NOT NULL
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasTipoCalculo', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasTipoCalculo
(
    IDCampanhaTelevendasTipoCalculo INT NOT NULL PRIMARY KEY,
    DescricaoTipoCalculo            VARCHAR(200) NOT NULL
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasPeriodoCompetenciaSituacao', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasPeriodoCompetenciaSituacao
(
    IDCampanhaTelevendasPeriodoCompetenciaSituacao INT NOT NULL PRIMARY KEY,
    DescricaoPeriodoCompetenciaSituacao            VARCHAR(200) NOT NULL
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasTipoPessoa', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasTipoPessoa
(
    IDCampanhaTelevendasTipoPessoa INT NOT NULL PRIMARY KEY,
    Descricao                      VARCHAR(200) NOT NULL
);
GO

/* ---------- Tabela principal da campanha ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendas', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendas
(
    IDCampanhaTelevendas                            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DataCompetencia                                  DATETIME NULL,
    IDCampanhaTelevendasPeriodoCompetenciaSituacao   INT NOT NULL,
    DataInicioApuracao                               DATETIME NULL,
    DataFimApuracao                                  DATETIME NULL,
    TotalRanking                                     INT NULL,
    DescricaoCampanha                                VARCHAR(150) NOT NULL,
    IDCampanhaTelevendasTipoApuracao                 INT NOT NULL,
    IDCampanhaTelevendasTipoCalculo                  INT NOT NULL,
    RegraValidacao                                   INT NULL,
    TipoValor                                        INT NOT NULL,
    DataFimAntecipada                                DATETIME NULL,
    Observacao                                       VARCHAR(150) NULL,
    ConsideraExclusivas                              INT NULL,
    TipoCampanhaTelevendas                           BIT NULL,
    Dinamica                                         BIT NULL,
    CONSTRAINT FK_tblCampanhaTelevendas_TipoApuracao
        FOREIGN KEY (IDCampanhaTelevendasTipoApuracao) REFERENCES dbo.tblCampanhaTelevendasTipoApuracao (IDCampanhaTelevendasTipoApuracao),
    CONSTRAINT FK_tblCampanhaTelevendas_TipoCalculo
        FOREIGN KEY (IDCampanhaTelevendasTipoCalculo) REFERENCES dbo.tblCampanhaTelevendasTipoCalculo (IDCampanhaTelevendasTipoCalculo),
    CONSTRAINT FK_tblCampanhaTelevendas_PeriodoCompetenciaSituacao
        FOREIGN KEY (IDCampanhaTelevendasPeriodoCompetenciaSituacao) REFERENCES dbo.tblCampanhaTelevendasPeriodoCompetenciaSituacao (IDCampanhaTelevendasPeriodoCompetenciaSituacao)
);
GO

/* ---------- Metas ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendasMeta', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasMeta
(
    IDCampanhaTelevendas INT NOT NULL PRIMARY KEY,
    MetaValor            DECIMAL(18,2) NOT NULL,
    GatilhoPositivacao   DECIMAL(18,2) NOT NULL,
    GatilhoVenda         DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_tblCampanhaTelevendasMeta_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasMetaInd', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasMetaInd
(
    IDCampanhaTelevendas INT NOT NULL,
    IDPessoa             INT NOT NULL,
    IDSupervisor         INT NOT NULL,
    TipoPessoa           INT NOT NULL,
    MetaValor            DECIMAL(18,2) NOT NULL,
    Gatilho              DECIMAL(18,2) NOT NULL,
    IDGerente            INT NOT NULL,
    CONSTRAINT PK_tblCampanhaTelevendasMetaInd PRIMARY KEY (IDCampanhaTelevendas, IDPessoa),
    CONSTRAINT FK_tblCampanhaTelevendasMetaInd_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

/* ---------- Faixa de premiacao ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendasFaixaPremiacao', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasFaixaPremiacao
(
    IDCampanhaTelevendas INT NOT NULL,
    ValorInicio          DECIMAL(18,2) NOT NULL,
    ValorFim             DECIMAL(18,2) NOT NULL,
    RankingInicio        INT NOT NULL,
    RankingFim           INT NOT NULL,
    PremioValor          DECIMAL(18,2) NOT NULL,
    LimitePremio         DECIMAL(18,2) NOT NULL,
    TipoPessoa           INT NOT NULL,
    CONSTRAINT PK_tblCampanhaTelevendasFaixaPremiacao PRIMARY KEY (IDCampanhaTelevendas, TipoPessoa, RankingInicio),
    CONSTRAINT FK_tblCampanhaTelevendasFaixaPremiacao_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

/* ---------- Resultado apurado ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendasResultado', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasResultado
(
    IDCampanhaTelevendas          INT NOT NULL,
    DataCompetencia               DATETIME NOT NULL,
    DescricaoCampanha             VARCHAR(200) NOT NULL,
    IDPessoaTelevendas            INT NOT NULL,
    NomeDigitador                 VARCHAR(200) NOT NULL,
    Ranking                       INT NOT NULL,
    ValorApurado                  DECIMAL(18,2) NOT NULL,
    Premiacao                     DECIMAL(18,2) NOT NULL,
    LogCalculo                    VARCHAR(MAX) NULL,
    DataCalculo                   DATETIME NOT NULL,
    TipoPessoa                    INT NOT NULL,
    IDSupervisor                  INT NOT NULL,
    NomeSupervisor                VARCHAR(200) NOT NULL,
    RealizadoTotal                DECIMAL(18,2) NOT NULL,
    ObjetivoIndividual            DECIMAL(18,2) NOT NULL,
    PercentualRealizadoIndividual DECIMAL(18,2) NOT NULL,
    ValorApuradoBees              DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_tblCampanhaTelevendasResultado PRIMARY KEY (IDCampanhaTelevendas, IDPessoaTelevendas),
    CONSTRAINT FK_tblCampanhaTelevendasResultado_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

/* ---------- Combo / Produto / Fabricante / Linha / Cliente / Digitadora (regras da campanha) ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendasCombo', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasCombo
(
    IDCampanhaTelevendas INT NOT NULL,
    IDCombo              INT NOT NULL,
    Contem               VARCHAR(10) NULL,
    CONSTRAINT PK_tblCampanhaTelevendasCombo PRIMARY KEY (IDCampanhaTelevendas, IDCombo),
    CONSTRAINT FK_tblCampanhaTelevendasCombo_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasProduto', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasProduto
(
    IDCampanhaTelevendas INT NOT NULL,
    IDProduto            INT NOT NULL,
    Contem               VARCHAR(10) NULL,
    CONSTRAINT PK_tblCampanhaTelevendasProduto PRIMARY KEY (IDCampanhaTelevendas, IDProduto),
    CONSTRAINT FK_tblCampanhaTelevendasProduto_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasFabricante', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasFabricante
(
    IDCampanhaTelevendas INT NOT NULL,
    IDFabricante         INT NOT NULL,
    Contem               VARCHAR(10) NULL,
    CONSTRAINT PK_tblCampanhaTelevendasFabricante PRIMARY KEY (IDCampanhaTelevendas, IDFabricante),
    CONSTRAINT FK_tblCampanhaTelevendasFabricante_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasLinhaProduto', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasLinhaProduto
(
    IDCampanhaTelevendas INT NOT NULL,
    IDLinhaProduto       INT NOT NULL,
    Contem               VARCHAR(10) NULL,
    CONSTRAINT PK_tblCampanhaTelevendasLinhaProduto PRIMARY KEY (IDCampanhaTelevendas, IDLinhaProduto),
    CONSTRAINT FK_tblCampanhaTelevendasLinhaProduto_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasCliente', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasCliente
(
    IDCampanhaTelevendas INT NOT NULL,
    IDCliente            INT NOT NULL,
    Contem               VARCHAR(10) NULL,
    CONSTRAINT PK_tblCampanhaTelevendasCliente PRIMARY KEY (IDCampanhaTelevendas, IDCliente),
    CONSTRAINT FK_tblCampanhaTelevendasCliente_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasDigitadora', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasDigitadora
(
    IDCampanhaTelevendas INT NOT NULL,
    IDPessoaTelevendas   INT NOT NULL,
    Contem               VARCHAR(10) NULL,
    CONSTRAINT PK_tblCampanhaTelevendasDigitadora PRIMARY KEY (IDCampanhaTelevendas, IDPessoaTelevendas),
    CONSTRAINT FK_tblCampanhaTelevendasDigitadora_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

IF OBJECT_ID('dbo.tblCampanhaTelevendasRegraValidacao', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasRegraValidacao
(
    IDCampanhaTelevendas          INT NOT NULL,
    IDCampanhaTelevendasValidar   INT NOT NULL,
    ResultadoValidacao            VARCHAR(50) NULL,
    CONSTRAINT PK_tblCampanhaTelevendasRegraValidacao PRIMARY KEY (IDCampanhaTelevendas, IDCampanhaTelevendasValidar),
    CONSTRAINT FK_tblCampanhaTelevendasRegraValidacao_Campanha
        FOREIGN KEY (IDCampanhaTelevendas) REFERENCES dbo.tblCampanhaTelevendas (IDCampanhaTelevendas)
);
GO

/* ---------- Base de digitadoras / supervisores ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendasBaseDigitadoras', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasBaseDigitadoras
(
    IDSupervisor       INT NOT NULL,
    NomeSupervisor     VARCHAR(200) NOT NULL,
    IDPessoaTelevendas INT NOT NULL,
    NomeDigitador      VARCHAR(200) NOT NULL,
    UsuarioDigitador   VARCHAR(100) NOT NULL,
    IDSetor            INT NOT NULL,
    CONSTRAINT PK_tblCampanhaTelevendasBaseDigitadoras PRIMARY KEY (IDSupervisor, IDPessoaTelevendas)
);
GO

/* ---------- Distincao pessoa x fabricante ---------- */

IF OBJECT_ID('dbo.tblCampanhaTelevendasDistincaoPessoaFabricante', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaTelevendasDistincaoPessoaFabricante
(
    IDFabricante         INT NOT NULL,
    DescricaoFabricante  VARCHAR(200) NOT NULL,
    IDPessoa             INT NOT NULL,
    IDDigitador          INT NOT NULL,
    CONSTRAINT PK_tblCampanhaTelevendasDistincaoPessoaFabricante PRIMARY KEY (IDFabricante, IDPessoa)
);
GO

/* ---------- Campanha comercial (legado, sem controller ativo hoje) ---------- */

IF OBJECT_ID('dbo.tblCampanhaComercial', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaComercial
(
    IDCampanhaComercial INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NomeCampanha        VARCHAR(100) NOT NULL
);
GO

/* ---------- Tabelas de comissao/vendas Bees usadas pelo relatorio de Sell-Out ---------- */

IF OBJECT_ID('dbo.tblVendasBees', 'U') IS NULL
CREATE TABLE dbo.tblVendasBees
(
    IDPedido    INT NULL,
    IDOperacao  INT NULL,
    IDDigitador UNIQUEIDENTIFIER NULL
);
GO

IF OBJECT_ID('dbo.tblComissaoVendasClienteVendedor', 'U') IS NULL
CREATE TABLE dbo.tblComissaoVendasClienteVendedor
(
    IDCliente               INT NULL,
    IDGerente               INT NULL,
    IDComissaoVendasCenario INT NULL
);
GO

/* Stub da function usada em CampaignResumeSellOutRepository - so para viabilizar teste local */
IF OBJECT_ID('dbo.fnObterIDComissaoVendasCenario', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fnObterIDComissaoVendasCenario;
GO
CREATE FUNCTION dbo.fnObterIDComissaoVendasCenario (@DataReferencia DATE)
RETURNS INT
AS
BEGIN
    RETURN 1;
END
GO


/* =====================================================================
   2) GS300ERP - TABELAS MESTRE (recriadas como TABELA; em producao sao VIEWS)
   ===================================================================== */

USE GS300ERP;
GO

IF OBJECT_ID('dbo.uvwPessoaFisicaJuridica', 'U') IS NULL
CREATE TABLE dbo.uvwPessoaFisicaJuridica
(
    IDPessoa    INT NOT NULL PRIMARY KEY,
    CpfCnpj     VARCHAR(20) NULL,
    RazaoSocial VARCHAR(200) NULL,
    Nome        VARCHAR(200) NULL
);
GO

IF OBJECT_ID('dbo.vwProduto', 'U') IS NULL
CREATE TABLE dbo.vwProduto
(
    IDProduto   INT NOT NULL PRIMARY KEY,
    DescProduto VARCHAR(200) NULL,
    CodBarras   VARCHAR(20) NULL
);
GO

IF OBJECT_ID('dbo.tblProduto', 'U') IS NULL
CREATE TABLE dbo.tblProduto
(
    IDProduto   INT NOT NULL PRIMARY KEY,
    DescProduto VARCHAR(200) NULL
);
GO

IF OBJECT_ID('dbo.tblProdutoLinha', 'U') IS NULL
CREATE TABLE dbo.tblProdutoLinha
(
    IDProdutoLinha    INT NOT NULL PRIMARY KEY,
    DescProdutoLinha  VARCHAR(200) NULL
);
GO

IF OBJECT_ID('dbo.uvwClienteOutrasInformacoes', 'U') IS NULL
CREATE TABLE dbo.uvwClienteOutrasInformacoes
(
    IDCliente   INT NOT NULL PRIMARY KEY,
    NomeCliente VARCHAR(200) NULL,
    CPF_CNPJ    VARCHAR(20) NULL,
    DescCidade  VARCHAR(100) NULL,
    UFCidade    VARCHAR(2) NULL
);
GO

IF OBJECT_ID('dbo.tblSegUsuario', 'U') IS NULL
CREATE TABLE dbo.tblSegUsuario
(
    IDUsuario   UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    NomeUsuario VARCHAR(200) NULL
);
GO


/* =====================================================================
   3) GS300BI - CONSOLIDACAO DE VENDAS (usada no relatorio de Sell-Out)
   ===================================================================== */

USE GS300BI;
GO

IF OBJECT_ID('dbo.tblConsolidacaoVendas', 'U') IS NULL
CREATE TABLE dbo.tblConsolidacaoVendas
(
    IDPedido            INT NULL,
    IDOperacao          INT NULL,
    IDProduto           INT NULL,
    IDProdutoLinha      INT NULL,
    IDPessoaCliente     INT NULL,
    IDPessoaVendedor    INT NULL,
    IDPessoaFabricante  INT NULL,
    IDPessoaGerente     INT NULL,
    IDDigitador         UNIQUEIDENTIFIER NULL,
    IDOrigem            INT NULL,
    DataFaturamento     DATETIME NULL,
    Quantidade          INT NULL,
    Total               DECIMAL(18,2) NULL
);
GO

/* Upgrade idempotente: quem ja rodou a versao anterior deste script nao tinha esta
   coluna, usada direto por SellOutSummaryRepository (tblConVen.IDPessoaGerente <> 159452). */
IF COL_LENGTH('dbo.tblConsolidacaoVendas', 'IDPessoaGerente') IS NULL
    ALTER TABLE dbo.tblConsolidacaoVendas ADD IDPessoaGerente INT NULL;
GO


/* =====================================================================
   4) MASSA DE DADOS DE TESTE
   =====================================================================
   IDs usados (para facilitar os testes manuais na API):
     Fabricante ......... 501
     Linha de Produto .... 601
     Produtos ............ 701, 702
     Clientes ............ 801, 802
     Supervisor .......... 901
     Pessoa Televendas ... 1001
     Campanhas ........... 1 (aberta/positivacao), 2 (fechada/vendas), 3 (cancelada)
   ===================================================================== */

USE GS300ERP;
GO

/* ---------- Fabricante / Cliente (pessoa fisica ou juridica) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.uvwPessoaFisicaJuridica WHERE IDPessoa = 501)
INSERT INTO dbo.uvwPessoaFisicaJuridica (IDPessoa, CpfCnpj, RazaoSocial, Nome) VALUES
(501, '11222333000144', 'Fabricante Teste LTDA', 'Fabricante Teste LTDA');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.uvwPessoaFisicaJuridica WHERE IDPessoa = 801)
INSERT INTO dbo.uvwPessoaFisicaJuridica (IDPessoa, CpfCnpj, RazaoSocial, Nome) VALUES
(801, '11122233344', 'Farmacia Teste Cliente 1 LTDA', 'Farmacia Teste Cliente 1'),
(802, '55566677788', 'Farmacia Teste Cliente 2 LTDA', 'Farmacia Teste Cliente 2');
GO

/* ---------- Produto (view legada usada no relatorio de Sell-Out) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.vwProduto WHERE IDProduto = 701)
INSERT INTO dbo.vwProduto (IDProduto, DescProduto, CodBarras) VALUES
(701, 'Produto Teste 700ML', '7891234567901'),
(702, 'Produto Teste Sache 20UN', '7891234567902');
GO

/* ---------- Produto (tabela mestre usada na rota /Campaign/Params/Detail/produto) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblProduto WHERE IDProduto = 701)
INSERT INTO dbo.tblProduto (IDProduto, DescProduto) VALUES
(701, 'Produto Teste 700ML'),
(702, 'Produto Teste Sache 20UN');
GO

/* ---------- Linha de Produto (rota /Campaign/Params/Detail/linha) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblProdutoLinha WHERE IDProdutoLinha = 601)
INSERT INTO dbo.tblProdutoLinha (IDProdutoLinha, DescProdutoLinha) VALUES
(601, 'Linha Higiene e Beleza');
GO

/* ---------- Cliente com dados completos (rota /Campaign/Params/Detail/cliente) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.uvwClienteOutrasInformacoes WHERE IDCliente = 801)
INSERT INTO dbo.uvwClienteOutrasInformacoes (IDCliente, NomeCliente, CPF_CNPJ, DescCidade, UFCidade) VALUES
(801, 'Farmacia Teste Cliente 1', '11122233344', 'Sao Paulo', 'SP'),
(802, 'Farmacia Teste Cliente 2', '55566677788', 'Campinas', 'SP');
GO

/* ---------- Usuarios/vendedores (Sell-Out: nome do vendedor) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblSegUsuario WHERE IDUsuario = 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA')
INSERT INTO dbo.tblSegUsuario (IDUsuario, NomeUsuario) VALUES
('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 'Vendedor Teste Convencional'),
('BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB', 'Vendedor Teste Bees');
GO


/* =====================================================================
   4-B) MASSA DE DADOS ADICIONAL (mais rica) - cobre os relatorios novos
   (DynamicReportController, SellOutSummaryController) que precisam de
   varios fabricantes/linhas/produtos/clientes/vendedores/meses para que
   agrupamentos, filtros e as regras de exclusao fixas no codigo
   (IDGerente <> 159452 e IDDigitador Bees <> 'ce782385-...') facam sentido.
   Nao mexe nos dados minimos criados acima (IDs 501/801/802/601/701/702/
   901/1001/1..3), apenas complementa com IDs novos.
   ===================================================================== */

/* ---------- Mais fabricantes (Pessoa Juridica) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.uvwPessoaFisicaJuridica WHERE IDPessoa = 502)
INSERT INTO dbo.uvwPessoaFisicaJuridica (IDPessoa, CpfCnpj, RazaoSocial, Nome) VALUES
(502, '22333444000155', 'Fabricante Beta Higiene LTDA', 'Fabricante Beta Higiene LTDA'),
(503, '33444555000166', 'Fabricante Gama Alimentos LTDA', 'Fabricante Gama Alimentos LTDA'),
(504, '44555666000177', 'Fabricante Delta Farma LTDA', 'Fabricante Delta Farma LTDA');
GO

/* ---------- Gerentes (Pessoa) - inclui o ID 159452, excluido por regra fixa no codigo
   (ver "DANIEL CAMPOS PRESTE" em CampaignResumeSellOutRepository, DynamicReportRepository
   e SellOutSummaryRepository) para permitir testar que ele SOME dos relatorios. ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.uvwPessoaFisicaJuridica WHERE IDPessoa = 201)
INSERT INTO dbo.uvwPessoaFisicaJuridica (IDPessoa, CpfCnpj, RazaoSocial, Nome) VALUES
(201, '11111111111', NULL, 'Gerente Regional Sul'),
(202, '22222222222', NULL, 'Gerente Regional Sudeste'),
(159452, '33333333333', NULL, 'Daniel Campos Preste');
GO

/* ---------- Mais clientes (Pessoa) - 803 a 815 (13 clientes adicionais) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.uvwPessoaFisicaJuridica WHERE IDPessoa = 803)
INSERT INTO dbo.uvwPessoaFisicaJuridica (IDPessoa, CpfCnpj, RazaoSocial, Nome)
SELECT
    IDCliente,
    RIGHT('00000000000' + CAST(IDCliente AS VARCHAR(11)), 11),
    'Cliente Teste ' + CAST(IDCliente - 800 AS VARCHAR(3)) + ' LTDA',
    'Cliente Teste ' + CAST(IDCliente - 800 AS VARCHAR(3))
FROM (SELECT 803 + n AS IDCliente FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS v(n)) x;
GO

/* ---------- Mais linhas de produto ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblProdutoLinha WHERE IDProdutoLinha = 602)
INSERT INTO dbo.tblProdutoLinha (IDProdutoLinha, DescProdutoLinha) VALUES
(602, 'Linha Alimentos'),
(603, 'Linha Farma'),
(604, 'Linha Limpeza'),
(605, 'Linha Bebidas');
GO

/* ---------- Mais produtos (703 a 715), distribuidos entre as 5 linhas ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.vwProduto WHERE IDProduto = 703)
INSERT INTO dbo.vwProduto (IDProduto, DescProduto, CodBarras)
SELECT
    IDProduto,
    'Produto Teste ' + CAST(IDProduto - 700 AS VARCHAR(3)),
    '789123456' + RIGHT('0000' + CAST(IDProduto AS VARCHAR(4)), 4)
FROM (SELECT 703 + n AS IDProduto FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS v(n)) x;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblProduto WHERE IDProduto = 703)
INSERT INTO dbo.tblProduto (IDProduto, DescProduto)
SELECT IDProduto, DescProduto FROM dbo.vwProduto WHERE IDProduto BETWEEN 703 AND 715;
GO

/* ---------- Clientes com dados completos (703 a 715 nao se aplica aqui; 803 a 815) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.uvwClienteOutrasInformacoes WHERE IDCliente = 803)
INSERT INTO dbo.uvwClienteOutrasInformacoes (IDCliente, NomeCliente, CPF_CNPJ, DescCidade, UFCidade)
SELECT
    IDCliente,
    'Cliente Teste ' + CAST(IDCliente - 800 AS VARCHAR(3)),
    RIGHT('00000000000' + CAST(IDCliente AS VARCHAR(11)), 11),
    cidade.DescCidade,
    cidade.UFCidade
FROM (SELECT 803 + n AS IDCliente, n FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS v(n)) x
CROSS APPLY (
    SELECT * FROM (VALUES
        (0, 'Sao Paulo', 'SP'), (1, 'Rio de Janeiro', 'RJ'), (2, 'Belo Horizonte', 'MG'),
        (3, 'Curitiba', 'PR'), (4, 'Porto Alegre', 'RS')
    ) AS c(Seq, DescCidade, UFCidade)
    WHERE c.Seq = x.n % 5
) cidade;
GO

/* ---------- Mais vendedores/operadores (Sell-Out e Dynamic Report) ----------
   1002 a 1006: canal Televendas (precisam existir tambem em
   tblCampanhaTelevendasBaseDigitadoras.UsuarioDigitador, inserido mais abaixo).
   Canal Bees: mais um vendedor "normal" e a GUID FIXA excluida no codigo
   ('ce782385-bece-485b-9e33-05ec60591610' = "BRENDA EXCLUSIVA GRANDES CONTAS"),
   incluida de proposito para permitir testar que ela SOME dos relatorios
   quando ConsideraGrandesContas = false. ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblSegUsuario WHERE IDUsuario = '11111111-1111-1111-1111-111111111002')
INSERT INTO dbo.tblSegUsuario (IDUsuario, NomeUsuario) VALUES
('11111111-1111-1111-1111-111111111002', 'Operador Televendas 2'),
('11111111-1111-1111-1111-111111111003', 'Operador Televendas 3'),
('11111111-1111-1111-1111-111111111004', 'Operador Televendas 4'),
('11111111-1111-1111-1111-111111111005', 'Operador Televendas 5'),
('11111111-1111-1111-1111-111111111006', 'Operador Televendas 6'),
('22222222-2222-2222-2222-222222222002', 'Vendedor Bees Convencional 2'),
('ce782385-bece-485b-9e33-05ec60591610', 'Brenda Exclusiva Grandes Contas');
GO


USE GS300GP;
GO

/* ---------- Lookups ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasTipoApuracao WHERE IDCampanhaTelevendasTipoApuracao = 1)
INSERT INTO dbo.tblCampanhaTelevendasTipoApuracao (IDCampanhaTelevendasTipoApuracao, DescricaoTipoApuracao) VALUES
(1, 'POSITIVACAO'),
(2, 'VENDAS'),
(3, 'QUANTIDADE VENDIDA'),
(6, 'POSITIVACAO ESPECIFICA');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasTipoCalculo WHERE IDCampanhaTelevendasTipoCalculo = 1)
INSERT INTO dbo.tblCampanhaTelevendasTipoCalculo (IDCampanhaTelevendasTipoCalculo, DescricaoTipoCalculo) VALUES
(1, 'Calculo Padrao'),
(2, 'Calculo Alternativo');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasPeriodoCompetenciaSituacao WHERE IDCampanhaTelevendasPeriodoCompetenciaSituacao = 1)
INSERT INTO dbo.tblCampanhaTelevendasPeriodoCompetenciaSituacao (IDCampanhaTelevendasPeriodoCompetenciaSituacao, DescricaoPeriodoCompetenciaSituacao) VALUES
(1, 'ABERTO'),
(2, 'FECHADO'),
(3, 'EM APURACAO'),
(4, 'CANCELADO');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasTipoPessoa WHERE IDCampanhaTelevendasTipoPessoa = 1)
INSERT INTO dbo.tblCampanhaTelevendasTipoPessoa (IDCampanhaTelevendasTipoPessoa, Descricao) VALUES
(1, 'SUPERVISOR'),
(2, 'DIGITADOR'),
(3, 'GERENTE');
GO

/* ---------- Campanhas de teste ----------
   Campanha 1: aberta, positivacao, periodo = mes atual.
   Campanha 2: fechada, vendas, periodo = mes anterior.
   Campanha 3: cancelada (deve ser IGNORADA pelo /campaign-summary e pelo relatorio de resultado). */
DECLARE @Hoje       DATE = CAST(GETDATE() AS DATE);
DECLARE @InicioMes  DATE = DATEFROMPARTS(YEAR(@Hoje), MONTH(@Hoje), 1);
DECLARE @FimMes     DATE = EOMONTH(@Hoje);
DECLARE @InicioMesAnterior DATE = DATEADD(MONTH, -1, @InicioMes);
DECLARE @FimMesAnterior    DATE = EOMONTH(@InicioMesAnterior);

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendas WHERE IDCampanhaTelevendas = 1)
BEGIN
    SET IDENTITY_INSERT dbo.tblCampanhaTelevendas ON;

    INSERT INTO dbo.tblCampanhaTelevendas
        (IDCampanhaTelevendas, DataCompetencia, IDCampanhaTelevendasPeriodoCompetenciaSituacao,
         DataInicioApuracao, DataFimApuracao, TotalRanking, DescricaoCampanha,
         IDCampanhaTelevendasTipoApuracao, IDCampanhaTelevendasTipoCalculo, RegraValidacao,
         TipoValor, DataFimAntecipada, Observacao, ConsideraExclusivas, TipoCampanhaTelevendas, Dinamica)
    VALUES
        (1, @InicioMes, 1,
         @InicioMes, @FimMes, 10, 'Campanha Teste Positivacao',
         1, 1, NULL,
         1, NULL, 'Campanha de teste - aberta', 0, 0, 0),

        (2, @InicioMesAnterior, 2,
         @InicioMesAnterior, @FimMesAnterior, 10, 'Campanha Teste Vendas',
         2, 1, NULL,
         1, NULL, 'Campanha de teste - fechada', 0, 0, 0),

        (3, @InicioMesAnterior, 4,
         @InicioMesAnterior, @FimMesAnterior, 10, 'Campanha Teste Cancelada',
         1, 1, NULL,
         1, NULL, 'Campanha de teste - cancelada (deve ser ignorada no resumo)', 0, 0, 0);

    SET IDENTITY_INSERT dbo.tblCampanhaTelevendas OFF;
END
GO

/* ---------- Meta ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasMeta WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasMeta (IDCampanhaTelevendas, MetaValor, GatilhoPositivacao, GatilhoVenda) VALUES
(1, 10000.00, 5000.00, 5000.00),
(2, 20000.00, 8000.00, 8000.00);
GO

/* ---------- Faixa de premiacao (TipoPessoa: 1 = Supervisor) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasFaixaPremiacao WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasFaixaPremiacao
    (IDCampanhaTelevendas, ValorInicio, ValorFim, RankingInicio, RankingFim, PremioValor, LimitePremio, TipoPessoa) VALUES
(1, 0.00, 5000.00,  1, 3,  300.00, 1000.00, 1),
(1, 5000.01, 999999.00, 4, 10, 500.00, 1500.00, 1),
(2, 0.00, 999999.00, 1, 10, 400.00, 1200.00, 1);
GO

/* ---------- Resultado apurado (supervisor + operador) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasResultado WHERE IDCampanhaTelevendas = 1 AND IDPessoaTelevendas = 901)
INSERT INTO dbo.tblCampanhaTelevendasResultado
    (IDCampanhaTelevendas, DataCompetencia, DescricaoCampanha, IDPessoaTelevendas, NomeDigitador, Ranking,
     ValorApurado, Premiacao, LogCalculo, DataCalculo, TipoPessoa, IDSupervisor, NomeSupervisor,
     RealizadoTotal, ObjetivoIndividual, PercentualRealizadoIndividual, ValorApuradoBees)
SELECT
    1, DataCompetencia, 'Campanha Teste Positivacao', 901, 'Supervisor Teste', 1,
    15000.00, 500.00, 'Calculo de teste - supervisor', GETDATE(), 1, 901, 'Supervisor Teste',
    15000.00, 10000.00, 150.00, 2000.00
FROM dbo.tblCampanhaTelevendas WHERE IDCampanhaTelevendas = 1;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasResultado WHERE IDCampanhaTelevendas = 1 AND IDPessoaTelevendas = 1001)
INSERT INTO dbo.tblCampanhaTelevendasResultado
    (IDCampanhaTelevendas, DataCompetencia, DescricaoCampanha, IDPessoaTelevendas, NomeDigitador, Ranking,
     ValorApurado, Premiacao, LogCalculo, DataCalculo, TipoPessoa, IDSupervisor, NomeSupervisor,
     RealizadoTotal, ObjetivoIndividual, PercentualRealizadoIndividual, ValorApuradoBees)
SELECT
    1, DataCompetencia, 'Campanha Teste Positivacao', 1001, 'Digitador Teste', 1,
    8000.00, 300.00, 'Calculo de teste - operador', GETDATE(), 2, 901, 'Supervisor Teste',
    8000.00, 5000.00, 160.00, 1000.00
FROM dbo.tblCampanhaTelevendas WHERE IDCampanhaTelevendas = 1;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasResultado WHERE IDCampanhaTelevendas = 2 AND IDPessoaTelevendas = 901)
INSERT INTO dbo.tblCampanhaTelevendasResultado
    (IDCampanhaTelevendas, DataCompetencia, DescricaoCampanha, IDPessoaTelevendas, NomeDigitador, Ranking,
     ValorApurado, Premiacao, LogCalculo, DataCalculo, TipoPessoa, IDSupervisor, NomeSupervisor,
     RealizadoTotal, ObjetivoIndividual, PercentualRealizadoIndividual, ValorApuradoBees)
SELECT
    2, DataCompetencia, 'Campanha Teste Vendas', 901, 'Supervisor Teste', 1,
    22000.00, 400.00, 'Calculo de teste - campanha fechada', GETDATE(), 1, 901, 'Supervisor Teste',
    22000.00, 20000.00, 110.00, 3000.00
FROM dbo.tblCampanhaTelevendas WHERE IDCampanhaTelevendas = 2;
GO

/* ---------- Parametros da campanha (Produto / Linha / Fabricante / Cliente) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasProduto WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasProduto (IDCampanhaTelevendas, IDProduto, Contem) VALUES
(1, 701, 'S'), (1, 702, 'S'), (2, 701, 'S');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasLinhaProduto WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasLinhaProduto (IDCampanhaTelevendas, IDLinhaProduto, Contem) VALUES
(1, 601, 'S'), (2, 601, 'S');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasFabricante WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasFabricante (IDCampanhaTelevendas, IDFabricante, Contem) VALUES
(1, 501, 'S'), (2, 501, 'S');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasCliente WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasCliente (IDCampanhaTelevendas, IDCliente, Contem) VALUES
(1, 801, 'S'), (1, 802, 'S'), (2, 801, 'S');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasDigitadora WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasDigitadora (IDCampanhaTelevendas, IDPessoaTelevendas, Contem) VALUES
(1, 1001, 'S');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasCombo WHERE IDCampanhaTelevendas = 1)
INSERT INTO dbo.tblCampanhaTelevendasCombo (IDCampanhaTelevendas, IDCombo, Contem) VALUES
(1, 1101, 'S');
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasDistincaoPessoaFabricante WHERE IDFabricante = 501 AND IDPessoa = 801)
INSERT INTO dbo.tblCampanhaTelevendasDistincaoPessoaFabricante (IDFabricante, DescricaoFabricante, IDPessoa, IDDigitador) VALUES
(501, 'Fabricante Teste LTDA', 801, 1001);
GO

/* ---------- Base de digitadoras (liga o vendedor "convencional" ao GUID usado no Sell-Out) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasBaseDigitadoras WHERE IDSupervisor = 901 AND IDPessoaTelevendas = 1001)
INSERT INTO dbo.tblCampanhaTelevendasBaseDigitadoras
    (IDSupervisor, NomeSupervisor, IDPessoaTelevendas, NomeDigitador, UsuarioDigitador, IDSetor) VALUES
(901, 'Supervisor Teste', 1001, 'Digitador Teste', 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1);
GO

/* ---------- Campanha comercial (legado, tabela sem controller ativo hoje) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaComercial WHERE IDCampanhaComercial = 1)
BEGIN
    SET IDENTITY_INSERT dbo.tblCampanhaComercial ON;
    INSERT INTO dbo.tblCampanhaComercial (IDCampanhaComercial, NomeCampanha) VALUES (1, 'Campanha Comercial Teste');
    SET IDENTITY_INSERT dbo.tblCampanhaComercial OFF;
END
GO

/* ---------- Comissao por cliente/vendedor (exigida pelo JOIN + WHERE do relatorio de Sell-Out) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblComissaoVendasClienteVendedor WHERE IDCliente = 801)
INSERT INTO dbo.tblComissaoVendasClienteVendedor (IDCliente, IDGerente, IDComissaoVendasCenario) VALUES
(801, 1, 1),
(802, 1, 1);
GO

/* ---------- Venda via Bees (segunda metade do UNION do relatorio de Sell-Out) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblVendasBees WHERE IDPedido = 9001 AND IDOperacao = 1)
INSERT INTO dbo.tblVendasBees (IDPedido, IDOperacao, IDDigitador) VALUES
(9001, 1, 'BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB');
GO


/* ---------- Mais supervisores/operadores (902/903), completando a base de digitadoras ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasBaseDigitadoras WHERE IDSupervisor = 901 AND IDPessoaTelevendas = 1002)
INSERT INTO dbo.tblCampanhaTelevendasBaseDigitadoras
    (IDSupervisor, NomeSupervisor, IDPessoaTelevendas, NomeDigitador, UsuarioDigitador, IDSetor) VALUES
(901, 'Supervisor Teste',    1002, 'Operador Televendas 2', '11111111-1111-1111-1111-111111111002', 1),
(902, 'Supervisor Sudeste',  1003, 'Operador Televendas 3', '11111111-1111-1111-1111-111111111003', 2),
(902, 'Supervisor Sudeste',  1004, 'Operador Televendas 4', '11111111-1111-1111-1111-111111111004', 2),
(903, 'Supervisor Nordeste', 1005, 'Operador Televendas 5', '11111111-1111-1111-1111-111111111005', 3),
(903, 'Supervisor Nordeste', 1006, 'Operador Televendas 6', '11111111-1111-1111-1111-111111111006', 3);
GO

/* ---------- Campanhas 4 (cancelada, tipo Positivacao Especifica) e 5 (fechada, ha 2 meses) ----------
   Campanha 4 existe so para confirmar que campanhas canceladas (situacao=4) continuam
   fora do /campaign-summary e do /campaign-result-report, igual a campanha 3.
   Campanha 5 tem 3 supervisores x 2 operadores com ValorApurado, para dar volume e
   ranking real ao teste de faixa de premiacao. ---------- */
DECLARE @HojeB DATE = CAST(GETDATE() AS DATE);
DECLARE @InicioMesAtualB DATE = DATEFROMPARTS(YEAR(@HojeB), MONTH(@HojeB), 1);
DECLARE @InicioMesAnteriorB DATE = DATEADD(MONTH, -1, @InicioMesAtualB);
DECLARE @FimMesAnteriorB DATE = EOMONTH(@InicioMesAnteriorB);
DECLARE @InicioDoisMesesB DATE = DATEADD(MONTH, -2, @InicioMesAtualB);
DECLARE @FimDoisMesesB DATE = EOMONTH(@InicioDoisMesesB);

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendas WHERE IDCampanhaTelevendas = 4)
BEGIN
    SET IDENTITY_INSERT dbo.tblCampanhaTelevendas ON;

    INSERT INTO dbo.tblCampanhaTelevendas
        (IDCampanhaTelevendas, DataCompetencia, IDCampanhaTelevendasPeriodoCompetenciaSituacao,
         DataInicioApuracao, DataFimApuracao, TotalRanking, DescricaoCampanha,
         IDCampanhaTelevendasTipoApuracao, IDCampanhaTelevendasTipoCalculo, RegraValidacao,
         TipoValor, DataFimAntecipada, Observacao, ConsideraExclusivas, TipoCampanhaTelevendas, Dinamica)
    VALUES
        (4, @InicioMesAnteriorB, 4,
         @InicioMesAnteriorB, @FimMesAnteriorB, 10, 'Campanha Teste Positivacao Especifica Cancelada',
         6, 1, NULL,
         1, NULL, 'Campanha de teste - cancelada (deve ser ignorada no resumo)', 0, 0, 0),

        (5, @InicioDoisMesesB, 2,
         @InicioDoisMesesB, @FimDoisMesesB, 15, 'Campanha Teste Vendas Dois Meses Atras',
         2, 1, NULL,
         1, NULL, 'Campanha de teste - fechada, ha dois meses, com ranking completo', 0, 0, 1);

    SET IDENTITY_INSERT dbo.tblCampanhaTelevendas OFF;
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasMeta WHERE IDCampanhaTelevendas = 4)
INSERT INTO dbo.tblCampanhaTelevendasMeta (IDCampanhaTelevendas, MetaValor, GatilhoPositivacao, GatilhoVenda) VALUES
(4, 12000.00, 6000.00, 6000.00),
(5, 25000.00, 10000.00, 10000.00);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasFaixaPremiacao WHERE IDCampanhaTelevendas = 5)
INSERT INTO dbo.tblCampanhaTelevendasFaixaPremiacao
    (IDCampanhaTelevendas, ValorInicio, ValorFim, RankingInicio, RankingFim, PremioValor, LimitePremio, TipoPessoa) VALUES
(5, 0.00, 10000.00,     1, 5,  350.00, 1000.00, 1),
(5, 10000.01, 999999.00, 6, 15, 600.00, 1800.00, 1),
(5, 0.00, 999999.00,    1, 15, 200.00, 800.00,  2);
GO

/* ---------- Resultado apurado da campanha 5: 3 supervisores + 6 operadores, com ranking ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasResultado WHERE IDCampanhaTelevendas = 5 AND IDPessoaTelevendas = 901)
INSERT INTO dbo.tblCampanhaTelevendasResultado
    (IDCampanhaTelevendas, DataCompetencia, DescricaoCampanha, IDPessoaTelevendas, NomeDigitador, Ranking,
     ValorApurado, Premiacao, LogCalculo, DataCalculo, TipoPessoa, IDSupervisor, NomeSupervisor,
     RealizadoTotal, ObjetivoIndividual, PercentualRealizadoIndividual, ValorApuradoBees)
SELECT
    c.IDCampanhaTelevendas, c.DataCompetencia, c.DescricaoCampanha, r.IDPessoaTelevendas, r.NomeDigitador, r.Ranking,
    r.ValorApurado, r.Premiacao, r.LogCalculo, GETDATE(), r.TipoPessoa, r.IDSupervisor, r.NomeSupervisor,
    r.ValorApurado, r.ObjetivoIndividual,
    CASE WHEN r.ObjetivoIndividual = 0 THEN 0 ELSE CAST(r.ValorApurado / r.ObjetivoIndividual * 100 AS DECIMAL(18,2)) END,
    r.ValorApuradoBees
FROM dbo.tblCampanhaTelevendas c
CROSS JOIN (VALUES
    (901,  'Supervisor Teste',       901, 'Supervisor Teste',       1, 1, 32000.00, 800.00, 20000.00, 4000.00, 'Calculo de teste - supervisor Teste'),
    (902,  'Supervisor Sudeste',     902, 'Supervisor Sudeste',     1, 2, 28000.00, 600.00, 18000.00, 3500.00, 'Calculo de teste - supervisor Sudeste'),
    (903,  'Supervisor Nordeste',    903, 'Supervisor Nordeste',    1, 3, 24000.00, 500.00, 16000.00, 3000.00, 'Calculo de teste - supervisor Nordeste'),
    (1001, 'Digitador Teste',        901, 'Supervisor Teste',       2, 1, 12000.00, 350.00, 8000.00,  1500.00, 'Calculo de teste - operador 1'),
    (1002, 'Operador Televendas 2',  901, 'Supervisor Teste',       2, 2, 11000.00, 300.00, 8000.00,  1300.00, 'Calculo de teste - operador 2'),
    (1003, 'Operador Televendas 3',  902, 'Supervisor Sudeste',     2, 3, 10500.00, 280.00, 8000.00,  1200.00, 'Calculo de teste - operador 3'),
    (1004, 'Operador Televendas 4',  902, 'Supervisor Sudeste',     2, 4, 9800.00,  260.00, 8000.00,  1100.00, 'Calculo de teste - operador 4'),
    (1005, 'Operador Televendas 5',  903, 'Supervisor Nordeste',    2, 5, 9200.00,  240.00, 8000.00,  1000.00, 'Calculo de teste - operador 5'),
    (1006, 'Operador Televendas 6',  903, 'Supervisor Nordeste',    2, 6, 8700.00,  220.00, 8000.00,  900.00,  'Calculo de teste - operador 6')
) AS r(IDPessoaTelevendas, NomeDigitador, IDSupervisor, NomeSupervisor, TipoPessoa, Ranking, ValorApurado, Premiacao, ObjetivoIndividual, ValorApuradoBees, LogCalculo)
WHERE c.IDCampanhaTelevendas = 5;
GO

/* ---------- Metas individuais da campanha 5 (uma por operador) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasMetaInd WHERE IDCampanhaTelevendas = 5 AND IDPessoa = 1001)
INSERT INTO dbo.tblCampanhaTelevendasMetaInd (IDCampanhaTelevendas, IDPessoa, IDSupervisor, TipoPessoa, MetaValor, Gatilho, IDGerente) VALUES
(5, 1001, 901, 2, 8000.00, 4000.00, 201),
(5, 1002, 901, 2, 8000.00, 4000.00, 201),
(5, 1003, 902, 2, 8000.00, 4000.00, 202),
(5, 1004, 902, 2, 8000.00, 4000.00, 202),
(5, 1005, 903, 2, 8000.00, 4000.00, 159452),
(5, 1006, 903, 2, 8000.00, 4000.00, 159452);
GO

/* ---------- Escopo da campanha 5 (todos os fabricantes/linhas/produtos/clientes/digitadoras) ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasFabricante WHERE IDCampanhaTelevendas = 5)
INSERT INTO dbo.tblCampanhaTelevendasFabricante (IDCampanhaTelevendas, IDFabricante, Contem)
SELECT 5, IDFabricante, 'S' FROM (VALUES (501),(502),(503),(504)) AS f(IDFabricante);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasLinhaProduto WHERE IDCampanhaTelevendas = 5)
INSERT INTO dbo.tblCampanhaTelevendasLinhaProduto (IDCampanhaTelevendas, IDLinhaProduto, Contem)
SELECT 5, IDLinhaProduto, 'S' FROM (VALUES (601),(602),(603),(604),(605)) AS l(IDLinhaProduto);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasProduto WHERE IDCampanhaTelevendas = 5)
INSERT INTO dbo.tblCampanhaTelevendasProduto (IDCampanhaTelevendas, IDProduto, Contem)
SELECT 5, 700 + n, 'S' FROM (VALUES(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15)) AS p(n);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasCliente WHERE IDCampanhaTelevendas = 5)
INSERT INTO dbo.tblCampanhaTelevendasCliente (IDCampanhaTelevendas, IDCliente, Contem)
SELECT 5, 800 + n, 'S' FROM (VALUES(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15)) AS c(n);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasDigitadora WHERE IDCampanhaTelevendas = 5)
INSERT INTO dbo.tblCampanhaTelevendasDigitadora (IDCampanhaTelevendas, IDPessoaTelevendas, Contem)
SELECT 5, 1000 + n, 'S' FROM (VALUES(1),(2),(3),(4),(5),(6)) AS d(n);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.tblCampanhaTelevendasRegraValidacao WHERE IDCampanhaTelevendas = 5 AND IDCampanhaTelevendasValidar = 2)
INSERT INTO dbo.tblCampanhaTelevendasRegraValidacao (IDCampanhaTelevendas, IDCampanhaTelevendasValidar, ResultadoValidacao) VALUES
(5, 2, 'OK');
GO

/* ---------- Comissao dos novos clientes (803 a 815) ----------
   801-805 -> gerente 201 | 806-810 -> gerente 202 | 811-815 -> gerente 159452 (EXCLUIDO por
   regra fixa no codigo). Cenario sempre 1, pois o stub de fnObterIDComissaoVendasCenario
   sempre retorna 1. ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblComissaoVendasClienteVendedor WHERE IDCliente = 803)
INSERT INTO dbo.tblComissaoVendasClienteVendedor (IDCliente, IDGerente, IDComissaoVendasCenario)
SELECT
    IDCliente,
    CASE WHEN IDCliente BETWEEN 801 AND 805 THEN 201
         WHEN IDCliente BETWEEN 806 AND 810 THEN 202
         ELSE 159452 END,
    1
FROM (SELECT 803 + n AS IDCliente FROM (VALUES(0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12)) AS v(n)) x;
GO


USE GS300BI;
GO

/* ---------- Vendas consolidadas (usadas no relatorio de Sell-Out) ----------
   Uma linha "convencional" (branch 1 do UNION) e uma linha "Bees" (branch 2). */
DECLARE @Hoje2      DATE = CAST(GETDATE() AS DATE);
DECLARE @InicioMes2 DATE = DATEFROMPARTS(YEAR(@Hoje2), MONTH(@Hoje2), 1);
DECLARE @DataVenda  DATETIME = DATEADD(DAY, 5, CAST(@InicioMes2 AS DATETIME));

IF NOT EXISTS (SELECT 1 FROM dbo.tblConsolidacaoVendas WHERE IDPedido = 8001)
INSERT INTO dbo.tblConsolidacaoVendas
    (IDPedido, IDOperacao, IDProduto, IDProdutoLinha, IDPessoaCliente, IDPessoaVendedor,
     IDPessoaFabricante, IDDigitador, IDOrigem, DataFaturamento, Quantidade, Total) VALUES
(8001, 1, 701, 601, 801, 1001, 501, 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1, @DataVenda, 10, 1500.00),
(8002, 1, 702, 601, 802, 1001, 501, 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1, @DataVenda, 5,  750.00);
/* sem GO aqui de proposito: @DataVenda precisa continuar no escopo (era um bug do
   script original - o GO fechava o batch e o INSERT do pedido 9001 abaixo falhava
   com "e necessario declarar a variavel escalar @DataVenda") */

IF NOT EXISTS (SELECT 1 FROM dbo.tblConsolidacaoVendas WHERE IDPedido = 9001)
INSERT INTO dbo.tblConsolidacaoVendas
    (IDPedido, IDOperacao, IDProduto, IDProdutoLinha, IDPessoaCliente, IDPessoaVendedor,
     IDPessoaFabricante, IDDigitador, IDOrigem, DataFaturamento, Quantidade, Total) VALUES
(9001, 1, 701, 601, 801, 1001, 501, 'BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB', 1, @DataVenda, 20, 3000.00);
GO

/* ---------- Vendas Televendas ricas (canal 1) para DynamicReportController e SellOutSummaryController ----------
   Cobre os 15 clientes, 15 produtos, 4 fabricantes e 6 operadores criados na secao 4-B,
   distribuidos nos ultimos 3 meses (mes atual, anterior e retrasado). Inclui de proposito
   pedidos de clientes cujo gerente e o ID 159452 (EXCLUIDO por regra fixa no codigo em
   CampaignResumeSellOutRepository/DynamicReportRepository/SellOutSummaryRepository), para
   permitir confirmar que eles somem dos relatorios. ---------- */
IF NOT EXISTS (SELECT 1 FROM dbo.tblConsolidacaoVendas WHERE IDPedido = 8100)
BEGIN
    DECLARE @InicioMesAtualC DATE = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);

    ;WITH Clientes AS (
        SELECT * FROM (VALUES
            (1,801),(2,802),(3,803),(4,804),(5,805),(6,806),(7,807),(8,808),(9,809),(10,810),
            (11,811),(12,812),(13,813),(14,814),(15,815)
        ) AS C(Seq, IDCliente)
    ),
    Produtos AS (
        SELECT * FROM (VALUES
            (1,701,601),(2,702,602),(3,703,603),(4,704,604),(5,705,605),
            (6,706,601),(7,707,602),(8,708,603),(9,709,604),(10,710,605),
            (11,711,601),(12,712,602),(13,713,603),(14,714,604),(15,715,605)
        ) AS P(Seq, IDProduto, IDProdutoLinha)
    ),
    Fabricantes AS (
        SELECT * FROM (VALUES (1,501),(2,502),(3,503),(4,504)) AS F(Seq, IDFabricante)
    ),
    Operadores AS (
        SELECT * FROM (VALUES
            (1, CAST('11111111-1111-1111-1111-111111111001' AS UNIQUEIDENTIFIER)),
            (2, CAST('11111111-1111-1111-1111-111111111002' AS UNIQUEIDENTIFIER)),
            (3, CAST('11111111-1111-1111-1111-111111111003' AS UNIQUEIDENTIFIER)),
            (4, CAST('11111111-1111-1111-1111-111111111004' AS UNIQUEIDENTIFIER)),
            (5, CAST('11111111-1111-1111-1111-111111111005' AS UNIQUEIDENTIFIER)),
            (6, CAST('11111111-1111-1111-1111-111111111006' AS UNIQUEIDENTIFIER))
        ) AS O(Seq, IDDigitador)
    ),
    Meses AS (
        SELECT 0 AS MesOffset UNION ALL SELECT 1 UNION ALL SELECT 2
    ),
    Combos AS (
        SELECT
            c.Seq AS ClienteSeq, c.IDCliente,
            p.Seq AS ProdutoSeq, p.IDProduto, p.IDProdutoLinha,
            fa.IDFabricante,
            op.IDDigitador,
            m.MesOffset,
            CASE WHEN c.IDCliente BETWEEN 801 AND 805 THEN 201
                 WHEN c.IDCliente BETWEEN 806 AND 810 THEN 202
                 ELSE 159452 END AS IDGerente,
            ROW_NUMBER() OVER (ORDER BY m.MesOffset, c.Seq, p.Seq) AS RowSeq
        FROM Clientes c
        CROSS JOIN Produtos p
        CROSS JOIN Meses m
        CROSS APPLY (SELECT IDFabricante FROM Fabricantes WHERE Seq = ((c.Seq + p.Seq) % 4) + 1) fa
        CROSS APPLY (SELECT IDDigitador FROM Operadores WHERE Seq = ((c.Seq + p.Seq) % 6) + 1) op
        WHERE (c.Seq + p.Seq) % 4 = 0
    )
    INSERT INTO dbo.tblConsolidacaoVendas
        (IDPedido, IDOperacao, IDProduto, IDProdutoLinha, IDPessoaCliente, IDPessoaVendedor,
         IDPessoaFabricante, IDPessoaGerente, IDDigitador, IDOrigem, DataFaturamento, Quantidade, Total)
    SELECT
        8100 + cb.RowSeq,
        1,
        cb.IDProduto,
        cb.IDProdutoLinha,
        cb.IDCliente,
        NULL,
        cb.IDFabricante,
        cb.IDGerente,
        cb.IDDigitador,
        1,
        DATEADD(DAY, (cb.ClienteSeq + cb.ProdutoSeq) % 25, DATEADD(MONTH, -cb.MesOffset, @InicioMesAtualC)),
        2 + ((cb.ClienteSeq + cb.ProdutoSeq) % 8),
        CAST((2 + ((cb.ClienteSeq + cb.ProdutoSeq) % 8)) * (35.00 + ((cb.ProdutoSeq * 7) % 50)) AS DECIMAL(18,2))
    FROM Combos cb;
END
GO

/* ---------- Vendas Bees ricas (canal 2) ----------
   Mesmos clientes/produtos, deslocamento diferente no modulo, cobrindo os 2 meses mais
   recentes. Para os clientes 811 a 815 (gerente 159452, ja excluido), metade dos pedidos
   usa de proposito a GUID 'ce782385-bece-485b-9e33-05ec60591610' (vendedora Bees excluida
   por regra fixa no codigo, exceto quando ConsideraGrandesContas = true), permitindo testar
   as duas regras de exclusao juntas e separadas. ---------- */
IF NOT EXISTS (SELECT 1 FROM GS300GP.dbo.tblVendasBees WHERE IDPedido = 9100)
BEGIN
    DECLARE @InicioMesAtualD DATE = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);

    SELECT
        ROW_NUMBER() OVER (ORDER BY m.MesOffset, c.Seq, p.Seq) AS RowSeq,
        c.Seq AS ClienteSeq, c.IDCliente,
        p.Seq AS ProdutoSeq, p.IDProduto, p.IDProdutoLinha,
        fa.IDFabricante,
        CASE
            WHEN c.IDCliente BETWEEN 811 AND 815 AND ((c.Seq + p.Seq) % 2 = 0)
                THEN CAST('ce782385-bece-485b-9e33-05ec60591610' AS UNIQUEIDENTIFIER)
            WHEN ((c.Seq + p.Seq) % 2 = 0)
                THEN CAST('22222222-2222-2222-2222-222222222001' AS UNIQUEIDENTIFIER)
            ELSE CAST('22222222-2222-2222-2222-222222222002' AS UNIQUEIDENTIFIER)
        END AS IDDigitador,
        m.MesOffset,
        CASE WHEN c.IDCliente BETWEEN 801 AND 805 THEN 201
             WHEN c.IDCliente BETWEEN 806 AND 810 THEN 202
             ELSE 159452 END AS IDGerente
    INTO #VendasBees
    FROM (SELECT * FROM (VALUES
            (1,801),(2,802),(3,803),(4,804),(5,805),(6,806),(7,807),(8,808),(9,809),(10,810),
            (11,811),(12,812),(13,813),(14,814),(15,815)
        ) AS C(Seq, IDCliente)) c
    CROSS JOIN (SELECT * FROM (VALUES
            (1,701,601),(2,702,602),(3,703,603),(4,704,604),(5,705,605),
            (6,706,601),(7,707,602),(8,708,603),(9,709,604),(10,710,605),
            (11,711,601),(12,712,602),(13,713,603),(14,714,604),(15,715,605)
        ) AS P(Seq, IDProduto, IDProdutoLinha)) p
    CROSS JOIN (SELECT 0 AS MesOffset UNION ALL SELECT 1) m
    CROSS APPLY (SELECT IDFabricante FROM (VALUES (1,501),(2,502),(3,503),(4,504)) AS F(Seq, IDFabricante) WHERE Seq = ((c.Seq + p.Seq) % 4) + 1) fa
    WHERE (c.Seq + p.Seq) % 4 = 2;

    /* tblVendasBees mora no banco GS300GP (mesmo banco da conexao da API - por isso o
       codigo real referencia ela sem prefixo de schema/banco), mas este trecho roda com
       o contexto em GS300BI (para inserir em tblConsolidacaoVendas na sequencia), entao
       precisa do nome de 3 partes aqui. */
    INSERT INTO GS300GP.dbo.tblVendasBees (IDPedido, IDOperacao, IDDigitador)
    SELECT 9100 + RowSeq, 1, IDDigitador FROM #VendasBees;

    INSERT INTO dbo.tblConsolidacaoVendas
        (IDPedido, IDOperacao, IDProduto, IDProdutoLinha, IDPessoaCliente, IDPessoaVendedor,
         IDPessoaFabricante, IDPessoaGerente, IDDigitador, IDOrigem, DataFaturamento, Quantidade, Total)
    SELECT
        9100 + RowSeq, 1, IDProduto, IDProdutoLinha, IDCliente, NULL, IDFabricante, IDGerente, IDDigitador, 2,
        DATEADD(DAY, (ClienteSeq + ProdutoSeq) % 25, DATEADD(MONTH, -MesOffset, @InicioMesAtualD)),
        1 + ((ClienteSeq + ProdutoSeq) % 6),
        CAST((1 + ((ClienteSeq + ProdutoSeq) % 6)) * (40.00 + ((ProdutoSeq * 5) % 40)) AS DECIMAL(18,2))
    FROM #VendasBees;

    DROP TABLE #VendasBees;
END
GO


/* =====================================================================
   5) COMO TESTAR CADA ROTA (execute a API apontando para GS300GP e use
      os IDs de teste criados acima)
   =====================================================================

   GET  api/campaign                                  -> lista as 3 campanhas de teste
   GET  api/campaign/1                                 -> campanha 1 (aberta)
   GET  api/campaign/period?date=<DataCompetencia da campanha 1>  -> ver DataCompetencia gravada (SELECT DataCompetencia FROM GS300GP.dbo.tblCampanhaTelevendas WHERE IDCampanhaTelevendas = 1)

   GET  Campaign/Params/Detail/fabricante/1            -> Fabricante Teste LTDA (501)
   GET  Campaign/Params/Detail/linha/1                 -> Linha Higiene e Beleza (601)
   GET  Campaign/Params/Detail/produto/1                -> Produto Teste 700ML / Sache (701/702)
   GET  Campaign/Params/Detail/cliente/1?pageNumber=1&pageSize=10   -> Cliente 1 e 2 (801/802), paginado
   GET  Campaign/Params/Detail/cliente/1?idCliente=801  -> filtra so o cliente 801

   GET  api/campaign-summary?competenceDateFrom=<DataCompetencia da campanha 1>
        -> retorna a campanha 1 (aberta), NAO retorna a campanha 3 (cancelada)
   GET  api/campaign-summary/details/1                  -> detalhe supervisor (901) + operador (1001)

   GET  api/campaign-summary-by-description?competenceDateFrom=<data>&description=Campanha Teste
        -> (ajuste a rota conforme o controller de CampaignSummaryByDescription)

   GET  api/campaign-result-report?competenceDateFrom=<DataCompetencia da campanha 1>
        -> lista resultados de todas as campanhas nao canceladas a partir da data

   POST/GET (conforme o controller) para Sell-Out, usando o corpo:
     {
       "idCampaign": 1,
       "idOrigim": 1,
       "isValidSellOutGC": true,
       "startDate": "<DataInicioApuracao da campanha 1>",
       "endDate": "<DataFimApuracao da campanha 1>",
       "manufacturer": [{ "idManufacturer": 501 }],
       "productLines": [{ "idProductLine": 601 }],
       "products": [{ "idProduct": 701 }, { "idProduct": 702 }],
       "clients": [{ "idClients": 801 }, { "idClients": 802 }]
     }
     -> retorna 2 linhas: uma da venda "convencional" (pedido 8001/8002, vendedor
        "Vendedor Teste Convencional") e uma da venda "Bees" (pedido 9001, vendedor
        "Vendedor Teste Bees").

   ---------- Massa adicional (secao 4-B / 4-C) para os relatorios mais novos ----------

   IDs criados a mais: Fabricantes 502-504 | Gerentes 201, 202 e 159452 (excluido) |
   Clientes 803-815 (15 no total) | Linhas 602-605 | Produtos 703-715 (15 no total) |
   Supervisores 902, 903 | Operadores 1002-1006 | Campanha 4 (cancelada) e 5 (fechada,
   ha 2 meses, com ranking completo de 3 supervisores + 6 operadores) | Vendas
   Televendas 8100+ e Bees 9100+, distribuidas nos ultimos 2-3 meses.

   POST api/sellout-summary
   POST api/sellout-summary/monthly
     Corpo de exemplo:
       {
         "startDate": "<primeiro dia de 3 meses atras>",
         "endDate": "<hoje>",
         "idManufacturer": [501, 502, 503, 504],
         "productLine": [601, 602, 603, 604, 605],
         "products": [],
         "clients": [],
         "consideraGrandesContas": false
       }
     -> soma as vendas Televendas + Bees dos ultimos 3 meses, EXCLUINDO automaticamente
        os pedidos do gerente 159452 e (quando consideraGrandesContas = false) os pedidos
        da GUID Bees 'ce782385-...'. Rode duas vezes (true/false) para comparar o efeito
        do filtro "Grandes Contas".

   POST api/dynamic-report
     Corpo de exemplo:
       {
         "startDate": "<primeiro dia de 3 meses atras>",
         "endDate": "<hoje>",
         "groupBy": ["NomeFabricante", "DataCompetencia"],
         "metrics": ["ValorVendido", "Positivacao"]
       }
     -> agrupa as vendas Televendas + Bees por fabricante e mes de competencia; os
        totais por fabricante devem bater com a soma manual das linhas geradas na
        secao 4-C acima (pedidos 8100+ e 9100+).

   GET api/campaign-summary?competenceDateFrom=<DataCompetencia da campanha 5>
     -> a campanha 5 aparece com TotalPot somando as 3 faixas de premiacao cadastradas
        e PercentageAchieved calculado a partir de ValorApurado / MetaValor.
   GET api/campaign-summary/details/5
     -> retorna os 3 supervisores + 6 operadores com Ranking de 1 a 6 (por TipoPessoa).
   ===================================================================== */
