using ECommerce.Domain.Entities;
using ECommerce.Application.Repositories;
using ECommerce.Persistance.Contexts;

namespace ECommerce.Persistance.Repositories
{
    public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
    {
        public OrderWriteRepository(ETradeDbContext context) : base(context)
        {
        }
    }
}
