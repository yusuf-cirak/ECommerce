using ECommerce.Application.Repositories.CompletedOrder;
using ECommerce.Persistance.Contexts;

namespace ECommerce.Persistance.Repositories.CompletedOrder;

public sealed class CompletedOrderReadRepository:ReadRepository<Domain.Entities.CompletedOrder>,ICompletedOrderReadRepository
{
    public CompletedOrderReadRepository(ETradeDbContext context) : base(context)
    {
    }
}