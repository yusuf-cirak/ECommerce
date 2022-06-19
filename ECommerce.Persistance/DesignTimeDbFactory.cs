using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECommerce.Persistance
{
    public class DesignTimeDbContextFactory:IDesignTimeDbContextFactory<ETradeDbContext>
    {
        public ETradeDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ETradeDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);

            return new(dbContextOptionsBuilder.Options);
        }
    }
}
