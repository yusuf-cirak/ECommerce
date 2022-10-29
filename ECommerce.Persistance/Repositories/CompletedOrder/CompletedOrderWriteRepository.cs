using ECommerce.Application.Repositories.CompletedOrder;
using ECommerce.Persistance.Contexts;

namespace ECommerce.Persistance.Repositories.CompletedOrder;

public sealed class CompletedOrderWriteRepository:WriteRepository<Domain.Entities.CompletedOrder>,ICompletedOrderWriteRepository
{
    public CompletedOrderWriteRepository(ETradeDbContext context) : base(context)
    {
    }
}