using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace ECommerce.SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication app)
        {
            app.MapHub<ProductHub>("/products-hub"); // Product ile ilgili socket işlemlerini üstlenecek route

            app.MapHub<OrderHub>("/orders-hub");
        }
    }
}
