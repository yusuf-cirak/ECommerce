using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Persistance.Contexts
{
    public class ETradeDbContext:DbContext
    {
        public ETradeDbContext(DbContextOptions options):base(options)
        {
            /* IoC Container'a Context eklenecek bu yüzden ayarların base'e gönderilmesi gerekir.  */
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // ChangeTracker : Entityler üzerinde yapılan değişikliklerin ya da yeni eklenen verilerin yakalanmasını sağlayan propertydir. Update operasyonlarında track edilen verileri yakalayıp elde etmemizi sağlar.

            var datas = ChangeTracker.Entries<BaseEntity>(); // ChangeTracker prop'u sayesinde BaseEntity türünde olan (Product,Order,Customer) sınıfın entries değerleri alınır.
            foreach (var data in datas)
            {
                _ = data.State switch // _= kullandığımızda bellekte herhangi bir değişken oluşturulmaz sadece işlem yapılır.
                {
                    EntityState.Added => data.Entity.CreatedTime = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedTime = DateTime.UtcNow,
                    _=>DateTime.UtcNow //
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
