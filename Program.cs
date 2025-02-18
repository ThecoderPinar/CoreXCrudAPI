using CoreXCrud.Data;
using CoreXCrud.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using CoreXCrud.DTOs;
using CoreXCrud.Validators;

var builder = WebApplication.CreateBuilder(args);

// 📌 1️⃣ Veritabanı Bağlantısını Yapılandırma
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 📌 2️⃣ Repository & Unit of Work Bağımlılıklarını Ekleyelim
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 📌 3️⃣ AutoMapper ve FluentValidation Servislerini Ekleyelim
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<UserValidator>();
});

// 📌 4️⃣ Swagger (API Dokümantasyonu) Ayarlarını Ekleyelim
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 📌 5️⃣ Geliştirme Ortamında Swagger UI’yi Aç
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 📌 6️⃣ HTTPS Yönlendirme
app.UseHttpsRedirection();

// 📌 7️⃣ Yetkilendirme Mekanizmasını Kullan
app.UseAuthorization();

// 📌 8️⃣ Controller Endpoint'lerini Tanımla
app.MapControllers();

// 📌 9️⃣ Uygulamayı Başlat
app.Run();
