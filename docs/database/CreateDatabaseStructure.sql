/* =====================================================================
   ApiCampaignDash - Script de criação de estrutura de banco de dados
   =====================================================================
   Gerado a partir do código-fonte (AppDbContext, EntityTypeConfigurations,
   entidades de domínio e queries SQL cruas dos repositórios) em 2026-07-18.

   O script está dividido em duas partes:

   PARTE 1 - Tabelas "próprias" do módulo de campanha (schema
             tblCampanhaTelevendas* e tblCampanhaComercial), cuja estrutura
             foi extraída com fidelidade das EntityTypeConfiguration (nomes
             de coluna, tipos, tamanhos e chaves primárias/estrangeiras).

   PARTE 2 - Tabelas/views EXTERNAS (GS300ERP, GS300BI e outras usadas via
             SQL cru em CampaignResumeSellOutRepository) recriadas de forma
             MÍNIMA, apenas com as colunas efetivamente usadas nas consultas.
             Não refletem a estrutura real completa dessas tabelas/views no
             ERP (chaves, índices, demais colunas, tipos exatos) - servem
             apenas para permitir testes locais. Em produção, uvwPessoaFisicaJuridica
             e vwProduto são VIEWS, não tabelas.

   Ajuste nomes de banco/schema conforme o ambiente onde for executar.
   ===================================================================== */

SET NOCOUNT ON;
GO

/* =====================================================================
   PARTE 1 - TABELAS DO MÓDULO DE CAMPANHA (GS300GP)
   ===================================================================== */

/* ---------- Tabelas de domínio / lookup ---------- */

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
    IDCampanhaTelevendas                           INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DataCompetencia                                 DATETIME NULL,
    IDCampanhaTelevendasPeriodoCompetenciaSituacao   INT NOT NULL,
    DataInicioApuracao                              DATETIME NULL,
    DataFimApuracao                                 DATETIME NULL,
    TotalRanking                                    INT NULL,
    DescricaoCampanha                               VARCHAR(150) NOT NULL,
    IDCampanhaTelevendasTipoApuracao                 INT NOT NULL,
    IDCampanhaTelevendasTipoCalculo                  INT NOT NULL,
    RegraValidacao                                  INT NULL,
    TipoValor                                       INT NOT NULL,
    DataFimAntecipada                               DATETIME NULL,
    Observacao                                      VARCHAR(150) NULL,
    ConsideraExclusivas                             INT NULL,      -- mapeado como bool? -> int? (HasConversion<int?>)
    TipoCampanhaTelevendas                          BIT NULL,
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

/* ---------- Faixa de premiação ---------- */

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

/* ---------- Distinção pessoa x fabricante ---------- */

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

/* ---------- Campanha comercial (legado) ----------
   Somente IDCampanhaComercial e NomeCampanha são de fato mapeados via
   EntityTypeConfiguration (CampaignResumeConfig). As demais propriedades da
   entidade CampaignResume (CampaignType, SalesTarget, AwardValue,
   ValueRealized, VerificationType, CalculationType, isValidExclusive) não
   possuem HasColumnName definido no código atual, portanto os nomes reais
   dessas colunas no banco não puderam ser confirmados a partir do
   repositório - ajuste conforme a estrutura real de tblCampanhaComercial. */

IF OBJECT_ID('dbo.tblCampanhaComercial', 'U') IS NULL
CREATE TABLE dbo.tblCampanhaComercial
(
    IDCampanhaComercial INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NomeCampanha        VARCHAR(100) NOT NULL
);
GO


/* =====================================================================
   PARTE 2 - TABELAS/VIEWS EXTERNAS (reprodução MÍNIMA)
   Usadas via SQL cru em CampaignResumeSellOutRepository,
   CampaignResultReportRepository e CampaignSummaryRepository.
   Contêm apenas as colunas referenciadas nas queries.
   ===================================================================== */

/* GS300BI.dbo.tblConsolidacaoVendas */
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
    IDDigitador         UNIQUEIDENTIFIER NULL,
    IDOrigem            INT NULL,
    DataFaturamento     DATETIME NULL,
    Quantidade          INT NULL,
    Total               DECIMAL(18,2) NULL
);
GO

/* GS300ERP.dbo.uvwPessoaFisicaJuridica -> em produção é uma VIEW; aqui recriada como tabela para testes */
IF OBJECT_ID('dbo.uvwPessoaFisicaJuridica', 'U') IS NULL
CREATE TABLE dbo.uvwPessoaFisicaJuridica
(
    IDPessoa    INT NOT NULL PRIMARY KEY,
    CpfCnpj     VARCHAR(20) NULL,
    RazaoSocial VARCHAR(200) NULL,
    Nome        VARCHAR(200) NULL
);
GO

/* GS300ERP.dbo.vwProduto -> em produção é uma VIEW; aqui recriada como tabela para testes */
IF OBJECT_ID('dbo.vwProduto', 'U') IS NULL
CREATE TABLE dbo.vwProduto
(
    IDProduto   INT NOT NULL PRIMARY KEY,
    DescProduto VARCHAR(200) NULL,
    CodBarras   VARCHAR(20) NULL
);
GO

/* GS300ERP.dbo.tblSegUsuario */
IF OBJECT_ID('dbo.tblSegUsuario', 'U') IS NULL
CREATE TABLE dbo.tblSegUsuario
(
    IDUsuario   UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    NomeUsuario VARCHAR(200) NULL
);
GO

/* tblVendasBees - PK/estrutura real não confirmada; apenas colunas usadas na query */
IF OBJECT_ID('dbo.tblVendasBees', 'U') IS NULL
CREATE TABLE dbo.tblVendasBees
(
    IDPedido    INT NULL,
    IDOperacao  INT NULL,
    IDDigitador UNIQUEIDENTIFIER NULL
);
GO

/* tblComissaoVendasClienteVendedor - PK/estrutura real não confirmada; apenas colunas usadas na query */
IF OBJECT_ID('dbo.tblComissaoVendasClienteVendedor', 'U') IS NULL
CREATE TABLE dbo.tblComissaoVendasClienteVendedor
(
    IDCliente               INT NULL,
    IDGerente               INT NULL,
    IDComissaoVendasCenario INT NULL
);
GO
