using System.Security.Claims;
using System.Text;
using ECommerce.Application;
using ECommerce.Application.Validators.Products;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Filters;
using ECommerce.Infrastructure.Services.Storage.Azure;
using ECommerce.Infrastructure.Services.Storage.Local;
using ECommerce.Persistance;
using ECommerce.WebAPI.Configurations.ColumnWriters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices(); //
builder.Services.AddInfrastructureServices(); // 
builder.Services.AddPersistanceServices(); //


//builder.Services.AddStorage(StorageType.Local);

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
// Accept anything from header, accept any method. Only on localhost:4200 and http & https protocol 

// Logging
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL")
        ,"Logs"
        ,needAutoCreateTable:true
        ,columnOptions:new Dictionary<string, ColumnWriterBase>()
        {
            {"message",new RenderedMessageColumnWriter()},
            {"message_template",new MessageTemplateColumnWriter()},
            {"level",new LevelColumnWriter()},
            {"time_stamp",new TimestampColumnWriter()},
            {"exception",new ExceptionColumnWriter()},
            {"log_event",new LogEventSerializedColumnWriter()},
            {"user_name",new UserNameColumnWriter()} // Custom ColumnWriter
        }
    )
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);

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
        expires != null && expires > DateTime.UtcNow,
    // LifetimeValidator delegate'inin expires parametresi token'�m�z�n expire s�resine sahiptir. E�er ki expires null ise false d�necektir, di�er �arta bak�lmas�na bile gerek yok. Fakat expires null de�ilse yani token'imiz varsa ayn� zamanda expires s�resinin DateTime.UtcNow'dan b�y�k olmas� gerekir.

    NameClaimType = ClaimTypes.Name // Jwt �zerinde Name claimine kar��l�k gelen de�eri User.Identity.Name propertysinden elde edebiliriz.
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseCors(); // cors middleware used

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var userName = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", userName);
    await next();
});

app.MapControllers();

app.Run();
