# Guia de Estudo — Padrões de C# no ApiCampaignDash

Roteiro construído a partir do código real do projeto, para revisar conceitos de C#/.NET depois de um tempo parado. Cada seção mostra "onde está no seu código" + "o conceito por trás".

---

## 1. Arquitetura em Camadas (N-Tier / Clean-ish Architecture)

O projeto está dividido em 6 projetos dentro da solution `ApiCampaignDash.slnx`:

```
ApiCampaignDash            → Presentation (Controllers, Program.cs)
ApiCampaignDash.Domain     → Entidades, Enums, Interfaces (não depende de nada)
ApiCampaignDash.Application→ Services, DTOs, Mappings (AutoMapper)
ApiCampaignDash.Infrastructure → DbContext, Repositories, EF Configurations
ApiCampaignDash.Tests      → xUnit + Moq
ApiCampaignDash.UI         → Front-end
```

Fluxo de dependência (regra de ouro: dependências apontam **para dentro**, em direção ao Domain):

```
Presentation → Application → Domain ← Infrastructure
```

- `Domain` não referencia nenhum outro projeto — é o núcleo estável.
- `Application` e `Infrastructure` dependem de `Domain`, mas não uma da outra diretamente na lógica (a ligação acontece via DI no `Program.cs`).
- Isso é a base da **Dependency Inversion** (ver seção 3.5): camadas de alto nível não dependem de detalhes de implementação, só de interfaces definidas no Domain.

**Por que isso importa:** você pode trocar o banco (Infrastructure) ou a API (Presentation) sem tocar nas regras de negócio (Domain/Application).

---

## 2. Injeção de Dependência (DI)

Arquivo: `ApiCampaignDash/Program.cs`

```csharp
// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(CampaignProfile));

// Repositórios e Services — sempre em pares Interface -> Implementação
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<ICampaignSummaryRepository, CampaignSummaryRepository>();
builder.Services.AddScoped<ICampaignSummaryService, CampaignSummaryService>();
```

### Conceitos para revisar

| Método | Ciclo de vida | Quando usar |
|---|---|---|
| `AddScoped` | 1 instância por requisição HTTP | Padrão para Services/Repositories que usam `DbContext` (o próprio `DbContext` é Scoped) |
| `AddTransient` | Nova instância a cada injeção | Serviços leves, sem estado, baratos de criar |
| `AddSingleton` | 1 instância para a aplicação inteira | Configuração, cache em memória, clientes HTTP reaproveitáveis |

⚠️ Regra que costuma pegar quem voltou a mexer em DI: **nunca injete um serviço `Scoped` dentro de um `Singleton`** — vaza o `DbContext` entre requisições e gera bugs difíceis de rastrear.

### Injeção via construtor (o padrão usado 100% do projeto)

```csharp
// Controller depende da abstração, não da implementação
public class CampaignController : ControllerBase
{
    private readonly ICampaignService _campaignService;

    public CampaignController(ICampaignService campaignService)
    {
        _campaignService = campaignService;
    }
}
```

O container de DI do ASP.NET Core resolve `ICampaignService` → `CampaignService` automaticamente, e dentro dele resolve `ICampaignRepository` e `IMapper`, em cascata.

---

## 3. SOLID — cada princípio com exemplo real do repo

### 3.1 Single Responsibility (S)
Cada classe tem um único motivo para mudar:
- `CampaignController` → só lida com HTTP (rota, status code).
- `CampaignService` → só orquestra regra de negócio + mapeamento.
- `CampaignRepository` → só acesso a dados.

```csharp
// Controller: não sabe nada de EF Core nem de SQL
[HttpGet("{id:int}")]
public async Task<ActionResult<CampaignDto>> GetById(int id)
{
    var campaign = await _campaignService.GetByIdAsync(id);
    return campaign == null ? NotFound() : Ok(campaign);
}
```

### 3.2 Open/Closed (O)
`BaseRepository<T>` fornece o comportamento comum; repositórios específicos **estendem** sem **modificar** a classe base:

```csharp
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.AsNoTracking().ToListAsync();
}

public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
{
    // Adiciona comportamento novo, sem tocar no BaseRepository
    public async Task<IEnumerable<Campaign>> GetByPeriodCampaignAsync(DateTime datetime) =>
        await _context.Campaigns.AsNoTracking()
            .Where(c => c.CompetenceDate == datetime)
            .ToListAsync();
}
```

### 3.3 Liskov Substitution (L)
Qualquer lugar que espera `IBaseRepository<Campaign>` pode receber um `CampaignRepository` sem quebrar nada — o contrato é respeitado (mesmos parâmetros, mesmo tipo de retorno, sem exceções surpresa).

### 3.4 Interface Segregation (I)
As interfaces são pequenas e focadas por entidade — não existe uma "IRepositoryDeusa" com 40 métodos:

```csharp
public interface ICampaignSummaryRepository
{
    Task<IEnumerable<CampaignSummary>> GetSummaryAsync(DateTime competenceDateFrom);
}
```

### 3.5 Dependency Inversion (D)
`CampaignService` depende de `ICampaignRepository` (abstração no Domain), nunca de `CampaignRepository` (implementação na Infrastructure):

```csharp
public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repository; // abstração
    private readonly IMapper _mapper;

    public CampaignService(ICampaignRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
}
```

Isso é o que permite trocar `CampaignRepository` por um mock em testes (`Moq`) sem tocar no `CampaignService`.

---

## 4. DTOs (Data Transfer Objects)

Pasta: `ApiCampaignDash.Application/DTOs/`

```csharp
public class CampaignDto
{
    public int IdCampaign { get; set; }
    public DateTime? CompetenceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public AssessmentType? IdAssessmentType { get; set; }
    // ...
}
```

**Por que DTO e não devolver a Entity direto na API?**
- Desacopla o contrato público da API do schema do banco (colunas em português, nomes de tabela legados, etc. ficam escondidos).
- Evita over-posting/expor campos internos sem querer.
- Permite formatar dados diferente do que está persistido (ex.: `CampaignSummaryDto` já vem com valores calculados/agregados, não é 1:1 com uma tabela).

### Mapeamento com AutoMapper

```csharp
// ApiCampaignDash.Application/Mappings/CampaignProfile.cs
public class CampaignProfile : Profile
{
    public CampaignProfile()
    {
        CreateMap<Campaign, CampaignDto>();
    }
}
```

Registro único no `Program.cs`:
```csharp
builder.Services.AddAutoMapper(cfg => { }, typeof(CampaignProfile));
```

Uso no Service:
```csharp
var campaigns = await _repository.GetAllAsync();
return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
```

**Vale revisar:** `CreateMap<Origem, Destino>()` por padrão mapeia por convenção (mesmo nome de propriedade). Quando os nomes divergem, se usa `.ForMember(dest => dest.X, opt => opt.MapFrom(src => src.Y))` — não tem exemplo disso no projeto ainda, mas é o próximo passo natural quando os nomes não baterem mais.

---

## 5. Repository Pattern (sem Unit of Work explícito)

```csharp
public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
}

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    // ...
}
```

Note que **não existe um `IUnitOfWork` separado** — isso é comum e aceitável em projetos menores: o próprio `AppDbContext` (que é `Scoped`, uma instância por requisição) já cumpre esse papel, já que todos os repositórios compartilham a mesma instância de contexto dentro de uma requisição. Se o projeto crescer e precisar coordenar `SaveChanges()` entre múltiplos repositórios numa mesma transação, aí sim vale introduzir um Unit of Work explícito.

### Repositório com SQL cru (para relatórios complexos)

```csharp
// CampaignSummaryRepository.cs — não herda de BaseRepository, é standalone
public async Task<IEnumerable<CampaignSummary>> GetSummaryAsync(DateTime competenceDateFrom)
{
    var parameters = new object[] { new SqlParameter("@dataMinima", competenceDateFrom.Date) };
    return await _context.Database
        .SqlQueryRaw<CampaignSummary>(Sql, parameters)
        .ToListAsync();
}
```

Ponto de atenção didático: aqui é usado `SqlParameter` explícito (parametrizado) — **isso evita SQL Injection**. Sempre que usar `SqlQueryRaw`/`FromSqlRaw`, os valores variáveis precisam entrar como parâmetro, nunca por concatenação de string.

---

## 6. Entity Framework Core

### DbContext

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Campaign> Campaigns { get; set; }
    // ... mais DbSets

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

`ApplyConfigurationsFromAssembly` varre o assembly procurando classes que implementam `IEntityTypeConfiguration<T>` — é assim que todas as classes em `Data/Configurations/` são carregadas automaticamente, sem precisar registrar uma a uma.

### Fluent API (preferida para regras de mapeamento — fica fora da entidade)

```csharp
public class CampaignConfig : IEntityTypeConfiguration<Campaign>
{
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.ToTable("tblCampanhaTelevendas");
        builder.HasKey(x => x.IdCampaign);
        builder.Property(x => x.Description).HasColumnName("DescricaoCampanha").HasMaxLength(150);
        builder.Property(x => x.ConsidersExclusives)
            .HasColumnName("ConsideraExclusivas")
            .HasConversion<int?>(); // conversão de tipo bool? <-> int? no banco
    }
}
```

Chave composta:
```csharp
builder.HasKey(x => new { x.IdCampaign, x.IdManufacturer });
builder.Ignore(x => x.Name); // propriedade que existe na classe mas não é coluna
```

### Data Annotations (usada nas entidades, para validação — não para mapeamento de tabela)

```csharp
public class Campaign
{
    [Key]
    public int IdCampaign { get; set; }

    [Required(ErrorMessage = "O campo Descrição da Campanha é obrigatório.")]
    [StringLength(150, ErrorMessage = "...")]
    public string Description { get; set; } = string.Empty;
}
```

**Ponto de atenção:** o projeto mistura Fluent API (nome de tabela/coluna) com Data Annotations (`[Required]`, `[StringLength]`) na mesma entidade. Funciona, mas o mais comum em times maiores é escolher um único lugar de verdade — geralmente Fluent API pra tudo, porque fica testável e não "polui" a entidade de domínio com atributos de infraestrutura.

### Boas práticas de performance já usadas no projeto
```csharp
await _dbSet.AsNoTracking().ToListAsync();
```
`AsNoTracking()` desliga o change tracker do EF — usar sempre em leituras que não vão ser alteradas e salvas depois (relatórios, GETs). Reduz uso de memória e CPU.

---

## 7. Async/Await — usado ponta a ponta

```
Controller (async Task<ActionResult<T>>)
   ↓ await
Service (async Task<T>)
   ↓ await
Repository (async Task<T>)
   ↓ await
EF Core (ToListAsync / FindAsync)
```

Exemplo completo da cadeia:
```csharp
// Controller
[HttpGet]
public async Task<ActionResult<IEnumerable<CampaignDto>>> GetAll()
{
    var campaigns = await _campaignService.GetAllAsync();
    return Ok(campaigns);
}

// Service
public async Task<IEnumerable<CampaignDto>> GetAllAsync()
{
    var campaigns = await _repository.GetAllAsync();
    return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
}

// Repository
public async Task<IEnumerable<T>> GetAllAsync() =>
    await _dbSet.AsNoTracking().ToListAsync();
```

Regra de revisão: `async` sobe em cascata — se o método mais interno é `async`, todo mundo que o chama deveria ser `async` também (evitar `.Result`/`.Wait()`, que bloqueiam threads e podem causar deadlock em contexto de ASP.NET).

---

## 8. Validação

Hoje só usa **Data Annotations** nas entidades de Domain:
```csharp
[Required(ErrorMessage = "O campo Nome do Supervisor é obrigatório.")]
[StringLength(200, ErrorMessage = "...")]
public string SupervisorName { get; set; } = string.Empty;
```

Isso funciona automaticamente no `ModelState` quando a entidade é usada como parâmetro de action com `[ApiController]` — mas repare que os DTOs (que é o que realmente chega via request) **não têm anotações de validação hoje**. Se algum endpoint futuro receber `CampaignDto` no body (POST/PUT), vale duplicar/mover as regras de validação para o DTO, já que é ele quem representa o contrato de entrada da API.

Próximo passo natural de estudo: **FluentValidation** — separa regra de validação da classe de dados, permite validações condicionais complexas e injeção de dependência dentro do validador (não usado ainda aqui).

---

## 9. Configuração

`appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=...;User ID=...;Password=...;..."
  }
}
```

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

⚠️ **Aviso de segurança, fora do escopo didático mas importante:** a senha do banco está em texto puro no `appsettings.json`, que normalmente vai pro controle de versão. Vale mover para `appsettings.Development.json` (fora do git) ou `dotnet user-secrets` / variáveis de ambiente / Azure Key Vault em produção.

**Conceito para revisar:** o projeto usa `Configuration.GetConnectionString(...)` direto. Em projetos maiores costuma-se usar o **Options Pattern** (`IOptions<T>`) para configurações fortemente tipadas:
```csharp
builder.Services.Configure<MinhaConfigSection>(builder.Configuration.GetSection("MinhaConfigSection"));
// depois, injeta IOptions<MinhaConfigSection> no construtor
```
Não usado ainda no projeto — bom próximo tópico de estudo.

---

## 10. Middleware, Filtros e Tratamento de Exceção

```csharp
app.UseHttpsRedirection();
app.UseCors("AllowElectronApp");
app.UseAuthorization();
```

O projeto usa só middlewares built-in (CORS, HTTPS redirect, Authorization). **Não há**:
- Middleware global de tratamento de exceção (`UseExceptionHandler` ou `try/catch` central)
- Action Filters customizados
- Autenticação/Autorização real (`app.UseAuthorization()` está registrado mas não há nenhum `[Authorize]` nem JWT configurado)

Esses são ótimos próximos passos de estudo, porque são padrões muito comuns em API .NET modernas:
- `IExceptionHandler` (novo padrão do .NET 8+) ou middleware customizado para devolver erros padronizados (`ProblemDetails`).
- JWT Bearer (`AddAuthentication().AddJwtBearer(...)`) + `[Authorize]` nos controllers.

---

## 11. Testes

```xml
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="Moq" Version="4.20.72" />
```

Estrutura pronta, mas ainda sem testes reais (`UnitTest1.cs` é o esqueleto padrão do template). Como o projeto já segue DI + interfaces em todo canto, ele está **pronto para ser testado com mocks** — é uma ótima prática revisar isso.

Exemplo de como um teste ficaria, usando o que já existe:
```csharp
public class CampaignServiceTests
{
    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCampaignDoesNotExist()
    {
        var repoMock = new Mock<ICampaignRepository>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Campaign?)null);
        var mapper = new Mock<IMapper>();

        var sut = new CampaignService(repoMock.Object, mapper.Object);

        var result = await sut.GetByIdAsync(1);

        Assert.Null(result);
    }
}
```
Isso só é possível **porque** `CampaignService` depende de `ICampaignRepository` (interface), não da classe concreta — é a Dependency Inversion (seção 3.5) pagando dividendo na hora de testar.

---

## Resumo — o que o projeto já demonstra bem vs. o que vale estudar a seguir

| Já implementado e sólido | Bom próximo passo de estudo |
|---|---|
| Arquitetura em camadas (Domain/Application/Infrastructure/Presentation) | Unit of Work explícito (se crescer) |
| DI via construtor, `AddScoped` | Options Pattern (`IOptions<T>`) |
| SOLID (S, O, L, I, D todos com exemplo real) | FluentValidation |
| Repository genérico + especializado | Middleware global de exceção / `ProblemDetails` |
| DTO + AutoMapper | Autenticação JWT + `[Authorize]` |
| EF Core (Fluent API + Data Annotations, `AsNoTracking`) | Testes reais (xUnit/Moq já configurados, só faltam os testes) |
| Async/await consistente em todas as camadas | Logging estruturado (Serilog) |
| SQL parametrizado (proteção contra SQL Injection) | API Versioning / Swagger mais completo |

---

*Gerado a partir da leitura do código em `C:\Users\costa\source\repos\RudineiCTS\ApiCampaignDash` em 2026-07-07.*
