﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Storage;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Infrastructure.Enums;
using ECommerce.Infrastructure.Services;
using ECommerce.Infrastructure.Services.Storage;
using ECommerce.Infrastructure.Services.Storage.Azure;
using ECommerce.Infrastructure.Services.Storage.Local;
using ECommerce.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService,StorageService>();

            services.AddScoped<ITokenHandler, TokenHandler>();

            services.AddScoped<IMailService, MailService>();


        }

        public static void AddStorage<T>(this IServiceCollection services) where T : Storage , IStorage
        {
            services.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection services, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    break;

                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
