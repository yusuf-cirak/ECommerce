using ECommerce.Application.Repositories.BasketItem;
using ECommerce.Persistance.Contexts;

namespace ECommerce.Persistance.Repositories.BasketItem;

public class BasketItemWriteRepository : WriteRepository<Domain.Entities.BasketItem>, IBasketItemWriteRepository
{
    public BasketItemWriteRepository(ETradeDbContext context) : base(context)
    {
    }
}