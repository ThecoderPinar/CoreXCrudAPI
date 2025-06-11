using CoreXCrud.Data;
using CoreXCrud.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AspNetCoreRateLimit;
using Serilog;
using CoreXCrud.Mappings;
using CoreXCrud.Validators.UserValidators;

var builder = WebApplication.CreateBuilder(args);

// 📌 1️⃣ Serilog Loglama Konfigürasyonu
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 📌 2️⃣ Veritabanı Bağlantısını Yapılandırma
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 📌 3️⃣ Repository & Unit of Work Bağımlılıklarını Ekleyelim
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 📌 4️⃣ AutoMapper ve FluentValidation Servislerini Ekleyelim
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<UserValidator>();
});

// 📌 5️⃣ Rate Limiting Servislerini Ekleyelim
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// 📌 6️⃣ JWT Authentication’ı Ekleyelim
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

builder.Services.AddAuthorization();

// 📌 7️⃣ Swagger (API Dokümantasyonu) ve Yetkilendirme Ayarlarını Ekleyelim
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreXCrud API", Version = "v1" });

    // 📌 Swagger UI için JWT Yetkilendirme Butonu Ekleyelim
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token formatında 'Bearer {token}' şeklinde giriniz."
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
            new string[] {}
        }
    });
});

var app = builder.Build();

// 📌 8️⃣ Geliştirme Ortamında Swagger UI’yi Aç
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 📌 9️⃣ HTTPS Yönlendirme
app.UseHttpsRedirection();

// 📌 🔟 Yetkilendirme Mekanizmasını Kullan
app.UseAuthentication();
app.UseAuthorization();

// 📌 1️⃣1️⃣ Rate Limiting Middleware’i Aktif Hale Getir
app.UseIpRateLimiting();

// 📌 1️⃣2️⃣ Serilog Middleware’i Aktif Hale Getir
app.UseSerilogRequestLogging();

// 📌 1️⃣3️⃣ Controller Endpoint'lerini Tanımla
app.MapControllers();

// 📌 1️⃣4️⃣ Uygulamayı Başlat
app.Run();