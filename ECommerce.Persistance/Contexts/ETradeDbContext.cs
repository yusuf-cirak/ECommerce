using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = ECommerce.Domain.Entities.File;

namespace ECommerce.Persistance.Contexts
{
    public class ETradeDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public ETradeDbContext(DbContextOptions options):base(options)
        {
            /* IoC Container'a Context eklenecek bu yüzden ayarların base'e gönderilmesi gerekir.  */
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Table Per Hierarchy Tables
        public DbSet<File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles  { get; set; }


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
