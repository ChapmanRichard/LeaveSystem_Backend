using Api.BLL.CommandDecorators;
using Api.BLL.CommandHandlers;
using Api.BLL.Services;
using Api.Common.Commands;
using Api.Common.Commands.Decorators;
using Api.Common.DI;
using Api.Common.Events;
using Api.Common.Helper;
using Api.Common.Loggers;
using Api.Common.Providers;
using Api.Common.Queries;
using Api.Common.Queries.Decorators;
using Api.Common.Repositories;
using Api.Common.Vaildators;
using Api.Contract.Model;
using Api.Contract.ServiceInterfaces;
using Api.DAL;
using Api.DAL.Repositories;
using Api.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// ���� Scrutor �� using ָ�������� Decorate ��չ����
using Scrutor;
using Serilog;
using Serilog.Events;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog bootstrap logger
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateBootstrapLogger();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// AutoMapper configuration
builder.Services.AddAutoMapper(config =>
{
    // �ֶ�����Profile��
    config.AddProfile<LeaveSystemWebAutoMapperProfile>();

    // ��ɨ��ָ������
    config.AddMaps(Assembly.GetExecutingAssembly());
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // ���� JWT Bearer �İ�ȫ���壨Authorize ��ť��
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "�����ʽ: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});


// DbContext - use LeaveSystemDBContext and read connection string from appsettings
builder.Services.AddDbContext<LeaveSystemDBContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))));

// Register core services
builder.Services.AddSingleton<ICommandBus, MemoryCommandBus>();
builder.Services.AddScoped<IWebCommandBus, WebMemoryCommandBus>();
builder.Services.AddSingleton<IQueryProcessor, MemoryQueryProcessor>();
builder.Services.AddSingleton<IEventBus, MemoryEventBus>();

// Register validators (open generic)
builder.Services.AddScoped(typeof(IValidator<>), typeof(DataAnnotationsValidator<>));

// Register helper/provider
builder.Services.AddScoped<ActionNameProvider>();

// Register HttpContextAccessor for AppContextHelper
builder.Services.AddHttpContextAccessor();

// Scan and register handlers (�� Api.BLL.* assembly Ϊ�����������Ŀ��������)
builder.Services.Scan(scan => scan
    // ɨ��������ص���Ŀ����
    .FromAssemblies(
        typeof(Api.BLL.CommandHandlers.NewsCommandHandler).Assembly, // Api.BLL
        typeof(Api.Common.Commands.ICommandBus).Assembly,          // Api.Common
        typeof(Api.Contract.Commands.CreateNewsCommand).Assembly  // Api.Contract
     )
    // ICommandHandler<,>
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    // IQueryHandler<,>
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    // ISingleQueryHandler<,>
    .AddClasses(classes => classes.AssignableTo(typeof(ISingleQueryHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    // �Զ�ע������ҵ����� (���� Api.BLL.Services �����ռ��µ���)
    .AddClasses(classes => classes.InNamespaceOf<NewsService>())
        .AsMatchingInterface()
        .WithScopedLifetime()
    .AddClasses(classes => classes.InNamespaceOf<StockBaseService>())
        .AsMatchingInterface()
        .WithScopedLifetime()
);

// Register decorators (Scrutor  Decorate ? open-generic)
// ע��װ�����ĵ���˳���Ӱ������㼶�������ϸ��� SimpleInjector ����Ϊ��������Ҫ�������� Decorate ����˳��
builder.Services.Decorate(typeof(ICommandHandler<,>), typeof(ActionNameCommandHandlerDecorator<,>));
builder.Services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
builder.Services.Decorate(typeof(ICommandHandler<,>), typeof(TransactionCommandHandlerDecortator<,>));
builder.Services.Decorate(typeof(ICommandHandler<,>), typeof(ErrorLogCommandHandlerDecorator<,>));

//builder.Services.Decorate(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>));
builder.Services.Decorate(typeof(ISingleQueryHandler<,>), typeof(ValidationSingleQueryHandlerDecorator<,>));
builder.Services.Decorate(typeof(ISingleQueryHandler<,>), typeof(ErrorLogSingleQueryHandlerDecorator<,>));

builder.Services.AddScoped<UnitOfWorkLeaveSystem>();
builder.Services.AddScoped(typeof(RepositoryBase<>), typeof(LeaveSystemDBRepository<>));
builder.Services.AddScoped(typeof(LeaveSystemDBRepository<>), typeof(LeaveSystemDBRepository<>));
builder.Services.AddScoped<IAppLogger, SeriLogger>();
//builder.Services.AddSingleton<IMapper, MemoryEventBus>();

//builder.Services.AddScoped<INewsService, NewsService>();

// Authentication - JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection.GetValue<string>("Key");
var issuer = jwtSection.GetValue<string>("Issuer");
var audience = jwtSection.GetValue<string>("Audience");
if (string.IsNullOrEmpty(key))
{
    // For development only - generate a temporary key
    key = "ThisIsADevelopmentSigningKeyDontUseInProd!";
}
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer ?? "DevIssuer",
            ValidAudience = audience ?? "DevAudience",
            IssuerSigningKey = signingKey
        };
    });

// Health checks
builder.Services.AddHealthChecks();

// �� Build ��Run ǰ��ȫ�� DIContainer
var app = builder.Build();

// Configure AppContextHelper
AppContextHelper.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());

// ��ȫ��������������ȷ������Ҫʹ�� DIContainer ǰִ�У�
Api.Common.DI.DIContainer.Instance = new Api.Common.DI.ServiceProviderDIContainer(app.Services);

// ��ʼ�� MapperProvider
MapperProvider.Instance = app.Services.GetRequiredService<IMapper>();


app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �����м�� / ·������
app.MapControllers();
app.MapHealthChecks("/health");

// Ensure the database is created before the app starts
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<LeaveSystemDBContext>();
        db.Database.EnsureCreated();
        SeedInitialData(db);
        Log.Information("Database ensured created");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred creating the DB.");
        throw;
    }
}

app.Run();

static void SeedInitialData(LeaveSystemDBContext db)
{
    var usersToSeed = new[]
    {
        new User { Username = "E1001", Role = "Employee" },
        new User { Username = "E1002", Role = "Manager" },
        new User { Username = "E1003", Role = "Admin" }
    };

    foreach (var user in usersToSeed)
    {
        var exists = db.Users.Any(x => x.Username == user.Username);
        if (!exists)
        {
            db.Users.Add(user);
        }
    }

    db.SaveChanges();

    var employee = db.Users.FirstOrDefault(x => x.Username == "E1001" && x.Role == "Employee");
    if (employee == null)
    {
        return;
    }

    var hasAnnualQuota = db.LeaveQuotas.Any(x => x.UserId == employee.Id && x.LeaveType == "Annual");
    if (!hasAnnualQuota)
    {
        db.LeaveQuotas.Add(new LeaveQuota
        {
            UserId = employee.Id,
            LeaveType = "Annual",
            TotalDays = 10m,
            RemainingDays = 10m,
            UpdatedAt = DateTime.Now,
            User = employee
        });

        db.SaveChanges();
    }
}
