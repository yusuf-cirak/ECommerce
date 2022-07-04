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
    ValidateAudience = true, // Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanacaðýný belirttiðimiz deðerdir. -> www.bilmemne.com
    ValidateIssuer = true, // Oluþturulacak token deðerini kimin daðýttýðýný ifade ettiðimiz alandýr. -> www.myapi.com
    ValidateLifetime = true, // Oluþturulan token deðerinin süresini kontrol eden yapýdýr.
    ValidateIssuerSigningKey = true, // Üretilecek token deðerinin uygulamamýza ait olduðunu belirten bir security key verisinin doðrulanmasýdýr.
    
    ValidAudience = builder.Configuration["Token:Audience"],
    ValidIssuer = builder.Configuration["Token:Issuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
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
