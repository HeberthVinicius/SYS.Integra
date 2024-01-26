using EmailService.src.EmailService.Application;
using EmailService.src.EmailService.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SYS.Integra.src.SYS.Integra.Application.Authentication;
using SYS.Integra.src.SYS.Integra.Application.Interfaces;
using SYS.Integra.src.SYS.Integra.Application.Services;
using SYS.Integra.src.SYS.Integra.Domain.Interfaces.Repositories;
using SYS.Integra.src.SYS.Integra.Infraestructure;
using SYS.Integra.src.SYS.Integra.Infraestructure.Repositories;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura��o da cultura padr�o para aceitar datas no formato DD/MM/YYYY
var cultureInfo = new CultureInfo("pt-BR");
cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
Thread.CurrentThread.CurrentCulture = cultureInfo;
Thread.CurrentThread.CurrentUICulture = cultureInfo;

// Adiciona servi�os ao cont�iner.
builder.Services.AddControllers();

// Configura DbContext
builder.Services.AddDbContext<ModelContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnetion")));
builder.Services.AddDbContext<ERPContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("ERPAGConnection")));

// Configura Cors - Compartilhamento de Recursos entre Origens.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Configura��es JWT do appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];
var expireMinutes = Convert.ToInt32(jwtSettings["ExpireMinutes"]);

// Adiciona o servi�o TokenService � inje��o de depend�ncia com as configura��es do appsettings.json
builder.Services.AddSingleton<TokenService>();

// Registra servi�os
builder.Services.AddSingleton<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IPrestadoresRepository, PrestadoresRepository>();
builder.Services.AddScoped<IPrestadoresService, PrestadoresService>();
builder.Services.AddScoped<IBeneficiariosRepository, BeneficiariosRepository>();
builder.Services.AddScoped<IBeneficiariosService, BeneficiariosService>();

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SYS.Integra", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();


// Configura o pipeline de solicita��o HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SYS.Integra v1");
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
        c.OAuthClientId("swagger-client");
        c.OAuthClientSecret("swagger-secret");
        c.OAuthAppName("Swagger UI");
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
