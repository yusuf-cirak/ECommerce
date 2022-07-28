using System.Text;
using ECommerce.Application;
using ECommerce.Application.Validators.Products;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Filters;
using ECommerce.Infrastructure.Services.Storage.Azure;
using ECommerce.Infrastructure.Services.Storage.Local;
using ECommerce.Persistance;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices(); //
builder.Services.AddInfrastructureServices(); // 
builder.Services.AddPersistanceServices(); //


//builder.Services.AddStorage(StorageType.Local);

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
// Accept anything from header, accept any method. Only on localhost:4200 and http & https protocol 

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin",opt => opt.TokenValidationParameters = new()
{
    ValidateAudience = true, // Olu�turulacak token de�erini kimlerin/hangi originlerin/sitelerin kullanaca��n� belirtti�imiz de�erdir. -> www.bilmemne.com
    ValidateIssuer = true, // Olu�turulacak token de�erini kimin da��tt���n� ifade etti�imiz aland�r. -> www.myapi.com
    ValidateLifetime = true, // Olu�turulan token de�erinin s�resini kontrol eden yap�d�r.
    ValidateIssuerSigningKey = true, // �retilecek token de�erinin uygulamam�za ait oldu�unu belirten bir security key verisinin do�rulanmas�d�r.
    
    ValidAudience = builder.Configuration["Token:Audience"],
    ValidIssuer = builder.Configuration["Token:Issuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
    LifetimeValidator = (notBefore,expires,securityToken,validationParameters)=> 
        // expires!=null?expires>DateTime.UtcNow:false
        expires != null && expires > DateTime.UtcNow
    // LifetimeValidator delegate'inin expires parametresi token'�m�z�n expire s�resine sahiptir. E�er ki expires null ise false d�necektir, di�er �arta bak�lmas�na bile gerek yok. Fakat expires null de�ilse yani token'imiz varsa ayn� zamanda expires s�resinin DateTime.UtcNow'dan b�y�k olmas� gerekir.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseCors(); // cors middleware used

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
