using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Services.Authentications;
using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Basket;
using ECommerce.Application.Repositories.BasketItem;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Persistance.Contexts;
using ECommerce.Persistance.Repositories;
using ECommerce.Persistance.Repositories.Basket;
using ECommerce.Persistance.Repositories.BasketItem;
using ECommerce.Persistance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            // Context
            services.AddDbContext<ETradeDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));

            // Identity
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ETradeDbContext>();

            // Repositories

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();

            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();

            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();



            services.AddHttpClient(); // facebook login'de http istekleri atılıyor bu yüzden api'ye httpClient eklenmeli.

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IBasketService, BasketService>();

            services.AddScoped<IOrderService, OrderService>();


            //services.AddScoped<IInternalAuthentication, AuthService>();
            //services.AddScoped<IExternalAuthentication, AuthService>();








        }
    }
}
