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
    ValidateAudience = true, // Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanacaðýný belirttiðimiz deðerdir. -> www.bilmemne.com
    ValidateIssuer = true, // Oluþturulacak token deðerini kimin daðýttýðýný ifade ettiðimiz alandýr. -> www.myapi.com
    ValidateLifetime = true, // Oluþturulan token deðerinin süresini kontrol eden yapýdýr.
    ValidateIssuerSigningKey = true, // Üretilecek token deðerinin uygulamamýza ait olduðunu belirten bir security key verisinin doðrulanmasýdýr.
    
    ValidAudience = builder.Configuration["Token:Audience"],
    ValidIssuer = builder.Configuration["Token:Issuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
    LifetimeValidator = (notBefore,expires,securityToken,validationParameters)=> 
        // expires!=null?expires>DateTime.UtcNow:false
        expires != null && expires > DateTime.UtcNow,
    // LifetimeValidator delegate'inin expires parametresi token'ýmýzýn expire süresine sahiptir. Eðer ki expires null ise false dönecektir, diðer þarta bakýlmasýna bile gerek yok. Fakat expires null deðilse yani token'imiz varsa ayný zamanda expires süresinin DateTime.UtcNow'dan büyük olmasý gerekir.

    NameClaimType = ClaimTypes.Name // Jwt üzerinde Name claimine karþýlýk gelen deðeri User.Identity.Name propertysinden elde edebiliriz.
    
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
