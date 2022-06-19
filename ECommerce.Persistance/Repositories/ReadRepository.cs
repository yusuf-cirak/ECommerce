using ECommerce.Application.Repositories;
using ECommerce.Domain.Entities.Common;
using ECommerce.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistance.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ETradeDbContext _context; // Context'i IoC Container'dan alacağız

        public ReadRepository(ETradeDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public IQueryable<T> GetAll(bool tracking = true) => tracking ? Table : Table.AsNoTracking();
        //var query = Table.AsQueryable(); // Gerektiğinde tabloyu AsNoTracking olarak değiştirebilmemiz için queryable türünde almamız lazım.
        //if (!tracking) query= Table.AsNoTracking();
        //return query;

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
            => tracking ? await Table.FindAsync(id) : await Table.AsNoTracking().FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        //var query =  Table.AsQueryable();
        //if (!tracking) query = Table.AsNoTracking();
        //return await query.FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

        //await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id)); Marker pattern ile BaseEntity'yi markerımız yapıp, sorgulamada rahatça kullanabiliyoruz.

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
            => tracking ? await Table.SingleAsync(method) : await Table.AsNoTracking().SingleAsync(method);

        //var query = Table.AsQueryable();
        //if (!tracking) query = Table.AsNoTracking();

        //return await query.FirstOrDefaultAsync(method);


        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        => tracking ? Table.Where(method) : Table.AsNoTracking().Where(method);
            //var query = Table.Where(method);
            //if (!tracking) query = query.AsNoTracking();
            //return query;
        
    }
}
